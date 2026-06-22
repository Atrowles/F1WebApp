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
using Microsoft.AspNetCore.Authorization;

namespace F1WebAPI.Services
{
    public class RaceResultService
    {
        private readonly IRepositoryBase<RaceResult> _repo;
        private readonly IRepositoryBase<Driver> _repoDriver;
        private readonly IRepositoryBase<Track> _repoTrack;
        private readonly IRepositoryBase<DriverResult> _repoDriverResult;

        public RaceResultService(IRepositoryBase<RaceResult> repo, IRepositoryBase<Driver> repoDriver, IRepositoryBase<Track> repoTrack, IRepositoryBase<DriverResult> repoDriverResult)
        {
            this._repo = repo; 
            this._repoDriver = repoDriver;
            this._repoTrack = repoTrack;
            this._repoDriverResult = repoDriverResult;
        }

       
        public async Task<ResponseDTO> GetRaceResults(FilterPageVM filter)
        {
            var races = await _repo.GetAll();
            var drivers = await _repoDriver.GetAll();
            var tracks = await _repoTrack.GetAll();

            // filter for only winners
            var winners = _repoDriverResult.GetWhere(x => x.Position == 1).Result;

            var results = (from a in races
                           join b in drivers on a.PoleDriver equals b.Id into pDriverLeft
                           from pDriver in pDriverLeft.DefaultIfEmpty()
                           join c in tracks on a.TrackId equals c.Id
                           join d in drivers on a.FastestLapDriver equals d.Id into fDriverLeft
                           from fDriver in fDriverLeft.DefaultIfEmpty()
                           join e in winners on a.Id equals e.ResultId into driverLeft
                           from winner in driverLeft.DefaultIfEmpty()
                           orderby a.RaceDate

                           select new RaceResultVM()
                           {
                               Id = a.Id,
                               FastestLapDriver = (fDriver != null) ? fDriver.Name : "",
                               FastestLapTime = a.FastestLapTime,
                               PoleDriver = (pDriver != null) ? pDriver.Name : "",
                               PoleTime = a.PoleTime,
                               RaceDate = a.RaceDate.ToShortDateString(),
                               TrackLocation = c.Location,
                               TrackId = a.TrackId,
                               RaceWinner = (winner != null) ? getRaceWinner(winner.DriverId) : ""
                           }
                      ).AsQueryable<RaceResultVM>();


            //this uses dynamic linq so we can query any field in the table
            //we can even use this to return only certain fields
            if (!String.IsNullOrEmpty(filter.SearchStr)) results = results.Where(filter.SearchStr);

            var pageResult = results.Skip((filter.ItemsPerPage * (filter.PageNo - 1))).Take(filter.ItemsPerPage).AsQueryable();
            

            if (!String.IsNullOrEmpty(filter.SortStr)) pageResult = pageResult.OrderBy(filter.SortStr);
          

            return new ResponseDTO(results.Count(), pageResult);

        }

        public async Task<ResponseDTO> GetAllRaceResuts()
        {
            var races = await _repo.GetAll();
            var drivers =  await _repoDriver.GetAll();
            var tracks = await _repoTrack.GetAll();

            // filter for only winners
            var winners =  _repoDriverResult.GetWhere(x => x.Position == 1).Result;

            var results = (from a in races
                           join b in drivers on a.PoleDriver equals b.Id into pDriverLeft
                           from pDriver in pDriverLeft.DefaultIfEmpty()
                           join c in tracks on a.TrackId equals c.Id
                           join d in drivers on a.FastestLapDriver equals d.Id into fDriverLeft
                           from fDriver in fDriverLeft.DefaultIfEmpty()
                           join e in winners on a.Id equals e.ResultId into driverLeft
                           from winner in driverLeft.DefaultIfEmpty()
                           orderby a.RaceDate

                           select new RaceResultVM()
                           {
                               Id = a.Id,
                               FastestLapDriver = (fDriver != null) ? fDriver.Name : "",
                               FastestLapTime = a.FastestLapTime,
                               PoleDriver = (pDriver != null) ? pDriver.Name : "",
                               PoleTime = a.PoleTime,
                               RaceDate = a.RaceDate.ToShortDateString(),
                               TrackLocation = c.Location,
                               TrackId = a.TrackId,
                               RaceWinner = (winner != null) ? getRaceWinner(winner.DriverId) : ""
                           }
                      ).AsQueryable<RaceResultVM>();


            return new ResponseDTO(races.Count(), races);

        }

        public async Task<RaceResult> SaveRaceResult(RaceResult race)
        {
            if (!this.Validate(race))
            {
                //check the model is valid else throw an excception
                //throw exception                
                throw new ValidationException("Invalid Race");
            }

            return await _repo.Add(race);
        }

        public async Task<ResponseDTO> GetRaceResult(long id)
        {
            string winnerName = "";
            var r = await _repo.GetById(id);
            //only contine if we ger result back
            if(r==null) return new ResponseDTO(0, null);

            var d = await _repoDriver.GetById(r.PoleDriver);
            var f = await _repoDriver.GetById(r.FastestLapDriver);
            var t = await _repoTrack.GetById(r.TrackId);

            var e = await _repoDriverResult.GetWhere(x => x.ResultId == id && x.Position==1);
           
            if (e.Count()>0)
            {
               
                var winner = await _repoDriver.GetById(e.FirstOrDefault().DriverId);
                winnerName = winner.Name;
            }

            if (r != null)
            {
                var race = new RaceResultVM();

                race.Id = r.Id; ;
                race.FastestLapTime = r.FastestLapTime;
                race.PoleDriver = (d != null) ?d.Name : "";
                race.FastestLapDriver = (f != null) ? f.Name : "";
                race.PoleTime = r.PoleTime;
                race.TrackLocation = (t != null) ? t.Location : "";
                race.RaceDate = r.RaceDate.ToShortDateString();
                race.TrackId = r.TrackId;
                race.RaceWinner = winnerName;
                return new ResponseDTO(1, race);
            }
            else
            {
                return new ResponseDTO(0, null);
            }

        }

        public async Task<RaceResult> DeleteRaceResult(long id)
        {

            return await _repo.Delete(id);
        }

        public async Task<RaceResult> UpdateRaceResult(RaceResult race)
        {
            //use not tracking as it will error when try to update beuase entity tracking will be turned on
            var r = await _repo.GetWhereNoTracking(x => x.Id == race.Id);
 
            //now check all fields to see which have chnaged and only update those that have.
            race.PoleTime = (r.FirstOrDefault().PoleTime != race.PoleTime) ? race.PoleTime : r.FirstOrDefault().PoleTime;

            return await _repo.Update(race);
        }

        public string getRaceWinner(long id)
        {
           var a = _repoDriver.GetById(id).GetAwaiter().GetResult();
            return a.Name;
        }

        public async Task<ResponseDTO> GetRacesByDate(DateTime raceDate)
        {
            var races = await _repo.GetWhere(x => x.RaceDate >=raceDate);
            var drivers = await _repoDriver.GetAll();
            var tracks = await  _repoTrack.GetAll();

            // filter for only winners
            var winners =  _repoDriverResult.GetWhere(x=>x.Position==1).Result;

            var results = (from a in races
                           join b in drivers on a.PoleDriver equals b.Id into pDriverLeft
                           from pDriver in pDriverLeft.DefaultIfEmpty()
                           join c in tracks on a.TrackId equals c.Id 
                           join d in drivers on a.FastestLapDriver equals d.Id into fDriverLeft
                           from fDriver in fDriverLeft.DefaultIfEmpty()
                           join e in winners on a.Id equals e.ResultId into driverLeft
                           from winner in driverLeft.DefaultIfEmpty()
                           orderby a.RaceDate

                           select new RaceResultVM()
                           {
                               Id = a.Id,
                               FastestLapDriver = (fDriver != null) ? fDriver.Name : "",
                               FastestLapTime = a.FastestLapTime,
                               PoleDriver = (pDriver != null) ? pDriver.Name : "",
                               PoleTime = a.PoleTime,
                               RaceDate = a.RaceDate.ToShortDateString(),                             
                               TrackLocation = c.Location,
                               TrackId = a.TrackId,
                               RaceWinner = (winner != null) ?  getRaceWinner(winner.DriverId) : ""
                           }
                      ).AsQueryable<RaceResultVM>();

      
            return new ResponseDTO(results.Count(), results);

        }

        private bool Validate(RaceResult race)
        {
            //validate here
            //if (!race.PoleTime) return false;


            return true;
        }


    }
}
