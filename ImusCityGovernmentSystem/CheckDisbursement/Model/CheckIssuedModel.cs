using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImusCityGovernmentSystem.CheckDisbursement.Model
{
    public class CheckIssuedModel
    {
        public string AccoutNumber { get; set; }
        public string BankName { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string CheckNo { get; set; }
        public string CompanyName { get; set; }
        public decimal Amount { get; set; }
        public string DisbursingOffice { get; set; }
    }
}
