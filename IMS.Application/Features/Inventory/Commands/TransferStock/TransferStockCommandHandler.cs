
using IMS.Application.Abstractions.Persistence;
using IMS.Application.Common.Results;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Inventory.Commands.TransferStock
{
    public sealed class TransferStockCommandHandler : IRequestHandler<TransferStockCommand, Result<int>>
    {
        private readonly IRepository<Product> _products;
        private readonly IRepository<Warehouse> _warehouses;
        private readonly IRepository<Location> _locations;
        private readonly IRepository<StockTransaction> _transactions;
        private readonly IRepository<StockBalance> _balances;
        private readonly IUnitOfWork _uow;

        public TransferStockCommandHandler(
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

        public async Task<Result<int>> Handle(TransferStockCommand cmd, CancellationToken ct)
        {
            // 1) validation of command
            if (cmd.Quantity <= 0)
                return Result<int>.Fail("Quantity must be greater than zero.");

            if(cmd.FromWarehouseId == cmd.ToWarehouseId && cmd.FromLocationId == cmd.ToLocationId)
                return Result<int>.Fail("Source and destination cannot be the same.");

            if(!await _products.AnyAsync(p => p.Id == cmd.ProductId, ct))
                return Result<int>.Fail("Product does not exist.");

            if(!await _warehouses.AnyAsync(w => w.Id == cmd.FromWarehouseId, ct))
                return Result<int>.Fail("Source warehouse does not exist.");
            if(!await _warehouses.AnyAsync(w => w.Id == cmd.ToWarehouseId, ct))
                return Result<int>.Fail("Destination warehouse does not exist.");

            if (cmd.FromLocationId is not null)
            {
                if (!await _locations.AnyAsync(l => l.Id == cmd.FromLocationId && l.WarehouseId == cmd.FromWarehouseId, ct))
                    return Result<int>.Fail("Source location does not exist in the source warehouse.");
            }
            if(cmd.ToLocationId is not null)
            { 
                if(!await _locations.AnyAsync(l => l.Id == cmd.ToLocationId && l.WarehouseId == cmd.ToWarehouseId, ct))
                    return Result<int>.Fail("Destination location does not exist in the destination warehouse.");
            }

            // 2) check stock availability
            var fromBalance = await _balances.FirstOrDefaultAsync(sb =>
                sb.ProductId == cmd.ProductId &&
                sb.WarehouseId == cmd.FromWarehouseId &&
                sb.LocationId == cmd.FromLocationId, ct);

            if(fromBalance is null || fromBalance.QuantityOnHand < cmd.Quantity)
                return Result<int>.Fail("Insufficient stock in the source location.");

            // 3) load/create destination balance
            var toBalance = await _balances.FirstOrDefaultAsync(sb =>
                sb.ProductId == cmd.ProductId &&
                sb.WarehouseId == cmd.ToWarehouseId &&
                sb.LocationId == cmd.ToLocationId, ct);

            if (toBalance is null)
            {
                toBalance = new StockBalance(cmd.ProductId, cmd.ToWarehouseId, cmd.ToLocationId,0);
                await _balances.AddAsync(toBalance, ct);
            }

            // 4) Apply snapshot changes
            try
            {
                fromBalance.ApplyDelta(-cmd.Quantity);
                toBalance.ApplyDelta(cmd.Quantity);
            }
            catch
            {
                return Result<int>.Fail("Error applying stock changes: Not enough stock in the source location.");
            }

            // 5) Create stock transaction record
            var outTx = StockTransaction.CreateOut(
                cmd.ProductId,
                cmd.FromWarehouseId,
                cmd.FromLocationId,
                cmd.Quantity,
                cmd.ReferenceType,
                cmd.ReferenceId
                );

            var inTx = StockTransaction.CreateIn(
                cmd.ProductId,
                cmd.ToWarehouseId,
                cmd.ToLocationId,
                cmd.Quantity,
                unitCost: null, // transfer doesn't create new cost. It is not purchased stock, it is moved stock.
                cmd.ReferenceType,
                cmd.ReferenceId
                );

            // 6) Add transactions
            await _transactions.AddAsync(outTx, ct);
            await _transactions.AddAsync(inTx, ct);

            // 7) Commit unit of work
            await _uow.SaveChangesAsync(ct);  //  XXXXXX  [happen in TransactionBehavior in _uow.CommitTransactionAsync]  
                                              // i put SaveChangesAsync here to make sure that the outTx.Id is generated before returning it.
                                              // Otherwise, if I return outTx.Id before saving changes, it will be 0 because
                                              // it is not yet saved to the database and the Id is generated by the database.  
            return Result<int>.Ok(outTx.Id);
        }
    }
}
