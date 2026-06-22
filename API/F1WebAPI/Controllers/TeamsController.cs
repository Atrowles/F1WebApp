using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using F1WebAPI.Context;
using F1WebAPI.DTOs;
using F1WebAPI.Models;
using F1WebAPI.Repositories;
using F1WebAPI.Services;
using F1WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace F1WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly TeamService _service;

        public TeamsController(TeamService service)
        {
            _service = service;
        }

        // GET: api/Teams
        [HttpPost("GetTeams", Name = "GetTeams")]

        public async Task<ActionResult<ResponseDTO>> GetTeams(FilterPageVM filter)
        {
            if(filter.PageNo==0 && filter.ItemsPerPage==0 && filter.SortStr == "" && filter.SearchStr == "")
            {
                //return all teams
                return Ok( await _service.GetAllTeams());
            }
            else
            {
                return Ok(await  _service.GetTeams(filter));
            }
            
        }





        // POST: api/Drivers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        
        public async Task<ActionResult<Team>> PostTeam(Team team)
        {

            try
            {
                var t = await _service.SaveTeam(team);

                //so in the new version using repo the call from the service will return an
                //Driver Object which can then be fed into the below

                //we must add routing to the top of the getteam method so the 
                //call below knows how to call it
                //this creates a 201 repsonse so we dont need to create one
                return CreatedAtRoute("GetTeam", new { id = t.Id }, t);
                //return CreatedAtRoute("GetTeam", new { id = t.Id }, t);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }

        }

        // DELETE: api/Tracks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Team>> DeleteTeam(long id)
        {
            

            try
            {
                var team =  await _service.DeleteTeam(id);
                if (team == null)
                {
                    return NotFound();
                }

                return Ok(team);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
 
        }


        // GET: api/Drivers/5
        //i needed to name this route so we can call the 
        [HttpGet("{id}", Name = "GetTeam")]

        public async Task<ActionResult<ResponseDTO>> GetTeam(long id )
        {

            try
            {
                var team =  await _service.GetTeam(id);
                //check if we can find it
                if (team != null)
                {
                    return Ok(team);
                }
                 else
                {
                    return NotFound();
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

         }

        [HttpGet("{name}/Teams", Name = "GetTeamsByName")]       
        public async Task<ActionResult<ResponseDTO>> GetTeamsByName(string name, int page, int itemsPerPage, string sortString)
        {

            try
            {
                var teams = await _service.GetTeamsByName(name, page, itemsPerPage, sortString);
                //check if we can find it
                if (teams != null)
                {
                    return Ok(teams);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public async Task<ActionResult<Team>> PutTeam(Team team)
        {

            try
            {
                var updatedObject = await _service.UpdateTeam(team);
                return Ok(updatedObject);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
    }
}