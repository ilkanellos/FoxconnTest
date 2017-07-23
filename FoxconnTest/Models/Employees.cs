using System;
using System.Collections.Generic;

namespace FoxconnTest.Models
{
    public partial class Employees
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
    }
}
