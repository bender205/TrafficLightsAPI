using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TrafficLights.Model.Entities;

namespace TrafficLights.Data.DataAccess
{
    public class TraficLightsContext : DbContext
    {
        public TraficLightsContext(DbContextOptions<TraficLightsContext> options) : base(options)
        {
        }
        public DbSet<TrafficLightEntity> Lights { get; set; }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrafficLightEntity>().ToTable("TraficLights");
        }
    }
}
