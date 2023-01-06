using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TrainRecord.Infrastructure.Extentions
{
    public static class EntityEntryExtensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
            entry.References.Any(
                r =>
                    r.TargetEntry != null
                    && r.TargetEntry.Metadata.IsOwned()
                    && (
                        r.TargetEntry.State == EntityState.Added
                        || r.TargetEntry.State == EntityState.Modified
                    )
            );
    }
}
