using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TrainRecord.Infrastructure.Extentions
{
    public static class EntityEntryExtensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry)
        {
            foreach (var reference in entry.References)
            {
                var target = reference.TargetEntry;
                if (target?.Metadata.IsOwned() == true && target.AddedOrModified())
                {
                    return true;
                }
            }

            return false;
        }

        public static bool AddedOrModified(this EntityEntry entry)
        {
            var states = new EntityState[2] { EntityState.Added, EntityState.Modified };
            return states.Contains(entry.State);
        }

        public static bool AnyChange(this EntityEntry entry)
        {
            var states = new EntityState[3]
            {
                EntityState.Added,
                EntityState.Modified,
                EntityState.Deleted
            };
            return states.Contains(entry.State);
        }
    }
}
