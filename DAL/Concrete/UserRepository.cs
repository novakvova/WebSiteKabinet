using DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Concrete
{
    public class UserRepository : SqlRepository, IUserRepository
    {
        public UserRepository(IAppDBContext context)
            :base(context)
        {

        }
        public UserProfile Add(UserProfile userProfile)
        {
            this.Insert(userProfile);
            this.SaveChanges();
            return userProfile;
        }
        
    }
}
