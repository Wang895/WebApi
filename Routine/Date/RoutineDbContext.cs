using Microsoft.EntityFrameworkCore;
using Routine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Date
{
    public class RoutineDbContext:DbContext
    {
        public RoutineDbContext(DbContextOptions<RoutineDbContext> options):base(options)
        {

        }
        public DbSet<Company> Companies { set; get; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().Property(x => x.Name).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Company>().Property(x => x.Introduction).HasMaxLength(500);
            modelBuilder.Entity<Employee>().Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(x => x.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(x => x.EmployeeNo).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Employee>().HasOne(x => x.Company).WithMany(x => x.Employees).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = Guid.Parse("5067c167-122d-464c-ac95-40322fae31d2"),
                    Name = "Microsoft",
                    Introduction = "Great Company",
                },
                new Company
                {
                    Id = Guid.Parse("f0ba734d-c62b-4e28-8d54-9e4c2d7dd94e"),
                    Name = "ALIBABA",
                    Introduction = "alipay",
                },
                new Company
                {
                    Id = Guid.Parse("2f9e2e78-96b1-4203-b4e9-87764f8ee601"),
                    Name = "Google",
                    Introduction = "Do not be evil",
                }
            );
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = Guid.NewGuid(),
                    CompanyId = Guid.Parse("2f9e2e78-96b1-4203-b4e9-87764f8ee601"),
                    LastName = "李",
                    FirstName = "光",
                    DateOfBirth = new DateTime(1994, 10, 14),
                    EmployeeNo = "1",
                    Gender = Gender.男,
                },
                 new Employee
                 {
                     Id = Guid.NewGuid(),
                     CompanyId = Guid.Parse("2f9e2e78-96b1-4203-b4e9-87764f8ee601"),
                     LastName = "张",
                     FirstName = "三",
                     DateOfBirth = new DateTime(1987, 10, 14),
                     EmployeeNo = "2",
                     Gender = Gender.男
                 },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    CompanyId = Guid.Parse("5067c167-122d-464c-ac95-40322fae31d2"),
                    LastName = "李",
                    FirstName = "四",
                    DateOfBirth = new DateTime(1990, 10, 14),
                    EmployeeNo = "3",
                    Gender = Gender.女,
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    CompanyId = Guid.Parse("5067c167-122d-464c-ac95-40322fae31d2"),
                    LastName = "赵",
                    FirstName = "六",
                    DateOfBirth = new DateTime(1990, 10, 14),
                    EmployeeNo = "4",
                    Gender = Gender.女,
                });
        }
    }
}
