using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Controllers
{
    public class Facade
    {
        private ControllerEmployee _controllerEmployee;

        public Facade()
        { 
            _controllerEmployee = new ControllerEmployee();
        }
        public bool ValidLogin(string login, string password) => _controllerEmployee.ValidLogin(login, password);

        internal bool ChangeEmployeePassword(string userEmployee, string newPassword)
        {
            return _controllerEmployee.ChangeEmployeePassword(userEmployee, newPassword);
        }

        internal bool CreateEmployee(string login, string name, string document)
        {
            return _controllerEmployee.CreateEmployee(login, name, document);
        }

        internal bool DisableEmployee(string userEmployee)
        {
            return _controllerEmployee.DisableEmployee(userEmployee);
        }

        internal List<string> GetEmployeesUsers()
        {
            return _controllerEmployee.GetEmployeesUsers();
        }
    }
}
