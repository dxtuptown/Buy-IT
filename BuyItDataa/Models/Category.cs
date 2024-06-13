using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyItData.Models
{
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        [Display(Name = "Category Name")]
        public int CategoryID { get; set; }
        [Display(Name = "Category Name")]
        [Required]
        public string CategoryName { get; set; }
        [Required]
        [Display(Name = "Display Order")]
        [Range(1, 100,
            ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public string DisplayOrder { get; set; }
        [Display(Name = "Date Time")]
        public DateTime DateTime { get; set; } = DateTime.Now;
        public ICollection<Product>? Products { get; set; }
    }
}

/* CodeFirst

        Nếu set quan hệ Specialization - Doctor là 1-n:

        1. Tạo hai class và viết các thuộc tính ra trước 

        2. Tại class Doctor với các thuộc tính được yêu cầu thêm dòng(luôn có dấu hỏi ở các thuộc tính từ khóa ngoại):
            [ForeignKey(nameof(SpecializationID))]
            public int SpecializationID { get; set; }
            public virtual Specialization? Specialization { get; set; }

        3. Tại class Specialization với các thuộc tính đã yêu cầu thêm dòng: 
            public ICollection<Doctor>? Doctors { get; set; } ở dưới thuộc tính và
        
            public Specialization() 
            {
                this.Doctors = new HashSet<Doctor>();
            }

        4. Tạo appsetting.json chuot phai vao appsettings.json | Properties, select Copy always  rồi thêm dòng:
            {
                "ConnectionStrings": {
                    "DefaultConnection": "Server=.;Database=FE_FA23;Trusted_Connection=True;TrustServerCertificate=True"
                }
            }

        5. Cài package: Core, SqlServer, Design, Configuration, Json, Tools và reference Web project tới Library

        6. Tạo biến và các đối tượng trong class MyDbContext : DbContext 
        {
            public DbSet<Specialization> Specializations { get; set; }
            public DbSet<Doctor> Doctors { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true);
                IConfiguration configuration = builder.Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Specialization>()
                    .Property(s => s.SpecializationName)
                    .IsRequired()
                    .HasMaxLength(50);
                modelBuilder.Entity<Specialization>()
                    .HasData(
                    new Specialization { SpecializationID = 1, SpecializationName = "Internal Medicine" },
                    new Specialization { SpecializationID = 2, SpecializationName = "Pediatrics" },
                    new Specialization { SpecializationID = 3, SpecializationName = "Dentistry" }
                    );
                modelBuilder.Entity<Doctor>()
                    .Property(d => d.DoctorName)
                    .IsRequired()
                    .HasMaxLength(50);
                modelBuilder.Entity<Doctor>()
                    .HasData(
                    new Doctor { DoctorID = 1, DoctorName = "Khanh Dat", SpecializationID = 1, Address = "Quang Nam" },
                    new Doctor { DoctorID = 2, DoctorName = "Quang Trieu", SpecializationID = 2, Address = "Da Nang" },
                    new Doctor { DoctorID = 3, DoctorName = "Dinh Phuc", SpecializationID = 3, Address = "Hue" },
                    new Doctor { DoctorID = 4, DoctorName = "Hoang Truong", SpecializationID = 3, Address = "Quang Tri" }
                    );

            } 
        }
        7. mở Terminal lên : dotnet ef migrations add "Initial" 
                             dotnet ef database update
        8. Trong program thêm: builder.Services.AddScoped(typeof(MyDbContext));
        *** Đã hoàn thành thư viện ***

*/
