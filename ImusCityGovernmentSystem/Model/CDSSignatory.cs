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
    
    public partial class CDSSignatory
    {
        public int CDSSignatoryID { get; set; }
        public Nullable<int> CityMayor { get; set; }
        public Nullable<int> CityTreasurer { get; set; }
        public Nullable<int> CityAccountant { get; set; }
        public Nullable<int> AccountantRepresentative { get; set; }
        public Nullable<int> CityAdministrator { get; set; }
        public Nullable<int> DisbursingOfficer { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Employee Employee1 { get; set; }
        public virtual Employee Employee2 { get; set; }
        public virtual Employee Employee3 { get; set; }
        public virtual Employee Employee4 { get; set; }
        public virtual Employee Employee5 { get; set; }
    }
}
