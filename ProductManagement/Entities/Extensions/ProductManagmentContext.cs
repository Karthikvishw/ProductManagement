using System;
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
                await base.SaveChangesAsync(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
