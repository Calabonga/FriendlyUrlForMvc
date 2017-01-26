using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FriendlyUrlForMvc.Models;

namespace FriendlyUrlForMvc.Data.Configurations {

    /// <summary>
    /// EditablePage Model Configuration
    /// </summary>
    public class EditablePageModelConfiguration : EntityTypeConfiguration<EditablePage> {

        public EditablePageModelConfiguration() {

            ToTable("EditablePage");

            HasKey(x => x.Id);

            Property(x => x.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasMaxLength(512);
            Property(x => x.Keywords).HasMaxLength(1024).IsRequired();
            Property(x => x.Description).HasMaxLength(4096).IsRequired();
            Property(x => x.Content).HasMaxLength(16384);
            Property(x => x.CreatedAt).IsRequired();
            Property(x => x.CreatedBy).HasMaxLength(50).IsRequired();
            Property(x => x.UpdatedAt).IsOptional();
            Property(x => x.UpdatedBy).HasMaxLength(50).IsOptional();
        }
    }
}