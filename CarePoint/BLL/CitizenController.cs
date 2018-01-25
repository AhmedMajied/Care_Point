using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CitizenController
    {
        private CarePointEntities _dbContext;

        public CitizenController()
        {
            _dbContext = new CarePointEntities();
        }
        public Citizen GetCitizen(long Id)
        {
            return _dbContext.Citizens.FirstOrDefault(c => c.Id == Id);
        }
    }
}
