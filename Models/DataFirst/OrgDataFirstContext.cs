using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models.DataFirst
{
    public partial class OrgDataFirstContext : DbContext
    {
        public OrgDataFirstContext()
        {
        }

        public OrgDataFirstContext(DbContextOptions<OrgDataFirstContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=OrganizationAppDataFirstCon");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(e => e.DepartmentName)
                    .HasName("PK_DepName")
                    .IsUnique();

                entity.Property(e => e.DepartmentID).HasColumnName("DepartmentID");

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");

                entity.Property(e => e.DateOfJoining).HasColumnType("date");

                entity.Property(e => e.Department)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoFileName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.DepartmentNavigation)
                    .WithMany(p => p.Employees)
                    .HasPrincipalKey(p => p.DepartmentName)
                    .HasForeignKey(d => d.Department)
                    .HasConstraintName("FK_Employees_Departments");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
