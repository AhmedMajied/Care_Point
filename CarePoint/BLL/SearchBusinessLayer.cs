using DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class SearchBusinessLayer
    {
        public CarePointEntities dbEntities;
        public SearchBusinessLayer()
        {
            dbEntities = new CarePointEntities();
        }
        public ICollection<Citizen> searchAccounts(string searchBy, string searchValue)
        {
            ICollection<Citizen> result = new List<Citizen>();
            String[] split = searchValue.Split(' ');
            if (searchBy.Equals("Name"))
            {
                foreach (string val in split)
                {
                    if (dbEntities.Citizens.Any(citizen => citizen.Name.Contains(val)))
                    {
                        result.Union(dbEntities.Citizens.Where(citizen => citizen.Name.Contains(searchValue)));
                    }
                }
                if (dbEntities.Citizens.Any(citizen => citizen.Name.Contains(searchValue)))
                {
                    result.Union(dbEntities.Citizens.Where(citizen => citizen.Name.Contains(searchValue)));
                }
            }
            else if (searchBy.Equals("E-mail") && (dbEntities.Citizens.Any(citizen => citizen.Email == searchValue)))
            {
                result = dbEntities.Citizens.Where(citizen => citizen.Email.Contains(searchValue)).ToList();
            }
            else if (searchBy.Equals("Phone") && (dbEntities.Citizens.Any(citizen => citizen.PhoneNumber == searchValue)))
            {
                result = dbEntities.Citizens.Where(citizen => citizen.PhoneNumber == searchValue).ToList();
            }
            return result;
        }

    }
}


