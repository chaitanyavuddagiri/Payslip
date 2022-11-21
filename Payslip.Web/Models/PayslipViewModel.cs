namespace Payslip.Web.Models
{
    public class PayslipViewModel
    {
        public Employee Employee { get; set; }
        public List<Employee>? Employees { get; set; }
        public List<EmployeePayslip>? EmployeePayslips { get; set; }
    }
}
