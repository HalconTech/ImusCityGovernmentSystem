using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImusCityGovernmentSystem.Model
{
    public class CheckRegisterModel
    {
        public string FundName { get; set; }
        public string Branch { get; set; }
        public string AccoutNumber { get; set; }
        public string BankName { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string CheckNo { get; set; }
        public string CompanyName { get; set; }
        public decimal Amount { get; set; }
        public string AmountInWords { get; set; }
    }
}
