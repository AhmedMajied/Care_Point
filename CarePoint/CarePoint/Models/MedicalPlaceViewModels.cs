using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarePoint.Models
{
    public class MedicalPlaceProfileViewModel
    {
        public ICollection<ServiceViewModel> Services { get; set; }

        public Service NewService { get; set; }
    }

    public class ServiceViewModel
    {
        public long ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public long CategoryID { get; set; }

        [Required]
        public decimal Cost { get; set; }

        public ICollection<WorkSlotViewModel> WorkSlots { get; set; }

        public WorkSlot NewWorkSlot { get; set; }
    }

    public class WorkSlotViewModel
    {
        public bool IsSaturday { get; set; }

        public bool IsSunday { get; set; }

        public bool IsMonday { get; set; }

        public bool IsTuesday { get; set; }

        public bool IsWednesday { get; set; }

        public bool IsThursday { get; set; }

        public bool IsFriday { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }
    }
}