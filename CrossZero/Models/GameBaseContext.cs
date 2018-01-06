using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
namespace CrossZero.Models
{
    public class GameBaseContext : DbContext

    {
        public GameBaseContext()
            :base(//"u0384178_SellexDB")
                  //"BoxModelDB")      
                 "GameBaseDB")
        { }
        public DbSet<Game> Games { get; set; }
        public DbSet<Hod> Hods { get; set; }
    }
}