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
    
    public partial class Disbursement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Disbursement()
        {
            this.Checks = new HashSet<Check>();
        }
    
        public int DisbursementID { get; set; }
        public Nullable<int> PayeeID { get; set; }
        public Nullable<int> PaymentTypeID { get; set; }
        public string VoucherNo { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<bool> Obligated { get; set; }
        public Nullable<bool> DocCompleted { get; set; }
        public string Certification { get; set; }
        public byte[] DateStamp { get; set; }
        public Nullable<int> PayeeRepID { get; set; }
        public string ObligationRequestNo { get; set; }
        public string PayeeName { get; set; }
        public Nullable<int> AdviceNo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Check> Checks { get; set; }
        public virtual Department Department { get; set; }
        public virtual Payee Payee { get; set; }
        public virtual PayeeRepresentative PayeeRepresentative { get; set; }
    }
}
