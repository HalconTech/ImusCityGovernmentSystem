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
    
    public partial class PayeeRepresentative
    {
        public int PayeeRepID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> PayeeID { get; set; }
        public string MobileNo { get; set; }
        public string Landline { get; set; }
    
        public virtual Payee Payee { get; set; }
    }
}
