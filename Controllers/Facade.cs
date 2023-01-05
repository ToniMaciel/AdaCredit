namespace AdaCredit.Controllers
{
    public class Facade
    {
        private ControllerEmployee _controllerEmployee;
        private ControllerClient _controllerClient;
        private ControllerTransaction _controllerTransaction;

        public Facade()
        { 
            _controllerEmployee = new ControllerEmployee();
            _controllerClient = new ControllerClient();
            _controllerTransaction = new ControllerTransaction();
        }

        internal ControllerClient GetClientController()
        {
            return this._controllerClient;
        }

        internal ControllerEmployee GetEmployeeControler()
        {
            return this._controllerEmployee;
        }

        internal ControllerTransaction GetTransactionController()
        {
            return this._controllerTransaction;
        }
    }
}
