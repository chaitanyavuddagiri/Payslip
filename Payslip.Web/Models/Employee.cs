using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Payslip.Web.Models
{
    public class Employee
    {
        [Required]
        [RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "No special characters allowed")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "No special characters allowed")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage ="Please enter a valid value")]
        [DisplayName("Annual Salary")]
        public decimal AnnualSalary { get; set; }
        [Required]
        [Range(0,99.99)]
        public decimal Super { get; set; }
        [Required]
        [DisplayName("Pay Period")]
        public Months PayPeriod { get; set; }
    }

    public enum Months
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
}
