using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading;
using System.Threading.Tasks;

namespace FriendlyUrlForMvc.Data.Base {
    public interface IDbContext {

        SaveChangesResult LastSaveChangesResult { get; }

        Database Database { get; }

        DbChangeTracker ChangeTracker { get; }

        DbContextConfiguration Configuration { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbSet Set(Type entityType);

        Task<int> SaveChangesAsync();

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        IEnumerable<DbEntityValidationResult> GetValidationErrors();

        DbEntityEntry Entry(object entity);

        void Dispose();

        string ToString();

        bool Equals(object obj);

        int GetHashCode();

        Type GetType();
    }
}
