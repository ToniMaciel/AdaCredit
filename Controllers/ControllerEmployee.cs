using AdaCredit.Employee;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdaCredit.Controllers
{
    public class ControllerEmployee
    {
        private EmployeeService employeeService;
        public ControllerEmployee()
        {
            this.employeeService = new EmployeeService();
        }
        internal bool ChangeEmployeePassword(EmployeeEntity employee, string newPassword)
            => employeeService.ChangeEmployeePassword(employee, newPassword);
        internal EmployeeEntity? CreateEmployee(string login, string name, string document) 
            => employeeService.AddEmployee(login, name, document);
        internal bool DisableEmployee(EmployeeEntity employee) 
            => employeeService.DisableEmployee(employee);
        internal List<EmployeeEntity> GetEmployees() 
            => employeeService.GetEmployees();
        internal bool ValidLogin(string login, string password) 
            => employeeService.IsValidLogin(login, password);
        internal List<EmployeeEntity> GetEmployees(bool isActive) 
            => employeeService.GetEmployees().Where(emp => emp.IsActive == isActive).ToList();
        internal EmployeeEntity? GetEmployee(string username) 
            => employeeService.GetEmployees().FirstOrDefault(emp => emp.Username == username);
        internal bool UpdateLogin(EmployeeEntity employee, DateTime now) 
            => employeeService.UpdateLogin(employee, now);
    }
}
