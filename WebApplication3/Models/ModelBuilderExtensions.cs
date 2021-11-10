using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    id = 1,
                    Name = "ARKhan",
                    Email = "ark882703@gmail.com",
                    Department = dept.IT,


                },
                new Employee
                {
                    id = 2,
                    Name = "Ahmed",
                    Email = "Ahmed03@gmail.com",
                    Department = dept.IT,

                }


                );
        }
    }
}
