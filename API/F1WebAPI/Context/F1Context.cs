using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using F1WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace F1WebAPI.Context
{
    public class F1Context:DbContext, IDataContext
    {
        public F1Context(DbContextOptions<F1Context> options) : base(options) 
        { 
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {           
            return base.SaveChangesAsync();
        }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void SetDetached(object entity)
        {
            Entry(entity).State = EntityState.Detached;
        }

        public override EntityEntry Update(object entity)
        {
           return base.Update(entity);
        }

        public int ExecuteSqlCommand(string command, object parameters)
        {
            return Database.ExecuteSqlRaw(command, parameters);
        }

        public DbContext Instance => this;

        public DbSet<Track> Tracks { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Driver> Drivers { get; set; }

        public DbSet<RaceResult> RaceResults { get; set; }
        public DbSet<DriverResult> DriverResults { get; set; }


    }
}
