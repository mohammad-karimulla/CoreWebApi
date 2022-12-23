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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(e => e.DepartmentName)
                    .HasName("UQ__Departme__D949CC34596374E6")
                    .IsUnique();

                entity.Property(e => e.DepartmentID);

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeID);

                entity.Property(e => e.DateOfJoining).HasColumnType("date");

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoFileName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.DepartmentNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Department)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Employees__Depar__4222D4EF");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
