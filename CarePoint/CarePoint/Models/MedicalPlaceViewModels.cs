using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarePoint.Models
{
    public class MedicalPlaceProfileViewModel
    {
        public long MedicalPlaceID { get; set; }

        public bool IsAdmin { get; set; }

        public ICollection<ServiceViewModel> Services { get; set; }

        public CareUnitViewModel NewCareUnit { get; set; }

        public ICollection<CareUnitViewModel> CareUnits { get; set; }

        public ICollection<ServiceCategory> ServiceCategories { get; set; }

        public ICollection<CareUnitType> CareUnitTypes { get; set; }

        public Service NewService { get; set; }
        public MedicalPlace medicalPlace { get; set; }
    }

    public class ServiceViewModel
    {
        public long ID { get; set; }

        public long ProviderID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public long CategoryID { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Cost must be greater than or equal 1")]
        public decimal Cost { get; set; }

        public ICollection<WorkSlotViewModel> WorkSlots { get; set; }

        public WorkSlotViewModel NewWorkSlot { get; set; }
    }

    public class WorkSlotViewModel
    {
        public long ServiceID { get; set; }

        public bool IsSaturday { get; set; }

        public bool IsSunday { get; set; }

        public bool IsMonday { get; set; }

        public bool IsTuesday { get; set; }

        public bool IsWednesday { get; set; }

        public bool IsThursday { get; set; }

        public bool IsFriday { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public bool IsRemoved { get; set; }
    }

    public class CareUnitViewModel
    {
        public long ID { get; set; }

        public long? ProviderID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Nullable<decimal> Cost { get; set; }

        public int AvailableRoomCount { get; set; }

        public DateTime? LastUpdate { get; set; }

        public long CareUnitTypeID { get; set; }
    }



    // Needs Revision !

    public class MedicalPlaceViewModels
    {
        public MedicalPlaceViewModel medicalPlace { get; set; }
        public List<SelectListItem> medicalPlaceTypes { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
    public class MedicalPlaceViewModel
    {
        [Required(ErrorMessage = "MedicalPlace Name is Required")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "MedicalPlace Name must contains characters only")]
        public string Name { get; set; }

        [Required(ErrorMessage = "MedicalPlace Type is Required")]
        public long TypeID { get; set; }

        [Required(ErrorMessage = "Must upload MedicalPlace Photo")]
        public HttpPostedFileWrapper Photo { get; set; }

        [Required(ErrorMessage = "Must upload MedicalPlace Permission")]
        public HttpPostedFileWrapper Permission { get; set; }


        [Required(ErrorMessage = "Must add MedicalPlace Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Must Enter MedicalPlace Phone Number")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "Phone Number must contains 11 numbers only")]
        public string Phone { get; set; }

        public long ID { get; set; }
        public string Description { get; set; }
        public long OwnerID { get; set; }
    }
    public class SearchPlaceViewModel
    {
        public string serviceType { get; set; }
        public string placeName { get; set; }
        public bool checkDistance { get; set; }
        public bool checkCost { get; set; }
        public bool checkRate { get; set; }
        public bool checkPopularity { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
    public class SlotViewModel
    {
        public string Type { get; set; }
        public double duration { get; set; }
        public string description { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Time { get; set; }
    }
    public class ServiceDayViewModel
    {
        public string day { get; set; }
        public int ID { get; set; }
    }
}