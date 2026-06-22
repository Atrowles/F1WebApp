using F1WebAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace F1WebAPI.Repositories
{
    public  class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        //this will be used to carry out all database actions, which will be the same just a different type
        //if the actions were differernt i.e differing code then we would make this all abstract and then overload 
        //the meothds in the TeamReposiroty etc

        internal IDataContext context;
        internal DbSet<TEntity> dbSet;

        public RepositoryBase(IDataContext context)
        {
            this.context = context;
            //this is setting the type of DbSet .ie teams
            //its basically making a shortcut to the add/remove methods so we dont habe to to context.dbset<Tentity>.add
            this.dbSet = context.Set<TEntity>();

        }


        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
                   

           return await dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetWhere(Expression <Func<TEntity,bool>> predicate)
        {

            //need to know hiow to use and ccall this
            //dbSet.Take(10).Skip(10);
            return await dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<bool> AddEntities(List<TEntity> entities)
        {

            dbSet.AddRange(entities);
            await context.SaveChangesAsync();

            return true;
        }

        public virtual async Task<IEnumerable<TEntity>> GetWhereNoTracking(Expression<Func<TEntity, bool>> predicate)
        {

            //need to know hiow to use and ccall this
            //dbSet.Take(10).Skip(10);
            return await dbSet.Where(predicate).AsNoTracking().ToListAsync();
        }


        public virtual async  Task<TEntity> Add(TEntity t)
        {
           // HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            //need to check if save is sucessfull by checking response

            this.dbSet.Add(t); //we can use the dbset here because of generics TEntity
            await this.context.SaveChangesAsync();

            return t;
        }

        public virtual async Task<TEntity> GetById(long id)
        {
            var obj = await this.dbSet.FindAsync(id);

            return obj;
        }

        public virtual async Task<TEntity> Update(TEntity t)

        {
            context.Update(t);

            //this had me confused for ages as it threw a entity cannot be tracked error
            //this was because I had not overloaded the update method of dbcontext and was
            //trying to do it a different way through attach 
            //the problem with this is the finAsync call below would turn on tracking of entities
            //and when tracking is on updates etc cannot be made. 
            //if we were to do it this way we should query the db using a .where and use asnotracking() 
            //to turn of tracking

            //var _entity = await this.dbSet.FindAsync(id);

            //_entity = t;
            // dbSet.Attach(_entity);         
            //context.SetDetached(_entity);

            //we need to set the model state (the thing we are updating) to say its modified
            //this will tell entity framework to run an update when savechanges is called

            //cos we are using the .update methdod above we do not need to set this
            //as the method will do it. We only use this when we are using attach for example

            //context.SetModified(t);
            //dbSet.Attach(_entity);


            await context.SaveChangesAsync();

            return t;


        }


        public virtual async Task<TEntity> Delete(long id)

        {
            var _entity =  await this.dbSet.FindAsync(id);

            this.dbSet.Remove(_entity);
            await context.SaveChangesAsync();

            return _entity;


        }

    }
 }
