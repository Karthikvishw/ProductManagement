using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public partial class ProductManagementContext : DbContext
    {
        public async Task SaveAsync()
        {
            try
            {
                var selectedEntityList = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);
                foreach (var entity in selectedEntityList)
                {
                    var propertyDeleted = entity.Entity.GetType().GetProperty("Deleted");
                    if( propertyDeleted != null)
                    {
                        propertyDeleted.SetValue(entity.Entity, true, null);
                        entity.State = EntityState.Modified;
                    }
                }
                await base.SaveChangesAsync(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
