using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;
using DAL;
using BLL;

namespace Extensions
{
    //
    // Summary:
    //     Extensions making it easier to get the user name/user id claims off of an identity
    public static class IdentityExtensions
    {
      
        public static Citizen GetCitizen(this IIdentity identity)
        {
            long id = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId<long>(identity);
            CitizenBusinessLayer citizenBL = new CitizenBusinessLayer();
            return citizenBL.GetCitizen(id);
        }
    }
}