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
    
    public partial class AuditTrail
    {
        public int AuditTrailID { get; set; }
        public Nullable<System.DateTime> LogDate { get; set; }
        public string Activity { get; set; }
        public string IPAddress { get; set; }
        public string ComputerName { get; set; }
        public Nullable<int> UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ModuleName { get; set; }
    }
}
