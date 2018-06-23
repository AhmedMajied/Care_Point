﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CarePointEntities : DbContext
    {
        public CarePointEntities()
            : base("name=CarePointEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ActiveIngredient> ActiveIngredients { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<AttachmentType> AttachmentTypes { get; set; }
        public virtual DbSet<BloodType> BloodTypes { get; set; }
        public virtual DbSet<CareRequest> CareRequests { get; set; }
        public virtual DbSet<CareUnitMembershipRequest> CareUnitMembershipRequests { get; set; }
        public virtual DbSet<CareUnitRating> CareUnitRatings { get; set; }
        public virtual DbSet<CareUnit> CareUnits { get; set; }
        public virtual DbSet<CareUnitType> CareUnitTypes { get; set; }
        public virtual DbSet<Citizen> Citizens { get; set; }
        public virtual DbSet<Disease> Diseases { get; set; }
        public virtual DbSet<Dose> Doses { get; set; }
        public virtual DbSet<HistoryRecord> HistoryRecords { get; set; }
        public virtual DbSet<MedicalPlace> MedicalPlaces { get; set; }
        public virtual DbSet<MedicalPlaceType> MedicalPlaceTypes { get; set; }
        public virtual DbSet<MedicineActiveIngredient> MedicineActiveIngredients { get; set; }
        public virtual DbSet<MedicineForm> MedicineForms { get; set; }
        public virtual DbSet<Medicine> Medicines { get; set; }
        public virtual DbSet<Pharmacy> Pharmacies { get; set; }
        public virtual DbSet<PharmacyMedicine> PharmacyMedicines { get; set; }
        public virtual DbSet<PharmacyMembershipRequest> PharmacyMembershipRequests { get; set; }
        public virtual DbSet<PharmacyRating> PharmacyRatings { get; set; }
        public virtual DbSet<ReceiptMedicine> ReceiptMedicines { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<RelationType> RelationTypes { get; set; }
        public virtual DbSet<Relative> Relatives { get; set; }
        public virtual DbSet<RequestStatus> RequestStatuses { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }
        public virtual DbSet<ServiceMembershipRequest> ServiceMembershipRequests { get; set; }
        public virtual DbSet<ServiceRating> ServiceRatings { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<SOSs> SOSses { get; set; }
        public virtual DbSet<Speciality> Specialities { get; set; }
        public virtual DbSet<Symptom> Symptoms { get; set; }
        public virtual DbSet<UserClaim> UserClaims { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<WorkSlot> WorkSlots { get; set; }
        public virtual DbSet<PotentialDisease> PotentialDiseases { get; set; }
    }
}
