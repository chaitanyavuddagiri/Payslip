Framework
-------------

.Net 6 

Building App
----------------

dotnet build

Run Application
---------------------

dotnet run --project .\Payslip.Web through command prompt .net SDK need to be installed

or

double click on payslip.sln after cloning or downloading the repository file this requeire visual studio IDE

Design
--------
This app has only one view with multiple buttons to add employees and generate payslips for the added employees

Models
---------
1. Employee - represents an employee with his/her salary details
2. EmployeePayslip - represents an employee payslip with tax and net income details
3. TaxRate - defins a tax structure with min and max amounts with its rate
4. PayslipViewModel - which represents our view Employee to create and employee, Employees list to display the list of employees, and EmployeePayslips to dispaly the list of calculated amouts of the given employees

Controller
-------------
1. Index action - to render the basic view
2. Index post action - using the switch case to handle the different button actions

Assumptions 
-----------------
1. Assumed all values of emplyee model as requeired fields
2. First Name and Last Name properties will not accept any specials characters
3. Annual Salary and Super fields will not accept and negitive decimal
4. Represented months as a dropdown with the help of Enum

Constants
-------------
1. Current Year - used to displaying the pay period assuming it as current year
2. Max Tax Amount - maximum threshold of the taxable amount 
3. Max Tax Rate - 39% of tax is applicable for the remining taxable amount after Max Tax Amount
4. Months in a Year - 12 make use of constant since it used in multiple places for claculating tax and monthly gross amount based on annual salary
5. Tax Rates - is an enumerable colletion of tax rates used in calculating the tax amount based on annual salary of an employee
6. emplyees - to maintain list of added employees across multiple postbacks and updating the latest view

Index Post Action has following functionality
------------------------------------------------------

1. Identifying the click action of different buttons

	a. Add Employee - this wll add the  employee to the static list of employee and updates the viewmodel of employees list
  
	b. Generate Payslips - this will loop through each and every employee from employees list and generates and required outupt based on given employee annual salary and super rates
		
    - GenerateEmployeePayslip - it loops through all the added employees and calculates its taxes and update the view model with the output
		
    - GetEmployeePayslip - deails with a single employee it calculates monthly tax, net imcome, gross income along with super and pay period
		
    - GetPayPeriod - it generates with string representing the payslip pay period in the following formated based on selected month '01 March - 31 March'
		
    - CalculateIncomeTax - Calculate montly income tax based on the Tax Rates and employee annual salary and returns a decimal which represents monthly tax amount


No test cases added currently
