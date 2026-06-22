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
    public class TracksController : ControllerBase
    {
        private readonly TrackService _service;

        public TracksController(TrackService service)
        {
            _service = service;
        }

        // GET: api/Teams
        [HttpPost("GetTracks", Name = "GetTracks")]

        public async Task<ActionResult<ResponseDTO>> GetTracks(FilterPageVM filter)
        {
           

            if (filter.PageNo == 0 && filter.ItemsPerPage == 0 && filter.SortStr == "" && filter.SearchStr == "")
            {
                //return all teams
                return Ok( await _service.GetAllTracks());
            }
            else
            {
                return Ok( await _service.GetTracks(filter));
            }

        }

        // POST: api/Tracks
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        
        public async Task<ActionResult<Track>> PostTrack(Track track)
        {
            
            try
            {
                var t =  await _service.SaveTrack(track);

                //so in the new version using repo the call from the service will return an
                //Driver Object which can then be fed into the below

                //we must add routing to the top of the getteam method so the 
                //call below knows how to call it
                //this creates a 201 repsonse so we dont need to create one

                //these do much the same thing they just use diffeernt to find method to call
                return CreatedAtAction("GetTrack", new { id = t.Id }, t);
               // return CreatedAtRoute("GetTrack", new { id = t.Id }, t);                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }

 
        }

        // DELETE: api/Tracks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Track>> DeleteTrack(long id)
        {
            

            try
            {
                var team =await  _service.DeleteTrack(id);
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



        // GET: api/Tracks/5
        //i needed to name this route so we can call the 
        [HttpGet("{id}", Name = "GetTrack")]        
        public async Task<ActionResult<Track>> GetTrack(long id)
        {

            try
            {
                var track =await _service.GetTrack(id);
                //check if we can find it
                if (track != null)
                {
                    return Ok(track);
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
        public async Task<ActionResult<Track>> PutTrack(Track track)
        {

            

            try
            {
                var updatedObject =await  _service.UpdateTrack(track);
                return Ok(updatedObject);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
    }
}