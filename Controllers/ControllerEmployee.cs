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

        internal bool ChangeEmployeePassword(string userEmployee, string newPassword)
         =>  employeeRepository.ChangeEmployeePassword(userEmployee, newPassword);

        internal bool CreateEmployee(string login, string name, string document)
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
    }
}
