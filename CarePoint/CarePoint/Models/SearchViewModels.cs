using DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarePoint.Models
{

    public class SearchViewModels
    {
        public SearchAccountViewModel SearchAccount { get; set; }
    }
    public class SearchAccountViewModel
    {
        [Required(ErrorMessage = "must choose this field")]
        public string searchBy { get; set; }

        [Required(ErrorMessage = "must fill this field")]
        public string searchFor { get; set; }

    }
    public class UserAccount
    {
        public string Name { get; set; }
        public long Id { get; set; }
        public byte[] Photo { get; set; }
    }
    public class AccountResultViewModel
    {
        public int doctorsCount { get; set; }
        public List<UserAccount> doctors { get; set; }

        public int citizensCount { get; set; }
        public List<UserAccount> citizens { get; set; }

        public int pharmacistsCount { get; set; }
        public List<UserAccount> pharmacists { get; set; }

    }
}