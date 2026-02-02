
using IMS.Application.Abstractions.Persistence;
using IMS.Application.Common.Result;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Inventory.Commands.ReceiveStock
{
    // Handler for processing ReceiveStockCommand to record stock receipt into inventory 
    // and update stock balances accordingly
    // IRequestHandler from MediatR to handle the command and return a Result<int> indicating success or failure
    public sealed class ReceiveStockCommandHandler : IRequestHandler<ReceiveStockCommand, Result<int>>
    {
        private readonly IRepository<Product> _products;
        private readonly IRepository<Warehouse> _warehouses;
        private readonly IRepository<Location> _locations;
        private readonly IRepository<StockTransaction> _Transactions;
        private readonly IRepository<StockBalance> _balance;
        private readonly IUnitOfWork _uow;

        public ReceiveStockCommandHandler(
            IRepository<Product> products,
            IRepository<Warehouse> warehouses,
            IRepository<Location> locations,
            IRepository<StockTransaction> transactions,
            IRepository<StockBalance> balance,
            IUnitOfWork uow)
        {
            _products = products;
            _warehouses = warehouses;
            _locations = locations;
            _Transactions = transactions;
            _balance = balance;
            _uow = uow;
        }
        
        public async Task<Result<int>> Handle(ReceiveStockCommand cmd, CancellationToken ct)
        {
            // (1) Validation for request data 
            if (cmd.Quantity <= 0)
                return Result<int>.Fail("Quantity must be greater than zero.");

            if(!await _products.AnyAsync(p => p.Id == cmd.ProductId, ct))
                return Result<int>.Fail("Product not found");

            if(!await _warehouses.AnyAsync(w => w.Id == cmd.WarehouseId, ct))
                return Result<int>.Fail("Warehouse not found");

            if(cmd.LocationId is not null)
                if (!await _locations.AnyAsync(l => l.Id == cmd.LocationId 
                                                    && l.WarehouseId == cmd.WarehouseId
                                                    , ct))
                    return Result<int>.Fail("Location not found in the specified warehouse");

            // (2) Create stock transaction for receiving stock
            //  a.) Use StockTransaction.CreateIn factory method to create "In" transaction
            var tx = StockTransaction.CreateIn(
                cmd.ProductId,
                cmd.WarehouseId,
                cmd.LocationId,
                cmd.Quantity,
                cmd.UnitCost,
                cmd.ReferenceType,
                cmd.ReferenceId
            );

            //  b.) Add transaction to repository
            await _Transactions.AddAsync(tx, ct);


            // (3) Update stock balance (snapshot)
            // We need to find existing balance row by (ProductId, WarehouseId, LocationId).
            // by getting FirstOrDefaultAsync, if not found, create new balance row

            //    a.) If found, update QuantityOnHand by adding received quantity 
            var balance = await _balance.FirstOrDefaultAsync(
                sb => sb.ProductId == cmd.ProductId
                      && sb.WarehouseId == cmd.WarehouseId
                      && sb.LocationId == cmd.LocationId, ct);

            //    b.) If not found, create new StockBalance with received quantity
            if (balance is null)
            {
                balance = new StockBalance(cmd.ProductId, cmd.WarehouseId, cmd.LocationId, 0);
                await _balance.AddAsync(balance, ct);
            }

            //    c.) Apply the quantity delta to the balance 
            balance.ApplyDelta(cmd.Quantity);


            // (4) Save changes to generate transaction ID. Commit unit of work only once.
            await _uow.SaveChangesAsync(ct);


            return Result<int>.Ok(tx.Id);
        }
    }
}
