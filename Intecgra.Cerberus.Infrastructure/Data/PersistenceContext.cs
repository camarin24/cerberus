using Intecgra.Cerberus.Domain.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Intecgra.Cerberus.Infrastructure.Data
{
    public class PersistenceContext : DbContext
    {
        public PersistenceContext()
        {
        }

        public PersistenceContext(DbContextOptions<PersistenceContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp").HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("application", "auth");

                entity.Property(e => e.ApplicationId)
                    .HasColumnName("application_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("client", "auth");

                entity.Property(e => e.ClientId)
                    .HasColumnName("client_id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<ClientApplication>(entity =>
            {
                entity.ToTable("client_application", "auth");

                entity.Property(e => e.ClientApplicationId).HasColumnName("client_application_id");

                entity.Property(e => e.ApplicationId).HasColumnName("application_id");

                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ClientApplications)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("client_application_application_id_fkey");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientApplications)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("client_application_client_id_fkey");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permission", "auth");

                entity.Property(e => e.PermissionId).HasColumnName("permission_id");

                entity.Property(e => e.ApplicationId).HasColumnName("application_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("name");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.Permissions)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("permission_application_id_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user", "auth");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasColumnName("name");
                
                entity.Property(e => e.Salt)
                    .HasColumnName("salt");
                
                entity.Property(e => e.Password)
                    .HasColumnName("password");

                entity.Property(e => e.Picture)
                    .HasColumnType("character varying")
                    .HasColumnName("picture");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_client_id_fkey");
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.ToTable("user_permission", "auth");

                entity.Property(e => e.UserPermissionId).HasColumnName("user_permission_id");

                entity.Property(e => e.PermissionId).HasColumnName("permission_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.UserPermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_permission_permission_id_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPermissions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_permission_user_id_fkey");
            });
        }
    }
}