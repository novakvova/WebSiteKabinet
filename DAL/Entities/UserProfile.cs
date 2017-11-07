using DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class UserProfile
    {
        [Key,ForeignKey("AppUserOf")]
        [StringLength(maximumLength:128)]
        public string Id { get; set; }
        public virtual AppUser AppUserOf { get; set; }
        [StringLength(maximumLength:256)]
        public string FirstName { get; set; }
        [StringLength(maximumLength: 256)]
        public string LastName { get; set; }
        [StringLength(maximumLength: 256)]
        public string Image { get; set; }
    }
}
