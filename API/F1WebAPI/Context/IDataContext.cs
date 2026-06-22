using F1WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace F1WebAPI.Context
{
    public interface IDataContext : IDisposable
    {
        DbContext Instance { get; }


        int ExecuteSqlCommand(string command, object parameters);
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        

        public DbSet<Track> Tracks { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Driver> Drivers { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        void SetModified(object entity);
        EntityEntry Update(object entity);

        void SetDetached(object entity);


    }
}
