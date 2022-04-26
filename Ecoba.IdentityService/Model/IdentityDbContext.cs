namespace Ecoba.IdentityService.Model;

using System;
using Microsoft.EntityFrameworkCore;

using Ecoba.IdentityService.Common.Enum;
using Ecoba.IdentityService.Common.Helper;
public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {

    }

    public DbSet<User> User { get; set; }
    public DbSet<UserRole> UserRole { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CreatePasswordHash("123#ecoba", out string passwordHash, out string passwordSalt);
        modelBuilder.Entity<User>()
            .HasData(
            new User
            {
                EmployeeId = "000",
                DisplayName = "Quản trị viên",
                Username = "admin",
                Mail = "it@ecoba.com.vn",
                PasswordHash = passwordHash,
                PasswordSatl = passwordSalt,
                Editable = false,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            }
        );

        modelBuilder.Entity<UserRole>()
            .Property(b => b.Id)
            .HasIdentityOptions(startValue: 2);

        modelBuilder.Entity<UserRole>()
            .HasData(
            new UserRole
            {
                Id = 1,
                Username = "admin",
                Role = Enum.GetName(typeof(RoleType), RoleType.Admin),
                Editable = false
            }
        );
    }

    private void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
    {
        passwordSalt = Encryption.Md5Hash(DateTime.UtcNow.ToString());
        passwordHash = Encryption.Md5Hash(passwordSalt + password);
    }
}
