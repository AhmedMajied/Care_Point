using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace BLL
{
    public class SearchBusinessLayer
    {
        public CarePointEntities dbEntities;
        public SearchBusinessLayer()
        {
            dbEntities = new CarePointEntities();
        }
        public List<List<Citizen>> searchAccounts(string searchBy, string searchValue)
        {
            List<Citizen> result = new List<Citizen>();
            List<Citizen> doctors = new List<Citizen>();
            List<Citizen> pharmacists = new List<Citizen>();
            List<Citizen> non_specialists = new List<Citizen>();
            String[] split = searchValue.Split(' ');
            if (searchBy.Equals("Name"))
            {
                foreach (string val in split)
                {
                    if (dbEntities.Citizens.Any(citizen => citizen.Name.Contains(val)))
                    {
                        result =result.Union(dbEntities.Citizens.Where(citizen => citizen.Name.Contains(val)).ToList()).ToList();
                    }
                }
                if (dbEntities.Citizens.Any(citizen => citizen.Name.Contains(searchValue)))
                {
                    result = result.Union(dbEntities.Citizens.Where(citizen => citizen.Name.Contains(searchValue)).ToList()).ToList();
                }
            }
            else if (searchBy.Equals("E-mail") && (dbEntities.Citizens.Any(citizen => citizen.Email.Contains(searchValue))))
            {
                result = dbEntities.Citizens.Where(citizen => citizen.Email.Contains(searchValue)).ToList();
            }
            else if (searchBy.Equals("Phone") && (dbEntities.Citizens.Any(citizen => citizen.PhoneNumber == searchValue)))
            {
                result = dbEntities.Citizens.Where(citizen => citizen.PhoneNumber == searchValue).ToList();
            }
            foreach (Citizen specialist in result)
            {
                if (specialist is Specialist)
                {
                    if (((Specialist)specialist).SpecialityID == 1)
                    {
                        doctors.Add(specialist);
                    }
                    else if (((Specialist)specialist).SpecialityID == 2)
                    {
                        pharmacists.Add(specialist);
                    }
                }
                else
                {
                    non_specialists.Add(specialist);
                   
                }
            }
            List<List<Citizen>> allCitizens = new List<List<Citizen>>();
            allCitizens.Add(non_specialists);//0
            allCitizens.Add(doctors);//1
            allCitizens.Add(pharmacists);//2
            return allCitizens;
        }
        public List<Citizen>getPatientList(long doctorId)
        {
            List<Citizen> patientList = new List<Citizen>();
            patientList = dbEntities.Attachments.Where(patient => patient.SpecialistID == doctorId).Select(p=>p.Citizen).ToList();
            return patientList;
        }

    }
}


