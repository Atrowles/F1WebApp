using F1WebAPI.DTOs;
using F1WebAPI.Models;
using F1WebAPI.Repositories;
using F1WebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace F1WebAPI.Services
{
    public class DriverResultService
    {
        private readonly IRepositoryBase<DriverResult> _repo;
        private readonly IRepositoryBase<Driver> _repoDriver;
        private readonly IRepositoryBase<RaceResult> _repoResult;


        public DriverResultService(IRepositoryBase<DriverResult> repo, IRepositoryBase<Driver> repoDriver, IRepositoryBase<RaceResult> repoResult)
        {
            this._repo = repo;
            this._repoDriver = repoDriver;
            this._repoResult = repoResult;
        }

        public async Task<ResponseDTO> GetDriverResults(FilterPageVM filter)
        {
            var race =  await _repo.GetAll();
            var driver = await _repoDriver.GetAll();
            var raceResults = await _repoResult.GetAll();

            var results = (from a in race
                           join b in driver on a.DriverId equals b.Id
                           join c in raceResults on a.ResultId equals c.Id
                           select new DriverResultVM()
                           {
                               Id = a.Id,
                               DriverName = (b != null) ? b.Name : "",
                               Position = a.Position,
                               RaceResultId = (c != null) ? c.Id : 0,
                               Gap = a.Gap
                           }
                        ).AsQueryable<DriverResultVM>();

            if (!String.IsNullOrEmpty(filter.SearchStr)) results = results.Where(filter.SearchStr);

            var pageResult = results.Skip((filter.ItemsPerPage * (filter.PageNo - 1))).Take(filter.ItemsPerPage).AsQueryable();
            
            if (!String.IsNullOrEmpty(filter.SortStr)) pageResult = pageResult.OrderBy(filter.SortStr);

            return new ResponseDTO(results.Count(), pageResult);

        }

        public async Task<ResponseDTO> GetAllDriverResuts()
        {
            var race =  await _repo.GetAll();
            var driver =  await _repoDriver.GetAll();
            var raceResults =  await _repoResult.GetAll();

            var results = (from a in race
                           join b in driver on a.DriverId equals b.Id
                           join c in raceResults on a.ResultId equals c.Id
                           select new DriverResultVM()
                           {
                               Id = a.Id,
                               DriverName = (b != null) ? b.Name : "",
                               Position = a.Position,
                               RaceResultId = (c != null) ? c.Id : 0,
                               Gap = a.Gap
                           }
                        ).AsQueryable<DriverResultVM>();

            return new ResponseDTO(results.Count(), results);

        }

        public async Task<List<DriverResult>> SaveRaceResult(List<DriverResult> races)
        {
            //if (!this.Validate(race))
            //{
            //check the model is valid else throw an excception
            //throw exception                
            //  throw new ValidationException("Invalid Result");
            //}

    
            await _repo.AddEntities(races);
            

            return races;
            
        }

    

        public async Task<DriverResult> DeleteDriverResult(long id)
        {

            return  await _repo.Delete(id);
        }

        public async Task<DriverResult> UpdateDriverResult(DriverResult race)
        {
            //use not tracking as it will error when try to update beuase entity tracking will be turned on
            var r =  await _repo.GetWhereNoTracking(x => x.Id == race.Id);

            //now check all fields to see which have chnaged and only update those that have.
            race.Gap = (r.FirstOrDefault().Gap != race.Gap && race.Gap!=0) ? race.Gap : r.FirstOrDefault().Gap;
            return  _repo.Update(race).Result;
        }

        public async Task<ResponseDTO> GetDriverResultsByRace(long raceNo)
        {
            var race = await _repo.GetWhere(x=>x.ResultId == raceNo);
            var driver =  await _repoDriver.GetAll();
            var raceResults = await _repoResult.GetAll();

            var results = (from a in race
                           join b in driver on a.DriverId equals b.Id
                           join c in raceResults on a.ResultId equals c.Id
                           select new DriverResultVM()
                           {
                               Id = a.Id,
                               DriverName = (b != null) ? b.Name : "",
                               Position = a.Position,
                               RaceResultId = (c != null) ? c.Id : 0,
                               Gap = a.Gap
                           }
                           ).AsQueryable<DriverResultVM>().OrderBy("Position Asc"); 

            return new ResponseDTO(results.Count(), results);

        }

        private bool Validate(DriverResult race)
        {
            //validate here
            //if (!race.PoleTime) return false;


            return true;
        }



    }
}
