using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPS_EmployeeDetails.Model
{
    public class ResponseObj
    {
        public string code { get; set; }

        //public string meta { get; set; }

        public EmployeeObj[] data { get; set; }
    }
}
