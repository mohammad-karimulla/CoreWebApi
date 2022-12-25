using Microsoft.EntityFrameworkCore;

namespace DBWebAPI.Models.DataFirst
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

        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=LP001512;Database=OrgDataFirst;Trusted_Connection=True;");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Departments>(entity =>
            {
                entity.HasKey(e => e.DepartmentID)
                    .HasName("PK__Departme__B2079BCDA433BB5A");

                entity.HasIndex(e => e.DepartmentName)
                    .HasName("UQ__Departme__D949CC34596374E6")
                    .IsUnique();

                entity.Property(e => e.DepartmentID);

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeID)
                    .HasName("PK__Employee__7AD04FF16A663D8C");

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
                    .HasConstraintName("FK__Employees__Depar__49C3F6B7");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
