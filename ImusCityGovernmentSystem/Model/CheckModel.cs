using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImusCityGovernmentSystem.Model
{
    public class CheckModel
    {
        public string CheckNo { get; set; }
        public DateTime CheckDate { get; set; }
        public string CompanyName { get; set; }
        public string AmountInWords { get; set; }
        public string CheckDescription { get; set; }
        public string Signatory1 { get; set; }
        public string Signatory2 { get; set; }
        public string VoucherNo { get; set; }
        public decimal Amount { get; set; }
    }
}
