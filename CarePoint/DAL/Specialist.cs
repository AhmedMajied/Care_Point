//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Specialist : Citizen
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Specialist()
        {
            this.AttachmentsAdded = new HashSet<Attachment>();
            this.CareRequests = new HashSet<CareRequest>();
            this.CareUnitMembershipRequests = new HashSet<CareUnitMembershipRequest>();
            this.HistoryRecordsAdded = new HashSet<HistoryRecord>();
            this.OwnedMedicalPlaces = new HashSet<MedicalPlace>();
            this.Pharmacies = new HashSet<Pharmacy>();
            this.PharmacyMembershipRequests = new HashSet<PharmacyMembershipRequest>();
            this.ReceiptsAdded = new HashSet<Receipt>();
            this.ServiceMembershipRequests = new HashSet<ServiceMembershipRequest>();
            this.AdministratedMedicalPlaces = new HashSet<MedicalPlace>();
        }
    
        public byte[] ProfessionLicense { get; set; }
        public long SpecialityID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attachment> AttachmentsAdded { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CareRequest> CareRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CareUnitMembershipRequest> CareUnitMembershipRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoryRecord> HistoryRecordsAdded { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicalPlace> OwnedMedicalPlaces { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pharmacy> Pharmacies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PharmacyMembershipRequest> PharmacyMembershipRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Receipt> ReceiptsAdded { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceMembershipRequest> ServiceMembershipRequests { get; set; }
        public virtual Speciality Speciality { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicalPlace> AdministratedMedicalPlaces { get; set; }
    }
}