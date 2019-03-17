using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImusCityGovernmentSystem.Model
{
    public class DisbursementVoucherModel
    {
        public string VoucherNo { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string TIN_EmpNo { get; set; }
        public string ObligationRequestNo { get; set; }
        public string CompanyAddress { get; set; }
        public string Unit_Project { get; set; }
        public string DepartmentCode { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Obligated { get; set; }
        public string DocumentCompleted { get; set; }
        public string Certification { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string PaymentName { get; set; }
        public string Signatory { get; set; }
        public string Signatory2 { get; set; }
        public string Signatory3 { get; set; }
    }
}
