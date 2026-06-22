using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using F1WebAPI.DTOs;
using F1WebAPI.Models;
using F1WebAPI.Repositories;
using F1WebAPI.Services;
using F1WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace F1WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaceResultController : ControllerBase
    {
        private readonly RaceResultService _service;

        public RaceResultController(RaceResultService service)
        {
            _service = service;
            
        }

        // POST: api/Teams
        //need to define api path here so its not ambiguous with other post method
        [HttpPost("GetRaceResults", Name = "GetRaceResults")]
        [Authorize]
        public async Task<ActionResult<ResponseDTO>> GetRaceResults(FilterPageVM filter)
        {
            if (filter.PageNo == 0 && filter.ItemsPerPage == 0 && filter.SearchStr == "")
            {
                //return all teams
                return Ok(await _service.GetAllRaceResuts());
            }
            else
            {
                return Ok( await _service.GetRaceResults(filter));
            }

        }

        // POST: api/RaceResult
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RaceResult>> PostRaceResult(RaceResult result)
        {

            try
            {
                var r = await _service.SaveRaceResult(result);

                return CreatedAtAction("GetRaceResult", new { id = r.Id }, r);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }


        }

        // DELETE: api/RaceResult/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RaceResult>> DeleteRaceResult(long id)
        {


            try
            {
                var result =await _service.DeleteRaceResult(id);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }

        }


        [HttpGet("{raceDate}/RaceResult", Name = "GetRaceByDate")]
        [Authorize]
        public async Task<ActionResult<ResponseDTO>> GetRaceByDate(string raceDate)
        {
            //raceDate must be in 12-12-2001 format
         
            try
            {
                var results = await _service.GetRacesByDate(DateTime.Parse(raceDate));


                //check if we can find it
                if (results != null)
                {
                    return Ok(results);
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

        // GET: api/RaceResult/5
        //i needed to name this route so we can call the 
        [HttpGet("{id}", Name = "GetRaceResult")]
        public async Task<ActionResult<RaceResult>> GetRaceResult(long id)
        {

            try
            {
                var result =  await _service.GetRaceResult(id);
                //check if we can find it
                if (result != null)
                {
                    return Ok(result);
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
        public async Task<ActionResult<RaceResult>> PutDriverResult(RaceResult result)
        {



            try
            {
                var updatedObject =  await _service.UpdateRaceResult(result);
                return Ok(updatedObject);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
    }
}