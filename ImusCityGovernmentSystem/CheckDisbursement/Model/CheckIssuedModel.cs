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
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string CheckNo { get; set; }
        public string CompanyName { get; set; }
        public decimal Amount { get; set; }
        public string DisbursingOfficer { get; set; }
        public string VoucherNo { get; set; }
        public string ReportNumber { get; set; }
        public string PaymentNature { get; set; }
        public string Center { get; set; }
    }
}
