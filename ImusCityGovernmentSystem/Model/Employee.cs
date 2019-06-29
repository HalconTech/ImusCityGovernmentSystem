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
    
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.BankTrails = new HashSet<BankTrail>();
            this.CDSSignatories = new HashSet<CDSSignatory>();
            this.CDSSignatories1 = new HashSet<CDSSignatory>();
            this.CDSSignatories2 = new HashSet<CDSSignatory>();
            this.CDSSignatories3 = new HashSet<CDSSignatory>();
            this.CDSSignatories4 = new HashSet<CDSSignatory>();
            this.CDSSignatories5 = new HashSet<CDSSignatory>();
            this.Checks = new HashSet<Check>();
            this.CheckReleases = new HashSet<CheckRelease>();
            this.Customers = new HashSet<Customer>();
            this.SecurityQuestionUsers = new HashSet<SecurityQuestionUser>();
            this.SubModuleUsers = new HashSet<SubModuleUser>();
            this.LicensingCodes = new HashSet<LicensingCode>();
        }
    
        public int EmployeeID { get; set; }
        public string EmployeeNo { get; set; }
        public string Title { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Nickname { get; set; }
        public Nullable<int> EmployeeDivisionID { get; set; }
        public Nullable<int> EmployeeStatusID { get; set; }
        public Nullable<int> EmployeePositionID { get; set; }
        public Nullable<System.DateTime> DateHired { get; set; }
        public Nullable<System.DateTime> DatePermanency { get; set; }
        public Nullable<System.DateTime> DateEndContract { get; set; }
        public string ReasonForLeaving { get; set; }
        public string OtherReasonForLeaving { get; set; }
        public Nullable<System.DateTime> DateResigned { get; set; }
        public Nullable<System.DateTime> DateRetired { get; set; }
        public string PermanentAddress { get; set; }
        public string CurrentAddress { get; set; }
        public string ProvincialAddress { get; set; }
        public string TelephoneNo { get; set; }
        public string ProvincialTelephoneNo { get; set; }
        public string MobileNo { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string Sex { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Birthplace { get; set; }
        public string CivilStatus { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string Spouse { get; set; }
        public string SpouseOccupation { get; set; }
        public Nullable<int> Children { get; set; }
        public string Child1 { get; set; }
        public string Child2 { get; set; }
        public string Child3 { get; set; }
        public string Child4 { get; set; }
        public string Child5 { get; set; }
        public string OtherChild { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string TaxStatus { get; set; }
        public string TIN { get; set; }
        public string SSS { get; set; }
        public string PAG_IBIG { get; set; }
        public string PhilHealth { get; set; }
        public string CedulaNo { get; set; }
        public string CedulaDate { get; set; }
        public string CedulaPlace { get; set; }
        public Nullable<System.DateTime> DateEncoded { get; set; }
        public Nullable<bool> Archive { get; set; }
        public byte[] Photo { get; set; }
        public string NameSuffix { get; set; }
        public string BankAccountNo { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BankTrail> BankTrails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CDSSignatory> CDSSignatories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CDSSignatory> CDSSignatories1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CDSSignatory> CDSSignatories2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CDSSignatory> CDSSignatories3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CDSSignatory> CDSSignatories4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CDSSignatory> CDSSignatories5 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Check> Checks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckRelease> CheckReleases { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual Division Division { get; set; }
        public virtual EmployeePosition EmployeePosition { get; set; }
        public virtual EmployeeStatu EmployeeStatu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SecurityQuestionUser> SecurityQuestionUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubModuleUser> SubModuleUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LicensingCode> LicensingCodes { get; set; }
    }
}
