using System.Data.Entity.ModelConfiguration;
using FriendlyUrlForMvc.Models;

namespace FriendlyUrlForMvc.Data.Configurations {
/// <summary>
/// FriendlyUrl Model Configuration
/// </summary>
public class FriendlyUrlModelConfiguration : EntityTypeConfiguration<FriendlyUrl> {
    public FriendlyUrlModelConfiguration() {

        ToTable("FriendlyUrl");

        HasKey(x => x.Id);

        Property(x => x.ActionName).HasMaxLength(256).IsRequired();
        Property(x => x.ControllerName).HasMaxLength(256).IsRequired();
        Property(x => x.PageId).IsOptional();
        Property(x => x.Permalink).HasMaxLength(512).IsRequired();
        HasOptional(x => x.Page);
    }
}
}