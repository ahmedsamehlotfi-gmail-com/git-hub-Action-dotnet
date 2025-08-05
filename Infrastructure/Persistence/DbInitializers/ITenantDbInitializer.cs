using Infrastructure.Tenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.DbInitializers
{
    public interface ITenantDbInitializer
    {
        Task InitializerDatabaseWithTenantAsync( CancellationToken cancellationToken);
        Task InitializeApplicationDbForTenantAsync( ABCSchoolTenantInfo tenant , CancellationToken cancellationToken);
        public Task InitializeDatabaseAsync(CancellationToken cancellationToken);
    }
}
