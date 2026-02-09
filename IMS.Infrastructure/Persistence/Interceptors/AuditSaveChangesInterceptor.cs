
using IMS.Application.Abstractions.Auth;
using IMS.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace IMS.Infrastructure.Persistence.Interceptors
{
    // This interceptor is responsible for automatically populating audit fields
    // (like CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) on entities that inherit from AuditableEntity.
    public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUser;
        
        public AuditSaveChangesInterceptor (ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        // The SavingChanges method is called synchronously before the changes are saved to the database.
        // DbContextEventData provides access to the DbContext and other relevant information about the event.
        // InterceptionResult<int> allows you to control the flow of the saving process, such as short-circuiting it or modifying the result.
        public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,   
        InterceptionResult<int> result)
        {
            // 1) Call the ApplyAudit method to populate the audit fields on the entities being tracked by the DbContext.
            ApplyAudit(eventData.Context);

            // 2) Call the base implementation of SavingChanges to continue with the normal saving process.
            return base.SavingChanges(eventData, result);
        }

        // The SavingChangesAsync method is the asynchronous counterpart to SavingChanges, allowing for non-blocking operations.
        // It follows the same logic as SavingChanges but is designed to work with asynchronous code patterns.
        // which mean it can be awaited and will not block the calling thread while the operation is in progress.
        // The cancellationToken parameter allows the operation to be cancelled if needed.
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
        {
            ApplyAudit(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        // The ApplyAudit method is responsible for iterating through the tracked entities in the DbContext and populating the audit fields based on their state (Added or Modified).
        private void ApplyAudit(DbContext? context)
        {
            // 1) Check if the DbContext is null. If it is,
            // return early since we cannot apply audit information without a context.
            if (context is null) return;

            // 2) Get the current UTC time, user ID, and user name from the ICurrentUserService.
            // This information will be used to populate the audit fields.
            var now = DateTime.UtcNow;
            var userId = _currentUser.UserId;
            var userName = _currentUser.DisplayName;

            // 3) Iterate through the tracked entities in the DbContext that are of type AuditableEntity.
            // ex : if you have an entity called Product that inherits from AuditableEntity, it will be included in this iteration.
            foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
            {
                // a) Check the state of each entity entry. If the state is Added,
                // it means a new entity is being added to the context,
                // and we need to set the CreatedAt, CreatedByUserId, CreatedByName,
                // UpdatedAt, UpdatedByUserId, and UpdatedByName fields.
                if (entry.State == EntityState.Added)
                {
                    // If the CreatedAt field is not already set (i.e., it has the default value), set it to the current UTC time.
                    if (entry.Entity.CreatedAt == default)
                        entry.Entity.CreatedAt = now;

                    // If the CreatedByUserId and CreatedByName fields are not already set,
                    // set them to the current user's ID, and name, respectively.
                    entry.Entity.CreatedByUserId ??= userId;
                    entry.Entity.CreatedByName ??= userName;

                    // For new entities, we also set the UpdatedAt, UpdatedByUserId,
                    // and UpdatedByName fields to the same values as the Created fields,
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedByUserId = userId;
                    entry.Entity.UpdatedByName = userName;
                }
                // b) If the state is Modified, it means an existing entity is being updated,
                // and we need to update the UpdatedAt, UpdatedByUserId, and UpdatedByName fields.
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedByUserId = userId;
                    entry.Entity.UpdatedByName = userName;

                    // Since the CreatedAt, CreatedByUserId, and CreatedByName fields should not be modified when an entity is updated,
                    // we explicitly mark them as not modified to prevent any accidental changes to these fields during an update operation.
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    entry.Property(x => x.CreatedByUserId).IsModified = false;
                    entry.Property(x => x.CreatedByName).IsModified = false;
                }
            }

        }
    }
}
