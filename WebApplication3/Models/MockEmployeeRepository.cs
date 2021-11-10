using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class employeeRepository : EmployeeRepository
    {
        private List<Employee> _employeeList;

        public employeeRepository()
        {
            _employeeList = new List<Employee>() {

            new Employee() { id = 1, Name = "Mary", Department = dept.IT, Email = "Maryme@gmail.com"},
            new Employee() { id = 2, Name = "sana", Department = dept.Staff, Email = "sana@gmail.com"},
            new Employee() { id = 3, Name = "Ayesha",Department = dept.IT, Email = "Ayesha@gmail.com"}
        };
        }

        public Employee Add(Employee employee)
        {
            employee.id = _employeeList.Max(e => e.id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.id == id);
            if (employee != null)
            {
                _employeeList.Remove(employee);
            }
            return employee;

        }

        public IEnumerable<Employee>GetAllEmployee()
        {
            return _employeeList;
        }
        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.id == id);
            throw new NotImplementedException();
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.id == employeeChanges.id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }
    }
}
