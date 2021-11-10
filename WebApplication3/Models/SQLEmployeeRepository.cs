using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class SQLEmployeeRepository : EmployeeRepository
    {
        private readonly AppDbContext context;

        public SQLEmployeeRepository(AppDbContext context,ILogger<SQLEmployeeRepository>logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public ILogger<SQLEmployeeRepository> logger { get; }

        public Employee Add(Employee employee)
        {
            context.Employee.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
           Employee employee =context.Employee.Find(id);
            if(employee != null)
            {
                context.Employee.Remove(employee);
                context.SaveChanges();
                
            }
            return employee;
        }
        
        public IEnumerable<Employee> GetAllEmployee()
        {
            return context.Employee;
        }

        public Employee GetEmployee(int id)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogError("Warning Log");
            logger.LogCritical("Critical Log");
            return  context.Employee.Find(id);
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee = context.Employee.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employeeChanges;
        }
    }
}