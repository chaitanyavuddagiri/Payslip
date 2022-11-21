using System.ComponentModel;

namespace Payslip.Web.Models
{
    public class EmployeePayslip
    {
        [DisplayName("Full Name")]
        public string FullName { get; set; }
        [DisplayName("Pay Period")]
        public string PayPeriod { get; set; }
        [DisplayName("Gross Income")]
        public decimal GrossIncome { get; set; }
        [DisplayName("Income Tax")]
        public decimal IncomeTax { get; set; }
        [DisplayName("Net Income")]
        public decimal NetIncome { get; set; }
        public decimal Super { get; set; }
    }
}
