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

namespace F1WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverResultController : ControllerBase
    {
        private readonly DriverResultService _service;

        public DriverResultController(DriverResultService service)
        {
            _service = service;
        }

         

        // GET: api/Teams
        [HttpPost("GetDriverResults", Name = "GetDriverResults")]
        public async Task<ActionResult<ResponseDTO>> GetDriverResults(FilterPageVM filter)
        {
            if (filter.PageNo == 0 && filter.ItemsPerPage == 0 && filter.SortStr == "")
            {
                //return all teams
                return Ok(await _service.GetAllDriverResuts());
            }
            else
            {
                return Ok(await _service.GetDriverResults(filter));
            }

        }

        // POST: api/Tracks
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]

        public async Task<ActionResult<List<DriverResult>>> PostDriverResult(List<DriverResult> result)
        {

            try
            {
              
                var r = await  _service.SaveRaceResult(result);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }


        }

        // DELETE: api/DriverResult/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DriverResult>> DeleteDriverResult(long id)
        {


            try
            {
                var result =await _service.DeleteDriverResult(id);
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


      
        // GET: api/DriverResult/5
        //i needed to name this route so we can call the 
        [HttpGet("{id}", Name = "GetDriverResult")]
        public async  Task<ActionResult<DriverResult>> GetDriverResult(long id)
        {

            try
            {
                var result = await _service.GetDriverResultsByRace(id);
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
        public async Task<ActionResult<DriverResult>> PutDriverResult(DriverResult result)
        {



            try
            {
                var updatedObject = await _service.UpdateDriverResult(result);
                return Ok(updatedObject);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }

    }
}