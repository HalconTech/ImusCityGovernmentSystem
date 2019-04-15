using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImusCityGovernmentSystem.CheckDisbursement.CheckReleasing.Model
{
    public class CheckReleasedListingModel
    {
        public string CheckNumber { get; set; }
        public string VoucherNumber { get; set; }
        public string Name { get; set; }
        public string DateReleased { get; set; }
        public string BankName { get; set; }
        public int ReleasedId { get; set; }
    }
}
