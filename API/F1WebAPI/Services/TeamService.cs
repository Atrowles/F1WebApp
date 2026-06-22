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
    public class TeamService
    {
        private readonly IRepositoryBase<Team> _repo;

        public TeamService(IRepositoryBase<Team> repo)
        {
            this._repo = repo;           
        }


        public async Task<ResponseDTO> GetTeams(FilterPageVM filter)
        {
            var t =await _repo.GetAll();

            var teams = (from results in t
                          select new TeamVM()
                          {
                              Id = results.Id,
                              CurrentPoints = results.CurrentPoints,
                              CustomerEngine = results.CustomerEngine,
                              Engine = results.Engine,
                              Name = results.Name,
                          }
                        ).AsQueryable<TeamVM>();

            if (!String.IsNullOrEmpty(filter.SearchStr)) teams = teams.Where(filter.SearchStr);

            var pageResult = teams.Skip((filter.ItemsPerPage * (filter.PageNo - 1))).Take(filter.ItemsPerPage).AsQueryable();

            

            if (!String.IsNullOrEmpty(filter.SortStr)) pageResult = pageResult.OrderBy(filter.SortStr);

            return new ResponseDTO(teams.Count(), pageResult);

        }

        public async Task<ResponseDTO> GetAllTeams()
        {
            var t = await _repo.GetAll();

            var teams = (from results in t
                         select new TeamVM()
                         {
                             Id = results.Id,
                             CurrentPoints = results.CurrentPoints,
                             CustomerEngine = results.CustomerEngine,
                             Engine = results.Engine,
                             Name = results.Name,
                         }
                      ).AsQueryable<TeamVM>();

            return new ResponseDTO(teams.Count(), teams);

        }

        public async Task<Team> SaveTeam(Team team)
        {
            if (!this.Validate(team))
            {
                //check the model is valid else throw an excception
                //throw exception                
                throw new ValidationException("Invalid Team");
            }

            return await _repo.Add(team);
        }

        public async Task<ResponseDTO> GetTeam(long id)
        {
            var t = await _repo.GetById(id);

            if (t != null)
            {
                var team = new TeamVM();

                team.Id = t.Id;
                team.CurrentPoints = t.CurrentPoints;
                team.CustomerEngine = t.CustomerEngine;
                team.Engine = t.Engine;
                team.Name = t.Name;
                
                return new ResponseDTO(1, team);
            }
            else
            {
                return new ResponseDTO(0, null);
            }

        }

        public async Task<Team> DeleteTeam(long id)
        {

             return await _repo.Delete(id);
        }

        public async Task<Team> UpdateTeam(Team team)
        {

            //use not tracking as it will error when try to update beuase entity tracking will be turned on
            var t= await _repo.GetWhereNoTracking(x => x.Id == team.Id);

            //now check all fields to see which have chnaged and only update those that have.
            team.Name = (t.FirstOrDefault().Name != team.Name && !String.IsNullOrEmpty(team.Name)) ? team.Name : t.FirstOrDefault().Name;

            return await _repo.Update(team);
        }

        public async Task<ResponseDTO> GetTeamsByName(string name, int page, int itemsPerPage, string sortString)
        {
            var t = await _repo.GetWhere(x => x.Name.Contains(name) || x.Engine.Contains(name));

            var teams = (from results in t
                         select new TeamVM()
                         {
                             Id = results.Id,
                             CurrentPoints = results.CurrentPoints,
                             CustomerEngine = results.CustomerEngine,
                             Engine = results.Engine,
                             Name = results.Name,
                         }
                    ).AsQueryable<TeamVM>();

            var pageResult = teams.Skip((itemsPerPage * (page - 1))).Take(itemsPerPage).AsQueryable();

            if (!String.IsNullOrEmpty(sortString)) pageResult = pageResult.OrderBy(sortString);
            return new ResponseDTO(teams.Count(), pageResult);

        }

        private bool Validate(Team team)
        {
            //validate here
           // if (!team.CustomerEngine) return false;


            return true;
        }

        
        //should maybe add getpaged etc in here

    }
}
