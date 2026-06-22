using F1WebAPI.Context;
using F1WebAPI.DTOs;
using F1WebAPI.Models;
using F1WebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using F1WebAPI.ViewModels;

namespace F1WebAPI.Services
{
    public class DriverService
    {
        private readonly IRepositoryBase<Driver> _repo;
        private readonly IRepositoryBase<Team> _repoTeam;

        public DriverService(IRepositoryBase<Driver> repo, IRepositoryBase<Team> team)
        {
            this._repo = repo;
            this._repoTeam   = team;
        }


        public async Task<ResponseDTO> GetDrivers(FilterPageVM filter)
        {
            var drivers =await _repo.GetAll();
            var teams =  await _repoTeam.GetAll();

            var result = (from a in drivers
                           join b in teams
                           on a.TeamID equals b.Id
                           select new DriverVM()
                           {
                               Championships = a.Championships,
                               CurrentPoints = a.CurrentPoints,
                               Id = a.Id,
                               Name=a.Name,
                               Poles = a.Poles,
                               RaceWins = a.RaceWins,
                               TeamName = b.Name,
                               TeamId = a.TeamID
                           }
                    ).AsQueryable<DriverVM>();

           

            if (!String.IsNullOrEmpty(filter.SearchStr)) result = result.Where(filter.SearchStr);

            

            var driverPageResult = result.Skip((filter.ItemsPerPage * (filter.PageNo - 1))).Take(filter.ItemsPerPage).AsQueryable();

            if (!String.IsNullOrEmpty(filter.SortStr)) driverPageResult = driverPageResult.OrderBy(filter.SortStr);

            return new ResponseDTO(result.Count(), driverPageResult);

        }

        public async Task<ResponseDTO> GetAllDrivers()
        {
            var drivers = await _repo.GetAll();
            var teams = await _repoTeam.GetAll();

            var result = (from a in drivers
                          join b in teams
                          on a.TeamID equals b.Id
                          select new DriverVM()
                          {
                              Championships = a.Championships,
                              CurrentPoints = a.CurrentPoints,
                              Id = a.Id,
                              Name = a.Name,
                              Poles = a.Poles,
                              RaceWins = a.RaceWins,
                              TeamName = b.Name,
                              TeamId = a.TeamID
                          }
                    ).AsQueryable<DriverVM>();

            return new ResponseDTO(result.Count(), result);

        }

        public async Task<Driver> SaveDriver(Driver driver)
        {
            if (!this.Validate(driver))
            {
                //check the model is valid else throw an excception
                //throw exception                
                throw new ValidationException("Invalid Driver");
            }

            return  await _repo.Add(driver);
        }

        public async Task<ResponseDTO> GetDriver(long id)
        {
            var d =await  _repo.GetById(id);
            if (d == null) return new ResponseDTO(0, null);
            var t = await _repoTeam.GetById(d.TeamID);

           
            if (d!= null)
            {
                var driver = new DriverVM();

                driver.Id = d.Id;
                driver.Name = d.Name;
                driver.Poles = d.Poles;
                driver.RaceWins = d.RaceWins;
                driver.TeamName = t.Name;
                driver.TeamId = d.TeamID;
                driver.Championships = d.Championships;
                driver.CurrentPoints = d.CurrentPoints;

                return new ResponseDTO(1, driver);
            }
            else
            {
                return new ResponseDTO(0, null);
            }

        }

        public async Task<Driver> DeleteDriver(long id)
        {

            return await  _repo.Delete(id);
        }
        public async Task<ResponseDTO> GetDriversByName(string name, int page, int itemsPerPage, string sortString)
        {
            var drivers = await _repo.GetWhere(x => x.Name.Contains(name));
            var teams = await _repoTeam.GetAll();

            var result = (from a in drivers
                          join b in teams
                          on a.TeamID equals b.Id
                          select new DriverVM()
                          {
                              Championships = a.Championships,
                              CurrentPoints = a.CurrentPoints,
                              Id = a.Id,
                              Name = a.Name,
                              Poles = a.Poles,
                              RaceWins = a.RaceWins,
                              TeamName = b.Name,
                              TeamId = a.TeamID

                          }
            ).AsQueryable<DriverVM>();


            var pageResult = result.Skip((itemsPerPage * (page - 1))).Take(itemsPerPage).AsQueryable();

            if (!String.IsNullOrEmpty(sortString)) pageResult = pageResult.OrderBy(sortString);

            return new ResponseDTO(result.Count(), pageResult);

        }

        public async Task<Driver> UpdateDriver(Driver driver)
        {
            //should call validate again here

            //use not tracking as it will error when try to update beuase entity tracking will be turned on
            var d = await  _repo.GetWhereNoTracking(x => x.Id == driver.Id);

            //now check all fields to see which have chnaged and only update those that have.
            driver.Poles = (d.FirstOrDefault().Poles != driver.Poles) ? driver.Poles : d.FirstOrDefault().Poles;

            return  _repo.Update(driver).Result;
        }

        private bool Validate(Driver driver)
        {
            //validate here
            if (driver.Name=="") return false;


            return true;
        }




    }
}
