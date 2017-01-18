using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.IO;
using System.Linq;
using DAL;

namespace Inspinia_MVC5.Migrations
{

    

    internal sealed class Configuration : DbMigrationsConfiguration<AdvContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }



        protected override void Seed(AdvContext context)
        {
        }
    }
}
