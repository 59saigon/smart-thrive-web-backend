using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SmartThrive.Repositories.Data.Entities;

namespace SWD.SmartThrive.Repositories.Data
{
    public class STDbContext : BaseDbContext
    {
        public STDbContext(DbContextOptions<STDbContext> options) : base(options)
        {
        }

        #region Config 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }
        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
            var strConn = /*config["ConnectionStrings:DB"]*/ config.GetConnectionString("SmartThrive");

            return strConn;
        }
        #endregion

        #region Dbset
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Package> Packages { get; set; } = null!;
        public virtual DbSet<Provider> Provider { get; set; } = null!;
        public virtual DbSet<Session> Sessions { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;

        public virtual DbSet<CourseXPackage> CourseXPackages { get; set; } = null!;

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
                e.Property(x => x.Username).IsRequired();
                e.Property(x => x.Email).IsRequired();
                e.Property(x => x.Password).IsRequired();
                e.Property(x => x.Status);
                e.Property(x => x.Address);
                e.Property(x => x.FirstName);
                e.Property(x => x.LastName);
                e.Property(x => x.FullName);
                e.Property(x => x.DOB).HasColumnType("date");
                e.Property(x => x.Gender);
                e.Property(x => x.Phone);

                e.Property(x => x.CreatedBy);
                e.Property(x => x.CreatedDate).HasColumnType("date");
                e.Property(x => x.LastUpdatedBy);
                e.Property(x => x.LastUpdatedDate).HasColumnType("date");
                e.Property(x => x.IsDeleted);

                e.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .HasConstraintName("FK_User_Role");
            });
            modelBuilder.Entity<Provider>(e =>
            {
                e.ToTable("Provider");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
                e.Property(x => x.CompanyName);
                e.Property(x => x.Website);

                e.Property(x => x.CreatedBy);
                e.Property(x => x.CreatedDate).HasColumnType("date");
                e.Property(x => x.LastUpdatedBy);
                e.Property(x => x.LastUpdatedDate).HasColumnType("date");
                e.Property(x => x.IsDeleted);

                modelBuilder.Entity<User>()
               .HasOne(e => e.Provider)
               .WithOne(p => p.User)
               .HasForeignKey<Provider>(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade); // Optional: specify behavior on delete

            });

            modelBuilder.Entity<Role>(e =>
            {
                e.ToTable("Role");
                e.HasKey(x => x.Id);
                e.Property(x => x.RoleName).IsRequired();

                e.Property(x => x.CreatedBy);
                e.Property(x => x.CreatedDate).HasColumnType("date");
                e.Property(x => x.LastUpdatedBy);
                e.Property(x => x.LastUpdatedDate).HasColumnType("date");
                e.Property(x => x.IsDeleted);
            });

            modelBuilder.Entity<Student>(e =>
            {
                e.ToTable("Student");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
                e.Property(x => x.StudentName);
                e.Property(x => x.Gender);
                e.Property(x => x.DOB).HasColumnType("date");

                e.Property(x => x.CreatedBy);
                e.Property(x => x.CreatedDate).HasColumnType("date");
                e.Property(x => x.LastUpdatedBy);
                e.Property(x => x.LastUpdatedDate).HasColumnType("date");
                e.Property(x => x.IsDeleted);

                e.HasOne(x => x.User)
            .WithMany(x => x.Students)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("FK_User_Student");

            });

            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Category");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
                e.Property(x => x.CategoryName);
                e.Property(x => x.CreatedBy);
                e.Property(x => x.CreatedDate).HasColumnType("date");
                e.Property(x => x.LastUpdatedBy);
                e.Property(x => x.LastUpdatedDate).HasColumnType("date");
                e.Property(x => x.IsDeleted);
            });

            modelBuilder.Entity<Subject>(e =>
            {
                e.ToTable("Subject");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
                e.Property(x => x.SubjectName);
                e.Property(x => x.CreatedBy);
                e.Property(x => x.CreatedDate).HasColumnType("date");
                e.Property(x => x.LastUpdatedBy);
                e.Property(x => x.LastUpdatedDate).HasColumnType("date");
                e.Property(x => x.IsDeleted);


                e.HasOne(x => x.Category)
            .WithMany(x => x.Subjects)
            .HasForeignKey(x => x.CategoryId)
            .HasConstraintName("FK_Category_Subject");
            });

            modelBuilder.Entity<Course>(e =>
            {
                e.ToTable("Course");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
                e.Property(x => x.Code);
                e.Property(x => x.CourseName);
                e.Property(x => x.SoldCourses);
                e.Property(x => x.Description);
                e.Property(x => x.Price).HasColumnType("decimal(18,2)");
                e.Property(x => x.TotalSessions);
                e.Property(x => x.TotalSlots);
                e.Property(x => x.IsApproved);
                e.Property(x => x.Address);
                e.Property(x => x.IsActive);
                e.Property(x => x.StartDate).HasColumnType("date");
                e.Property(x => x.EndDate).HasColumnType("date");

                e.Property(x => x.CreatedBy);
                e.Property(x => x.CreatedDate).HasColumnType("date");
                e.Property(x => x.LastUpdatedBy);
                e.Property(x => x.LastUpdatedDate).HasColumnType("date");
                e.Property(x => x.IsDeleted);



                e.HasOne(x => x.Subject)
            .WithMany(x => x.Courses)
            .HasForeignKey(x => x.SubjectId)
            .HasConstraintName("FK_Subject_Course");


                e.HasOne(x => x.Provider)
            .WithMany(x => x.Courses)
            .HasForeignKey(x => x.ProviderId)
            .HasConstraintName("FK_Provider_Course")
            .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Session>(e =>
            {
                e.ToTable("Session");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
                e.Property(x => x.SessionName);
                e.Property(x => x.LearnDate).HasColumnType("date");
                e.Property(x => x.Title);
                e.Property(x => x.Description);

                e.Property(x => x.CreatedBy);
                e.Property(x => x.CreatedDate).HasColumnType("date");
                e.Property(x => x.LastUpdatedBy);
                e.Property(x => x.LastUpdatedDate).HasColumnType("date");
                e.Property(x => x.IsDeleted);


                e.HasOne(x => x.Course)
            .WithMany(x => x.Sessions)
            .HasForeignKey(x => x.CourseId)
            .HasConstraintName("FK_Course_Session");
            });

            modelBuilder.Entity<Package>(e =>
            {
                e.ToTable("Package");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
                e.Property(x => x.PackageName);
                e.Property(x => x.StartDate).HasColumnType("date");
                e.Property(x => x.EndDate).HasColumnType("date");
                e.Property(x => x.QuantityCourse);
                e.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
                e.Property(x => x.IsActive);

                e.Property(x => x.CreatedBy);
                e.Property(x => x.CreatedDate).HasColumnType("date");
                e.Property(x => x.LastUpdatedBy);
                e.Property(x => x.LastUpdatedDate).HasColumnType("date");
                e.Property(x => x.IsDeleted);

                e.HasOne(x => x.Student)
            .WithMany(x => x.Packages)
            .HasForeignKey(x => x.StudentId)
            .HasConstraintName("FK_Student_Packages");
            });


            modelBuilder.Entity<CourseXPackage>(e =>
            {
                e.ToTable("CourseXPackage");

                e.HasKey(cp => cp.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");

                e.HasOne(cp => cp.Course)
                    .WithMany(c => c.CourseXPackages)
                    .HasForeignKey(cp => cp.CourseId)
                     .OnDelete(DeleteBehavior.Restrict); // Specify NO ACTION on delete;


                e.HasOne(cp => cp.Package)
                    .WithMany(p => p.CourseXPackages)
                    .HasForeignKey(cp => cp.PackageId)
                     .OnDelete(DeleteBehavior.Restrict); // Specify NO ACTION on delete;

                e.Property(x => x.CreatedBy);
                e.Property(x => x.CreatedDate).HasColumnType("date");
                e.Property(x => x.LastUpdatedBy);
                e.Property(x => x.LastUpdatedDate).HasColumnType("date");
                e.Property(x => x.IsDeleted);
            });

            modelBuilder.Entity<Order>(e =>
            {
                e.ToTable("Order");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd().HasDefaultValueSql("NEWId()");
                e.Property(x => x.Amount);
                e.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)"); ;
                e.Property(x => x.PaymentMethod);
                e.Property(x => x.Description);
                e.Property(x => x.Status);

                e.Property(x => x.CreatedBy);
                e.Property(x => x.CreatedDate).HasColumnType("date");
                e.Property(x => x.LastUpdatedBy);
                e.Property(x => x.LastUpdatedDate).HasColumnType("date");
                e.Property(x => x.IsDeleted);

                e.HasOne(x => x.Package)
             .WithMany(x => x.Orders)
             .HasForeignKey(x => x.PackageId)
             .HasConstraintName("FK_Package_Order");
            });
        }
    }
}
