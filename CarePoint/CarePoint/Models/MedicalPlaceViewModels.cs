using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using DAL;
namespace CarePoint.Models
{
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
}