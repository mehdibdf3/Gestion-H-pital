using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gestion_Hôpital;

namespace Gestion_Hôpital
{
    public class HospitalContext : DbContext
    {
        public DbSet<Medecin> Medecins { get; set; }
        public DbSet<Specialite> Specialites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=HospitalDB;Trusted_Connection=True;");
        }
    }
}
