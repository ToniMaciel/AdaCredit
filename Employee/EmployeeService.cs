using Bogus;
using System;
using System.Collections.Generic;
using static BCrypt.Net.BCrypt;
using System.Linq;

namespace AdaCredit.Employee
{
    internal class EmployeeService
    {
        private EmployeeRepository employeeRepository = EmployeeRepository.getInstance();

        internal EmployeeEntity? AddEmployee(string login, string name, string document, string username)
        {
            var newSalt = new Faker().Random.Int().ToString();
            var newEmployee = new EmployeeEntity(login, name, document, HashPassword(newSalt + "pass"), newSalt);

            try
            {
                this.employeeRepository.AddEmployee(newEmployee);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

            return newEmployee;
        }

        internal bool ChangeEmployeePassword(EmployeeEntity employee, string newPassword, string usernameLogged)
        {
            try
            {
                employee.UpdateHash(HashPassword(employee.Salt + newPassword), usernameLogged);
                this.employeeRepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        internal bool DisableEmployee(EmployeeEntity employee, string userLogged)
        {
            try
            {
                employee.Disable(userLogged);
                this.employeeRepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        internal List<EmployeeEntity> GetEmployees()
        {
            return this.employeeRepository.GetEmployees();
        }
        internal bool IsValidLogin(string login, string password)
        {
            var activeEmployees = this.employeeRepository.GetActiveEmployees();

            if (activeEmployees.Count == 0 && login == "user" && password == "pass")
                return true;

            var employee = activeEmployees.FirstOrDefault(x => x.Username == login);

            if (employee != null && Verify(employee.Salt + password, employee.Hash))
                return true;

            return false;
        }

        internal bool UpdateLogin(EmployeeEntity employee, DateTime now)
        {
            try
            {
                employee.UpdateLogin(now);
                this.employeeRepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
