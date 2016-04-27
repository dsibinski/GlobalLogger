using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using GlobalLogger.Demo.Models;
using GlobalLogger.Enums;
using NLog;
using NLog.LayoutRenderers;

namespace GlobalLogger.Demo
{
    public partial class ExamplesForm : Form
    {
        #region Examples - Logger

        private GlobalLogger Logger;

        #endregion Examples - Logger

        public ExamplesForm()
        {
            InitializeComponent();
        }

        private List<Employee> Employees { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Employees = new List<Employee>();
            bdsEmployees.DataSource = Employees;

            // events wiring
            btnCreateCashier.Click += BtnCreateCashierOnClick;
            btnCreateClerk.Click += BtnCreateClerkOnClick;
            btnCreateAccountant.Click += BtnCreateAccountantOnClick;
            btnCreateStoreManager.Click += BtnCreateStoreManagerOnClick;
            btnCreateRandomEmployees.Click += BtnCreateRandomEmployeesOnClick;
            btnSimpleLogger.Click += BtnSimpleLogger_Click;
            btnLogInfo.Click += BtnLogInfo_Click;
            btnTimeLog.Click += BtnTimeLog_Click;
            btnException.Click += BtnException_Click;
            btnNamedLogger.Click += BtnNamedLogger_Click;
            btnComplexLogger.Click += BtnComplexLogger_Click;

        }



        private void AddEmployee(Employee employee)
        {
            Employees.Add(employee);
            bdsEmployees.ResetBindings(false);
        }

        private void BtnCreateRandomEmployeesOnClick(object sender, EventArgs eventArgs)
        {
            for (int i = 0; i < numCreateRandomEmployees.Value; i++)
            {
                AddEmployee(EmployeeGenerator.Generate(DateTime.Now, EmployeeType.Random));
            }

            numCreateRandomEmployees.Value = 1;
        }

        private void BtnCreateStoreManagerOnClick(object sender, EventArgs eventArgs)
        {
            AddEmployee(EmployeeGenerator.Generate(DateTime.Now, EmployeeType.StoreManager));
        }

        private void BtnCreateAccountantOnClick(object sender, EventArgs eventArgs)
        {
            AddEmployee(EmployeeGenerator.Generate(DateTime.Now, EmployeeType.Accountant));
        }

        private void BtnCreateClerkOnClick(object sender, EventArgs eventArgs)
        {
            AddEmployee(EmployeeGenerator.Generate(DateTime.Now, EmployeeType.Clerk));
        }

        private void BtnCreateCashierOnClick(object sender, EventArgs eventArgs)
        {
            AddEmployee(EmployeeGenerator.Generate(DateTime.Now, EmployeeType.Cashier));
        }

        #region Examples methods

        private void BtnSimpleLogger_Click(object sender, EventArgs e)
        {
            // Default - named after calling class
            Logger = new GlobalLogger();
            
           
        }

        private void BtnTimeLog_Click(object sender, EventArgs e)
        {
            Logger.StartTimer("Timer1");
            Thread.Sleep(1000);
            Logger.StopTimer("Timer1", Level.Info, "Operation time: {0}", TimeUnit.Seconds);
        }

        private void BtnLogInfo_Click(object sender, EventArgs e)
        {
            Logger.LogInfo("Some info logging test message.");
        }

        private void BtnException_Click(object sender, EventArgs e)
        {
            var exception1 = new NullReferenceException("The customer passed cannot be null!");
            var exception2 = new ArgumentException("Customer provided is invalid", "Customer", exception1);
            var exception3 = new InvalidOperationException("The customer could not be saved properly!", exception2);

            try
            {
                throw exception3;
            }
            catch (Exception ex)
            {
                Logger = new GlobalLogger();
                Logger.LogException(ex, "Oups, something went wrong:" + Environment.NewLine, Level.Fatal);
                //            Logger.LogException(ex, "Oups, something went wrong:");       
                //            Logger.LogException(ex);
            }
        }

        private void BtnNamedLogger_Click(object sender, EventArgs e)
        {
            Logger = new GlobalLogger("LoggerAndrew");
        }

        #endregion Examples methods


        private void BtnComplexLogger_Click(object sender, EventArgs e)
        {
            var Youenn = new Programmer("Youenn");
            Logger = new GlobalLogger(Youenn);
        }

        class Programmer
        {
            public Programmer(string name)
            {
                Name = name;
            }
            private string Name { get; set; }
        }
    }
}
