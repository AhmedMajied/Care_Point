﻿using BLL;
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
        public List<UserAccount> getUsers(List<Citizen>allCitizens)
        {
            List<UserAccount> users = new List<UserAccount>();
            foreach (Citizen c in allCitizens)
            {
                users.Add(new UserAccount { Id = c.Id, Name = c.Name, Photo = c.Photo });
            }
            return users;
        }
        public JsonResult SearchAccount(string key,string value)
        {
            AccountResultViewModel accounts = new AccountResultViewModel();
            accounts.doctors = new List<UserAccount>();
            accounts.citizens = new List<UserAccount>();
            accounts.pharmacists = new List<UserAccount>();
            List<List<Citizen>> allCitizens = searchBusinessLayer.searchAccounts(key, value);
            accounts.citizens = getUsers(allCitizens[0]);
            accounts.doctors = getUsers(allCitizens[1]);
            accounts.pharmacists = getUsers(allCitizens[2]);
            accounts.pharmacistsCount = accounts.pharmacists.Count;
            accounts.citizensCount = accounts.citizens.Count;
            accounts.doctorsCount=accounts.doctors.Count;
            return Json(accounts);
        }
    }
}