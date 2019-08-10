using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImusCityGovernmentSystem.Model
{
    public class FundModel
    {
        public int id { get; set; }

        public string FundBankName { get; set; }

        public string FundName { get; set; }

        public string BankName { get; set; }

        public string Prefix { get; set; }

        public string AccountNumber { get; set; }

        public decimal CurrentBalance { get; set; }


    }
}
