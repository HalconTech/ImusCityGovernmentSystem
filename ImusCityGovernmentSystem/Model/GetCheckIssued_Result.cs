//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImusCityGovernmentSystem.Model
{
    using System;
    
    public partial class GetCheckIssued_Result
    {
        public string FundName { get; set; }
        public string Branch { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string CheckNo { get; set; }
        public string CompanyName { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string PaymentNature { get; set; }
        public string FundPrefix { get; set; }
        public string VoucherNo { get; set; }
    }
}
