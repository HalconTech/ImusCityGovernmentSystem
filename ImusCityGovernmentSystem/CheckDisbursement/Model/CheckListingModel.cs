using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImusCityGovernmentSystem.CheckDisbursement.Model
{
    public class CheckListingModel
    {
        public string CheckNo { get; set; }
        public string CheckDescription { get; set; }
        public DateTime CheckDateCreated { get; set; }
        public double CheckAmount { get; set; }
        public string ControlNo { get; set; }
        public string Status { get; set; }
        public int StatusID { get; set; }
        public string CheckUser { get; set; }
        public string VoucherNumber { get; set; }
        public string ProjectName { get; set; }
        public DateTime VoucherDateCreated { get; set; }
        public string AccountNumber { get; set; }
        public int FundID { get; set; }
        public string BankName { get; set; }
        public int BankID { get; set; }
    }
}
