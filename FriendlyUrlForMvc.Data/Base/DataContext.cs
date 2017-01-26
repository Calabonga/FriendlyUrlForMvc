using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using FriendlyUrlForMvc.Models;

namespace FriendlyUrlForMvc.Data.Base {
    public abstract class DataContext : DbContext, IDbContext {
        protected DataContext()
            : base("DefaultConnection") {
            Configuration.AutoDetectChangesEnabled = true;
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }

        public SaveChangesResult LastSaveChangesResult { get; private set; }

        public override int SaveChanges() {

            try {
                LastSaveChangesResult = new SaveChangesResult();
                var userName = "Anonymous";
                var principal = Thread.CurrentPrincipal;
                if (principal.Identity.IsAuthenticated) {
                    userName = principal.Identity.Name;
                }

                var createdSourceInfo = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
                var modifiedSourceInfo = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);

                foreach (var entry in createdSourceInfo) {
                    if (entry.Entity.GetType().GetInterfaces().Contains(typeof(Audit))) {
                        var creationDate = DateTime.Now;
                        entry.Property("CreatedAt").CurrentValue = creationDate;
                        entry.Property("CreatedBy").CurrentValue = userName;
                        entry.Property("CreatedAt").CurrentValue = creationDate;
                        entry.Property("UpdatedBy").CurrentValue = userName;

                        LastSaveChangesResult.AddMessage($"ChangeTracker has new entities: {entry.Entity.GetType()}");
                    }
                }
                foreach (var entry in modifiedSourceInfo) {
                    if (entry.Entity.GetType().GetInterfaces().Contains(typeof(IHaveAudit))) {
                        entry.Property("UpdatedAt").CurrentValue = DateTime.Now;
                        entry.Property("UpdatedBy").CurrentValue = userName;

                    }
                    LastSaveChangesResult.AddMessage($"ChangeTracker has modified entities: {entry.Entity.GetType()}");
                }

                return base.SaveChanges();
            }
            catch (DbUpdateException dbEx) {
                Debug.WriteLine("Error: {0}", dbEx.Message);
                ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
                var message = dbEx.InnerException == null ? dbEx.Message : GetErrorMessage(dbEx);
                LastSaveChangesResult.AddMessage(message);
                return base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx) {
                ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
                var sb = new StringBuilder();
                foreach (
                    var validationError in dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors)) {
                    sb.AppendFormat("Code {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                }

                return base.SaveChanges();
            }
        }

        private string GetErrorMessage(Exception exception) {
            var sb = new StringBuilder();
            do {
                sb.AppendLine(exception.Message);
                exception = exception.InnerException;
            } while (exception != null);
            return sb.ToString();
        }
    }
}
