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
    
    public partial class FundBank
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FundBank()
        {
            this.BankTrails = new HashSet<BankTrail>();
        }
    
        public int FundBankID { get; set; }
        public Nullable<int> FundID { get; set; }
        public Nullable<int> BankID { get; set; }
        public string AccountNumber { get; set; }
        public Nullable<decimal> CurrentBalance { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<int> AdviceNo { get; set; }
        public Nullable<decimal> StartingBalance { get; set; }
        public Nullable<bool> IsProcessed { get; set; }
    
        public virtual Bank Bank { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BankTrail> BankTrails { get; set; }
        public virtual Fund Fund { get; set; }
    }
}