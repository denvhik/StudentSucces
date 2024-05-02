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
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("User");

        OnModelCreatingPartial(builder);

    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

