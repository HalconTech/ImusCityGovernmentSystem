﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ImusCityHallEntities : DbContext
    {
        public ImusCityHallEntities()
            : base("name=ImusCityHallEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AuditTrail> AuditTrails { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<BankTrail> BankTrails { get; set; }
        public virtual DbSet<Check> Checks { get; set; }
        public virtual DbSet<CheckRelease> CheckReleases { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Disbursement> Disbursements { get; set; }
        public virtual DbSet<Division> Divisions { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeePosition> EmployeePositions { get; set; }
        public virtual DbSet<EmployeeRank> EmployeeRanks { get; set; }
        public virtual DbSet<EmployeeStatu> EmployeeStatus { get; set; }
        public virtual DbSet<Fund> Funds { get; set; }
        public virtual DbSet<FundBank> FundBanks { get; set; }
        public virtual DbSet<LicensingCode> LicensingCodes { get; set; }
        public virtual DbSet<Payee> Payees { get; set; }
        public virtual DbSet<PayeeRepresentative> PayeeRepresentatives { get; set; }
        public virtual DbSet<ReportParameter> ReportParameters { get; set; }
        public virtual DbSet<SecurityQuestionBank> SecurityQuestionBanks { get; set; }
        public virtual DbSet<SecurityQuestionUser> SecurityQuestionUsers { get; set; }
        public virtual DbSet<SubModule> SubModules { get; set; }
        public virtual DbSet<SubModuleUser> SubModuleUsers { get; set; }
        public virtual DbSet<UserAccount> UserAccounts { get; set; }
        public virtual DbSet<PayeeRepresentativeView> PayeeRepresentativeViews { get; set; }
    
        public virtual ObjectResult<GetCheckExpiryNotice_Result> GetCheckExpiryNotice()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetCheckExpiryNotice_Result>("GetCheckExpiryNotice");
        }
    
        public virtual ObjectResult<GetCheckRegister_Result> GetCheckRegister(Nullable<System.DateTime> date, Nullable<int> fundID)
        {
            var dateParameter = date.HasValue ?
                new ObjectParameter("Date", date) :
                new ObjectParameter("Date", typeof(System.DateTime));
    
            var fundIDParameter = fundID.HasValue ?
                new ObjectParameter("FundID", fundID) :
                new ObjectParameter("FundID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetCheckRegister_Result>("GetCheckRegister", dateParameter, fundIDParameter);
        }
    
        public virtual ObjectResult<GetDisbursementVoucher_Result> GetDisbursementVoucher(Nullable<int> disbursementID)
        {
            var disbursementIDParameter = disbursementID.HasValue ?
                new ObjectParameter("DisbursementID", disbursementID) :
                new ObjectParameter("DisbursementID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDisbursementVoucher_Result>("GetDisbursementVoucher", disbursementIDParameter);
        }
    }
}
