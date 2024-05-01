using Dal.Auth.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dal.Auth.Context;

partial class AuthDbContext:IdentityDbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
       
    }
    public DbSet<User> User { get; set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
      
        builder.Entity<User>().ToTable("Users");
        OnModelCreatingPartial(builder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

