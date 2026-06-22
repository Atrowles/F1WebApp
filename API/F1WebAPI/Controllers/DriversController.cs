using System ;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace F1WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly DriverService _service;

        public DriversController(DriverService service)
        {
            _service = service;
        }

        // GET: api/Drivers
        [HttpPost("GetDrivers", Name = "GetDrivers")]

        //[Authorize]
        public async Task<ActionResult<ResponseDTO>> GetDrivers(FilterPageVM filter)
        {
            //link team id to teams table here
            //prbs need to created viewmodels for these as we dont want team name in driver table
            //remember to remove it
            if (filter.PageNo == 0 && filter.ItemsPerPage == 0 && filter.SortStr == "" && filter.SearchStr == "")
            {
                //return all teams
                return Ok( await _service.GetAllDrivers());
            }
            else
            {
                return Ok( await _service.GetDrivers(filter));
            }


            
        }

        // POST: api/Drivers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        
        public async Task<ActionResult<Driver>> PostDriver(Driver driver)
        {

            try
            {
                var d = await _service.SaveDriver(driver);

                //so in the new version using repo the call from the service will return an
                //Driver Object which can then be fed into the below

                //we must add routing to the top of the getteam method so the 
                //call below knows how to call it
                //this creates a 201 repsonse so we dont need to create one
                return CreatedAtAction("GetDriver", new { id = d.Id }, d);
                //return CreatedAtRoute("GetDriver", new { id = d.Id }, d);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }

        }

        // DELETE: api/Driver/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Driver>> DeleteDriver(long id)
        {
            

            try
            {
                var driver =await _service.DeleteDriver(id);
                if (driver == null)
                {
                    return NotFound();
                }

                return Ok(driver);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }

        }


        // GET: api/Drivers/5
        //i needed to name this route so we can call the 
        [HttpGet("{id}", Name = "GetDriver")]


        public async Task<ActionResult<ResponseDTO>> GetDriver(long id)
        {

            try
            {
                var driver =  await _service.GetDriver(id);
                //check if we can find it
                if (driver != null)
                {
                    return Ok(driver);
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
        public async Task<ActionResult<Driver>> PutDriver(Driver driver)
        {

            

            try
            {
                var updatedObject = await _service.UpdateDriver(driver);
                return Ok(updatedObject);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
            }
        }
    }
}