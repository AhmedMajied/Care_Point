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
        public bool IsAdmin { get; set; }

        public ICollection<ServiceViewModel> Services { get; set; }

        public ICollection<ServiceCategory> ServiceCategories { get; set; }

        public Service NewService { get; set; }
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
}