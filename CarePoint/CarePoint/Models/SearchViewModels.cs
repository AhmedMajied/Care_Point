using DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarePoint.Models
{
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
    public class PatientViewModel {
        public int femaleCount { get; set; }
        public List<UserAccount> female { get; set; }
        public int maleCount { get; set; }
        public List<UserAccount> male { get; set; }
    }

}