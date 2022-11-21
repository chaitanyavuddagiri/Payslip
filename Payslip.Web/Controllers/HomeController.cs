using Microsoft.AspNetCore.Mvc;
using Payslip.Web.Models;
using System.Diagnostics;
using System.Globalization;

namespace Payslip.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const int CURRENT_YEAR = 2002;
        private const decimal MAXTAXAMOUNT = 180000;
        private const decimal MAXTAXRATE = 0.39M;
        private const decimal MONTHSINAYEAR = 12M;
        private readonly IEnumerable<TaxRate> taxRates;
        private static List<Employee> employees = new List<Employee>();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            taxRates = new[]
            {
                new TaxRate() {MinAmount=0M,MaxAmount=14000M, Rate=0.105M},
                new TaxRate() {MinAmount=14000M,MaxAmount=48000M, Rate=0.175M},
                new TaxRate() {MinAmount=48000M,MaxAmount=70000M, Rate=0.30M},
                new TaxRate() {MinAmount=70000M,MaxAmount=180000M, Rate=0.33M},
            };
        }

        public IActionResult Index()
        {
            return View(new PayslipViewModel());
        }

        [HttpPost]
        public IActionResult Index(PayslipViewModel viewModel, string submit)
        {
            switch (submit)
            {
                case "Employee":
                    {
                        if (!ModelState.IsValid)
                        {
                            return View(viewModel);
                        }

                        if (viewModel.Employee != null && employees.Find(e => e.FirstName == viewModel.Employee.FirstName && e.LastName == viewModel.Employee.LastName) == null)
                            employees.Add(viewModel.Employee);
                        else
                            ModelState.AddModelError("Employee", "Employee already added to the list");

                        viewModel.Employees = employees;
                        break;
                    }
                case "Payslip":
                    {
                        viewModel.Employees = employees;

                        viewModel = GenerateEmployeePayslip(viewModel);
                        break;
                    }
            }

            // resetting employee fields
            viewModel.Employee = new Employee();
            ModelState.Clear();
            return View(viewModel);
            
        }

        // fills the employee paslips and returns updated view model
        private PayslipViewModel GenerateEmployeePayslip(PayslipViewModel viewModel)
        {
            List<EmployeePayslip> employeePayslips = new List<EmployeePayslip>();

            if(viewModel.Employees != null)
            {
                foreach(var employee in viewModel.Employees)
                {
                    employeePayslips.Add(GetEmployeePayslip(employee));
                }
            }

            viewModel.EmployeePayslips = employeePayslips;
            return viewModel;
        }

        // calculates the employee playsips of each given employee based on the input
        private EmployeePayslip GetEmployeePayslip(Employee employee)
        {
            EmployeePayslip employeePayslip = new EmployeePayslip();
            employeePayslip.FullName = $"{employee.FirstName} {employee.LastName}";
            employeePayslip.PayPeriod = GetPayPeriod(employee.PayPeriod.ToString());
            employeePayslip.GrossIncome = Convert.ToDecimal((employee.AnnualSalary / MONTHSINAYEAR).ToString("0.00"));
            employeePayslip.IncomeTax = Convert.ToDecimal(CalculateIncomeTax(employee.AnnualSalary).ToString("0.00"));
            employeePayslip.NetIncome = Convert.ToDecimal((employeePayslip.GrossIncome - employeePayslip.IncomeTax).ToString("0.00"));
            employeePayslip.Super = Convert.ToDecimal((employeePayslip.GrossIncome * (Convert.ToDecimal(employee.Super) / 100)).ToString("0.00"));

            return employeePayslip;
        }

        // returns the employee pay period baed on the given month returns a string '01 March - 31 March'
        private string GetPayPeriod(string month, int year = CURRENT_YEAR)
        {
            // Get Month number
            int givenMonth = DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month;
            
            // Get First day of a month
            DateTime firstDay = new DateTime(year, givenMonth, 1);
            
            // Get First and Last day of a given month
            string firstDayOfMonth = firstDay.ToString("d MMMM");
            string lastDayOfMomth = firstDay.AddMonths(1).AddSeconds(-1).ToString("d MMMM");
            
            return $"{firstDayOfMonth} - {lastDayOfMomth}";
        }

        // Calculates the tax based on given annual salary and returns monthly tax amount
        private decimal CalculateIncomeTax(decimal annualSalary)
        {
            decimal incomeTax = 0;
            decimal reminingTaxableSalary = annualSalary;
            decimal previousTaxAmount = 0M;

            if (reminingTaxableSalary > 0 && reminingTaxableSalary <= MAXTAXAMOUNT)
            {
                foreach(TaxRate t in taxRates)
                {
                    if(annualSalary > t.MaxAmount)
                    {
                        incomeTax += (t.MaxAmount - previousTaxAmount) * t.Rate;
                        previousTaxAmount = t.MaxAmount;
                    }
                    else
                    {
                        reminingTaxableSalary = annualSalary - previousTaxAmount;
                        incomeTax += reminingTaxableSalary * t.Rate;
                        break;
                    }
                }
            }

            // If basesalary is greater than 180000 then 39% of tax will be applicable on the exceeded amount
            if(annualSalary > MAXTAXAMOUNT)
            {
                reminingTaxableSalary = annualSalary - previousTaxAmount;
                incomeTax += reminingTaxableSalary * MAXTAXRATE;
            }

            return incomeTax/MONTHSINAYEAR;
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}