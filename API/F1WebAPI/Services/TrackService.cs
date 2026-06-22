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
    public class TrackService
    {
        private readonly IRepositoryBase<Track> _repo;

        public TrackService(IRepositoryBase<Track> repo)
        {
            this._repo = repo;
        }

        public async Task<ResponseDTO> GetTracks(FilterPageVM filter)
        {
            var t = await _repo.GetAll();            
            //make it queryable so we can use sql like commands on it
            //this uses dyncamic linq so we can pass in a string value
            //we could also use this with a where clause if we wanted too
            

            var tracks = (from results in t
                          select new TrackVM()
                          {
                              FastestLap = results.FastestLap,
                              Id = results.Id,
                              Length = results.Length,
                              Location = results.Location
                          }
                                    ).AsQueryable<TrackVM>();

            if (!String.IsNullOrEmpty(filter.SearchStr)) tracks = tracks.Where(filter.SearchStr);

            var pageResult = tracks.Skip((filter.ItemsPerPage * (filter.PageNo - 1))).Take(filter.ItemsPerPage).AsQueryable();
            ;
            if (!String.IsNullOrEmpty(filter.SortStr)) pageResult = pageResult.OrderBy(filter.SortStr);

            return new ResponseDTO(tracks.Count(), pageResult);

        }

        public async Task<ResponseDTO> GetAllTracks()
        {
            var t =  await _repo.GetAll();

            return new ResponseDTO(t.Count(), t);

        }

        public async Task<ResponseDTO> GetTracksByLocation(string location,int page, int itemsPerPage, string sortString)
        {
            var t= await  _repo.GetWhere(x => x.Location.Contains(location));
  
            var tracks = (from results in t
                          select new TrackVM()
                          {
                              FastestLap = results.FastestLap,
                              Id = results.Id,
                              Length = results.Length,
                              Location = results.Location
                          }
                          ).AsQueryable<TrackVM>();

            var pageResult = tracks.Skip((itemsPerPage * (page - 1))).Take(itemsPerPage).AsQueryable();

            if (!String.IsNullOrEmpty(sortString)) pageResult = pageResult.OrderBy(sortString);

            return new ResponseDTO(tracks.Count(), pageResult);

        }

        public async Task<Track> SaveTrack(Track track)
        {
            if (!this.Validate(track))
            {
                //check the model is valid else throw an excception
                //throw exception                
                throw new ValidationException("Invalid Track");
            }

            return await _repo.Add(track);
        }

        public async Task<ResponseDTO> GetTrack(long id)
        {
            var t = await _repo.GetById(id);


            
            if (t != null)
            {
                var track = new TrackVM();

                track.Id = t.Id;
                track.Location = t.Location;
                track.Length = t.Length;
                track.FastestLap = t.FastestLap;

                return new ResponseDTO(1, track);
            }
            else
            {
                return new ResponseDTO(0, null);
            }
            
        }

        public async Task<Track> DeleteTrack(long id)
        {

            return await _repo.Delete(id);
        }

        public async Task<Track> UpdateTrack(Track track)
        {
            //should call validate again here
            //use not tracking as it will error when try to update beuase entity tracking will be turned on
            var t =  await _repo.GetWhereNoTracking(x => x.Id == track.Id);

            //now check all fields to see which have chnaged and only update those that have.
            track.Length = (t.FirstOrDefault().Length != track.Length && track.Length!=0) ? track.Length : t.FirstOrDefault().Length;
            return await _repo.Update(track);
        }

        private bool Validate(Track track)
        {
            //validate here
            if (track.Location == "") return false;


            return true;
        }




    }
}
