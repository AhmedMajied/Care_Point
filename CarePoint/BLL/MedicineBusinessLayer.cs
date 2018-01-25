﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class MedicineBusinessLayer
    {
        private CarePointEntities DBEntities;

        public MedicineBusinessLayer()
        {
            DBEntities = new CarePointEntities();
        }

        public ICollection<Medicine> getAllMedicines()
        {
            return DBEntities.Medicines.ToList();
        }
    }
}