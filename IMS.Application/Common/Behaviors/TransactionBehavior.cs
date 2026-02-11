
using IMS.Application.Abstractions.Persistence;
using IMS.Application.Abstractions.Transaction;
using MediatR;

namespace IMS.Application.Common.Behaviors
{
    // This behavior ensures that any command that implements ITransactionalCommand is executed within a transaction scope.
    // If there is already an active transaction, it will simply continue without starting a new one. Otherwise, it will begin a new transaction,
    // execute the command, and commit the transaction if successful, or roll back if an exception occurs.
    public sealed class TransactionBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {

        private readonly IUnitOfWork _uow;

        public TransactionBehavior(IUnitOfWork uow)
        {
            _uow = uow;
        }
                  
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            // 1) Check if the request is a transactional command. If not, simply proceed to the next behavior or handler.
            if (request is not ITransactionalCommand)
                return await next();

            // 2) If there is already an active transaction, proceed without starting a new one.
            // This allows for nested transactions or commands that are part of a larger transaction scope.
            if (_uow.HasActiveTransaction)
                return await next();

            // 3) If there is no active transaction, begin a new transaction scope.
            await _uow.BeginTransactionAsync(ct);


            // 4) Execute the next behavior or handler in the pipeline.
            // If it succeeds, commit the transaction. If it throws an exception,
            // roll back the transaction.
            try
            {
                var response = await next();

                await _uow.CommitTransactionAsync(ct);
                return response;
            }
            catch
            {
                await _uow.RollbackTransactionAsync(ct);
                throw;
            }
        }
    }
}
