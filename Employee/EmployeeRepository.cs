using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AdaCredit.Employee
{
    public class EmployeeRepository
    {
        private static EmployeeRepository? instance;
        private List<EmployeeEntity> employees = new();
        private string pathDir = BuildPathDir(Environment.CurrentDirectory);
        private string pathFile = String.Empty;

        private EmployeeRepository() 
        {
            if (!Directory.Exists(pathDir))
                Directory.CreateDirectory(pathDir);

            this.pathFile = pathDir + Path.DirectorySeparatorChar + "employees.csv";

            if (!File.Exists(pathFile))
                Save();
            else
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

        private static string BuildPathDir(string currentDirectory)
        {
            var position = currentDirectory.LastIndexOf("bin");
            var baseDir = currentDirectory[..(position)];
            return baseDir + "Employee" + Path.DirectorySeparatorChar + "Resources";
        }
        internal void Save()
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
        internal List<EmployeeEntity> GetEmployees()
        {
            return this.employees;
        }
        internal List<EmployeeEntity> GetActiveEmployees()
        {
            return this.employees.Where(emp => emp.IsActive).ToList();
        }
        internal bool AddEmployee(EmployeeEntity newEmployee)
        {
            try
            {
                this.employees.Add(newEmployee);
                Save();
                return true;
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
