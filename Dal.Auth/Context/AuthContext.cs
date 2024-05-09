using Dal.Auth.Model;
using DalAuth.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dal.Auth.Context;

public partial class AuthContext : IdentityDbContext<User,Roles,Guid>
{
    public AuthContext()
    {

    }
    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
       
    }
    public DbSet<User> User { get; set; } = null!;
    public DbSet<Photo> Photos { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("User");

        builder.Entity<User>()
           .HasMany(u => u.Photo)  
           .WithOne(p => p.User)
           .HasForeignKey(p => p.UserId)
          .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Photo>().ToTable("Photos");
        OnModelCreatingPartial(builder);

    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}