using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Document_Management_System.Models
{
    public class Employee
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePin { get; set; }
        public DateTime EmployeeDOB { get; set; }
    }
}