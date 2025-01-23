using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Status { get; set; }

    public virtual DbSet<Transport> Transports { get; set; }

    public virtual DbSet<Voyage> Voyages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("PK__Clients__B5AE4EC88662982E");

            entity.Property(e => e.IdClient).HasColumnName("ID_Client");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.IdDriver).HasName("PK__Drivers__C46ACD347D4A32E5");

            entity.Property(e => e.IdDriver).HasColumnName("ID_Driver");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Surname).HasMaxLength(50);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.IdEmployee).HasName("PK__Employee__D9EE4F36C1F14831");

            entity.Property(e => e.IdEmployee).HasColumnName("ID_Employee");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.Surname).HasMaxLength(50);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PK__Orders__EC9FA955ECA797CE");

            entity.Property(e => e.IdOrder).HasColumnName("ID_Order");
            entity.Property(e => e.ClientId).HasColumnName("Client_ID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.OrderNum).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(30);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.IdReport).HasName("PK__Reports__C624529442AAA7BD");

            entity.Property(e => e.IdReport).HasColumnName("ID_Report");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.ReportContent).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Roles__43DCD32DC24C75EA");

            entity.Property(e => e.IdRole).HasColumnName("ID_Role");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("Role");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PK__Status__5AC2A734DA2FC081");

            entity.ToTable("Status");

            entity.Property(e => e.IdStatus).HasColumnName("ID_Status");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("Status");
        });

        modelBuilder.Entity<Transport>(entity =>
        {
            entity.HasKey(e => e.IdTransport).HasName("PK__Transpor__10264A73F912F136");

            entity.Property(e => e.IdTransport).HasColumnName("ID_Transport");
            entity.Property(e => e.Maker).HasMaxLength(50);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.StNumber).HasMaxLength(50);
            entity.Property(e => e.StatusId).HasColumnName("Status_ID");
        });

        modelBuilder.Entity<Voyage>(entity =>
        {
            entity.HasKey(e => e.IdVoyage).HasName("PK__Voyages__68C1DAB9A3EB81D2");

            entity.Property(e => e.IdVoyage).HasColumnName("ID_Voyage");
            entity.Property(e => e.ArrivalPoint).HasMaxLength(255);
            entity.Property(e => e.DriverId).HasColumnName("Driver_ID");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.SendPoint).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(30);
            entity.Property(e => e.TransportId).HasColumnName("Transport_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
