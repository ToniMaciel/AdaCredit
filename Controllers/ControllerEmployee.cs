using AdaCredit.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Controllers
{
    public class ControllerEmployee
    {
        private EmployeeRepository employeeRepository = EmployeeRepository.getInstance();

        internal bool ChangeEmployeePassword(EmployeeEntity employee, string newPassword)
         =>  employeeRepository.ChangeEmployeePassword(employee, newPassword);

        internal EmployeeEntity CreateEmployee(string login, string name, string document)
        {
            return employeeRepository.addEmployee(login, name, document);
        }

        internal bool DisableEmployee(string userEmployee)
        {
            return employeeRepository.DisableEmployee(userEmployee);
        }

        internal List<string> GetEmployeesUsers()
        {
            return employeeRepository.GetEmployees().Select(emp => emp.Username).ToList();
        }

        internal bool ValidLogin(string login, string password) => employeeRepository.IsValidLogin(login, password);

        // TODO: arrumar isso para service
        internal List<EmployeeEntity> GetEmployees(bool isActive) => employeeRepository.GetEmployees().Where(emp => emp.IsActive == isActive).ToList();
        internal EmployeeEntity? GetEmployee(string username) => employeeRepository.GetEmployees().FirstOrDefault(emp => emp.Username == username);
        internal bool UpdateLogin(EmployeeEntity? employee, DateTime now) => employeeRepository.UpdateLogin(employee, now);
    }
}
