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
        public SubModule()
        {
            this.SubModuleUsers = new HashSet<SubModuleUser>();
        }
    
        public int SubModuleID { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public Nullable<int> ModuleID { get; set; }
    
        public virtual ICollection<SubModuleUser> SubModuleUsers { get; set; }
    }
}
