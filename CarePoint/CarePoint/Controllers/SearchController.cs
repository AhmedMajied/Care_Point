using BLL;
using CarePoint.Models;
using DAL;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
namespace CarePoint.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private SearchBusinessLayer searchBusinessLayer;
        public SearchController()
        {
            searchBusinessLayer = new SearchBusinessLayer();
        }
        public JsonResult SearchAccount(string key,string value)
        {
            Debug.WriteLine(key + "  ---  " + value);
            AccountResultViewModel accounts = new AccountResultViewModel();
            ICollection<Citizen> result = searchBusinessLayer.searchAccounts(key,value);
            foreach (Citizen citizen in result)
            {
                if (!(citizen is DAL.Specialist))
                {
                    accounts.nonSpecialists.Add(citizen);
                }
                else if (citizen is DAL.Specialist && citizen.Roles.Equals("Doctor"))
                {
                    accounts.doctors.Add(citizen);
                }
                else if (citizen is DAL.Specialist && citizen.Roles.Equals("Pharmacist"))
                {
                    accounts.pharmacists.Add(citizen);
                }
            }
            /*Debug.WriteLine("Citizens");
            foreach (Citizen c in accounts.nonSpecialists)
                Debug.WriteLine(c.Name);
            Debug.WriteLine("Doctors");
            foreach (Citizen c in accounts.doctors)
                Debug.WriteLine(c.Name);
            Debug.WriteLine("Pharmacists");
            foreach (Citizen c in accounts.pharmacists)
                Debug.WriteLine(c.Name);
            Debug.WriteLine("-----");*/
            return Json(accounts, JsonRequestBehavior.AllowGet);
        }
    }
}