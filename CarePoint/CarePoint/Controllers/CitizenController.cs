using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarePoint.Models;

namespace CarePoint.Controllers
{
    public class CitizenController : Controller
    {
        private CitizenBusinessLayer _citizenBusinessLayer;

        public CitizenController()
        {
            _citizenBusinessLayer = new CitizenBusinessLayer();
        }


        public ActionResult CurrentPatient(long citizenID)
        {
            Citizen citizen = _citizenBusinessLayer.GetCitizen(citizenID);

            return View(citizen);
        }

        public List<UserAccount> getUsers(List<Citizen> allCitizens)
        {
            List<UserAccount> users = new List<UserAccount>();
            foreach (Citizen c in allCitizens)
            {
                users.Add(new UserAccount { Id = c.Id, Name = c.Name, Photo = c.Photo });
            }
            return users;
        }
        public JsonResult SearchAccount(string key, string value)
        {
            AccountResultViewModel accounts = new AccountResultViewModel();
            accounts.doctors = new List<UserAccount>();
            accounts.citizens = new List<UserAccount>();
            accounts.pharmacists = new List<UserAccount>();
            List<List<Citizen>> allCitizens = _citizenBusinessLayer.searchAccounts(key, value);
            accounts.citizens = getUsers(allCitizens[0]);
            accounts.doctors = getUsers(allCitizens[1]);
            accounts.pharmacists = getUsers(allCitizens[2]);
            accounts.pharmacistsCount = accounts.pharmacists.Count;
            accounts.citizensCount = accounts.citizens.Count;
            accounts.doctorsCount = accounts.doctors.Count;
            return Json(accounts);
        }
        public JsonResult PatientsList(long doctorId)
        {
            PatientViewModel patients = new PatientViewModel();
            List<Citizen> list = _citizenBusinessLayer.getPatientList(doctorId);
            List<Citizen> maleList = new List<Citizen>();
            List<Citizen> femaleList = new List<Citizen>();
            foreach (Citizen c in list)
            {
                if (c.Gender.ToLower().Equals("male"))
                    maleList.Add(c);
                else if (c.Gender.ToLower().Equals("female"))
                    femaleList.Add(c);
            }
            patients.female = new List<UserAccount>();
            patients.female = getUsers(femaleList);
            patients.male = new List<UserAccount>();
            patients.male = getUsers(maleList);
            patients.femaleCount = patients.female.Count;
            patients.maleCount = patients.male.Count;
            return Json(patients);
        }
    }
}