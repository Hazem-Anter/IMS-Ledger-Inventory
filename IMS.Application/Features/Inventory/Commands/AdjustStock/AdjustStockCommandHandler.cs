
using IMS.Application.Abstractions.Persistence;
using IMS.Application.Common.Result;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Inventory.Commands.AdjustStock
{
    public sealed class AdjustStockCommandHandler : IRequestHandler<AdjustStockCommand, Result<int>>
    {
        private readonly IRepository<Product> _products;
        private readonly IRepository<Warehouse> _warehouses;
        private readonly IRepository<Location> _locations;
        private readonly IRepository<StockTransaction> _transactions;
        private readonly IRepository<StockBalance> _balances;
        private readonly IUnitOfWork _uow;

        public AdjustStockCommandHandler(
            IRepository<Product> products,
            IRepository<Warehouse> warehouses,
            IRepository<Location> locations,
            IRepository<StockTransaction> transactions,
            IRepository<StockBalance> balances,
            IUnitOfWork uow)
        {
            _products = products;
            _warehouses = warehouses;
            _locations = locations;
            _transactions = transactions;
            _balances = balances;
            _uow = uow;
        }
        public async Task<Result<int>> Handle(AdjustStockCommand cmd, CancellationToken ct)
        {
            // 1) validation of command
            if (cmd.DeltaQuantity == 0)
                return Result<int>.Fail("Delta quantity cannot be zero.");

            if(string.IsNullOrWhiteSpace(cmd.Reason))
                return Result<int>.Fail("Reason is required for stock adjustment.");

            if(!await _products.AnyAsync(p => p.Id == cmd.ProductId, ct))
                return Result<int>.Fail("Product not found.");

            if(!await _warehouses.AnyAsync(w => w.Id == cmd.WarehouseId, ct))
                return Result<int>.Fail("Warehouse not found.");

            if (cmd.LocationId is not null)
            {
                if(!await _locations.AnyAsync(l => l.Id == cmd.LocationId 
                                                    && l.WarehouseId == cmd.LocationId
                                                    ,ct))
                    return Result<int>.Fail("Location not found in the specified warehouse.");
            }

            // 2) Get balance or create new if not exists
            var balance = await _balances.FirstOrDefaultAsync(
                b => b.ProductId == cmd.ProductId &&
                     b.WarehouseId == cmd.WarehouseId &&
                     b.LocationId == cmd.LocationId,
                     ct);

            if(balance is null)
            {
                // No existing balance, only allow positive adjustments
                if (cmd.DeltaQuantity < 0)
                    return Result<int>.Fail("Cannot adjust negative stock when no balance exists.");

                balance = new StockBalance(cmd.ProductId, cmd.WarehouseId, cmd.LocationId, 0);
                await _balances.AddAsync(balance, ct);
            }
            // apply delta to balance
            try
            {
                balance.ApplyDelta(cmd.DeltaQuantity);
            }
            catch
            {
                return Result<int>.Fail("Stock adjustment would result in negative stock balance.");
            }

            // 3) Create stock transaction
            // Ledger adjustment transaction
            // Put reason into reference fields (for now) OR create a separate field later
            var tx = StockTransaction.CreateAdjust(
                cmd.ProductId,
                cmd.WarehouseId,
                cmd.LocationId,
                cmd.DeltaQuantity,
                referenceType: cmd.ReferenceType ?? "adjustment",
                referenceId: cmd.ReferenceId ?? cmd.Reason);

            await _transactions.AddAsync(tx, ct);

            // 4) Save changes
            await _uow.SaveChangesAsync(ct);

            return Result<int>.Ok(tx.Id);

        }
    }
}
