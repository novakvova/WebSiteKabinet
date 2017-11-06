using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    internal class DBInitializer : CreateDatabaseIfNotExists<AppDBContext>
    {
        protected override void Seed(AppDBContext context)
        {
            base.Seed(context);
        }
    }
}
