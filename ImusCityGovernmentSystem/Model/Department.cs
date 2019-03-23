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
    
    public partial class Department
    {
        public Department()
        {
            this.Employees = new HashSet<Employee>();
            this.Disbursements = new HashSet<Disbursement>();
        }
    
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public Nullable<int> DivisionID { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual Division Division { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Disbursement> Disbursements { get; set; }
    }
}
