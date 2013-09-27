using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using FSIX.Web.Models;

namespace FSIX.Web.Controllers
{
    [Authorize]
    public class ADController : ApiController
    {
        private readonly PrincipalContext _context;
        public ADController()
        {
            _context = new PrincipalContext(ContextType.Domain);
        }

        // GET api/myadinfo
        public ADUser GetMyInfo()
        {
            UserPrincipal user = UserPrincipal.FindByIdentity(_context, User.Identity.Name);
            return new ADUser
            {
                Username = user.SamAccountName,
                DisplayName = String.Format("{0} {1}", user.GivenName, user.Surname),
                Email = user.EmailAddress
            };
        }

        //public ICollection<ADUser> GetADUsers(string Term)
        //{
        //    ICollection<ADUser> users = new DirectorySearcher(Term).FindAll();
        //    return users;
        //}
    }
}
