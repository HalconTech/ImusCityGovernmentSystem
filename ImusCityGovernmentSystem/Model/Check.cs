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
    using System.Collections.Generic;
    
    public partial class Check
    {
        public int CheckID { get; set; }
        public Nullable<int> DisbursementID { get; set; }
        public string CheckNo { get; set; }
        public Nullable<int> FundID { get; set; }
        public string CheckDescription { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public byte[] DateStamp { get; set; }
        public Nullable<decimal> Amount { get; set; }
    
        public virtual Disbursement Disbursement { get; set; }
    }
}
