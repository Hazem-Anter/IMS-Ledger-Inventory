
using IMS.Application.Abstractions.Persistence;
using IMS.Application.Abstractions.Transaction;
using MediatR;

namespace IMS.Application.Common.Behaviors
{ 
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
            if (request is not ITransactionalCommand)
                return await next();

            if (_uow.HasActiveTransaction)
                return await next();

            await _uow.BeginTransactionAsync(ct);

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
