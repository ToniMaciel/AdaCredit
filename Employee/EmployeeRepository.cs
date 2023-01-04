using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using static BCrypt.Net.BCrypt;
using Bogus;

namespace AdaCredit.Employee
{
    public class EmployeeRepository
    {
        private static EmployeeRepository? instance;
        private List<EmployeeEntity> employees = new();
        private string pathFile = BuildPathFile(Environment.CurrentDirectory);

        private EmployeeRepository() 
        {
            // TODO: mesmo rolê feito em cliente
            if (!File.Exists(pathFile))
            {
                Save();
            } else
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    IncludePrivateMembers = true,
                };

                using var reader = new StreamReader(pathFile);
                using var csv = new CsvReader(reader, config);
                    employees = csv.GetRecords<EmployeeEntity>().ToList();
            }
        }

        public static EmployeeRepository getInstance()
        {
            instance ??= new EmployeeRepository();
            return instance;
        }

        private static string BuildPathFile(string currentDirectory)
        {
            var baseDir = currentDirectory.Split("bin")[0];
            return baseDir + "Employee" + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar + "employees.csv";
        }

        internal bool addEmployee(string login, string name, string document)
        {
            var newSalt = new Faker().Random.Int().ToString();
            var newEmployee = new EmployeeEntity(login, name, document, HashPassword(newSalt + "pass"), newSalt);
            
            try
            {
                this.employees.Add(newEmployee);
                Save();
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        private void Save()
        {
            try
            {
                using var writer = new StreamWriter(pathFile);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(employees);
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        internal bool IsValidLogin(string login, string password)
        {
            var activeEmployees = employees.Where(emp => emp.IsActive).ToList();

            if(activeEmployees.Count == 0 && login == "user" && password == "pass")
                    return true;
            
            var employee = activeEmployees.FirstOrDefault(x => x.Username == login);
            
            if(employee != null && Verify(employee.Salt+password, employee.Hash))
                    return true;

            return false;
        }

        internal List<EmployeeEntity> GetEmployees()
        {
            return this.employees;
        }

        internal bool ChangeEmployeePassword(string userEmployee, string newPassword)
        {
            var employee = employees.FirstOrDefault(x => x.Username == userEmployee);

            if (employee != null)
            {
                employee.UpdateHash(HashPassword(employee.Salt + newPassword));
                Save();
                return true;
            }

            return false;
        }

        internal bool DisableEmployee(string userEmployee)
        {
            var employee = employees.FirstOrDefault(x => x.Username == userEmployee);

            if (employee != null)
            {
                employee.Disable();
                Save();
                return true;
            }

            return false;
        }
    }
}
