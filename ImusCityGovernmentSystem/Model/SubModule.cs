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
    
    public partial class SubModule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubModule()
        {
            this.LicensingCodes = new HashSet<LicensingCode>();
            this.SubModuleUsers = new HashSet<SubModuleUser>();
        }
    
        public int SubModuleID { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public Nullable<int> ModuleID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LicensingCode> LicensingCodes { get; set; }
        public virtual Module Module { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubModuleUser> SubModuleUsers { get; set; }
    }
}
