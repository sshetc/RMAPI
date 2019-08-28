using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using RehabMakerAPI.Models;

namespace RehabMakerAPI.Controllers
{
  
    public class ParamsController : ApiController
    {
        private readonly RehabApiDataBaseContext _context = new RehabApiDataBaseContext();

        // GET: api/Params
        //[HttpGet]
        //public IEnumerable<Params> GetParams()
        //{
        //    return _context.Params;
        //}

        [HttpGet]
        [ResponseType(typeof(Params))]
        [Route("{id}/{simbol}/{ugo}", Name ="Para")]
        public IHttpActionResult ParaParams([FromUri]string id, [FromUri]string simbol, [FromUri]string ugo)
        {
            try {

                if
                (simbol == "0")
                {
                    var device = _context.Device.FirstOrDefault(m => m.Number == id);
                    var _params = _context.Params.FirstOrDefault(m => m.IdDevice == device.IdDevice);
                    var __params = _context.Params.Find(_params.IdParams);
                    return Ok(__params);
                }
                else if (simbol == "1")
                {
                    var device = _context.Device.FirstOrDefault(m => m.Number == id);
                    var _params = _context.Statistics.FirstOrDefault(m => m.IdDevice == device.IdDevice);
                    double averageSpeed = 0;
                    double totalDistance = 0;
                    double totalCalories = 0;

                    foreach (Statistics statistics in _context.Statistics)
                    {
                        if (statistics.IdDevice == device.IdDevice)
                        {
                            //statistics.AverageSpeed = statistics.AverageSpeed.Replace(".", ",");
                            averageSpeed += Convert.ToDouble(statistics.AverageSpeed);

                            //statistics.TotalDistance = statistics.TotalDistance.Replace(".", ",");
                            totalDistance += Convert.ToDouble(statistics.TotalDistance);

                            //statistics.TotalCalories = statistics.TotalCalories.Replace(".", ",");
                            totalCalories += Convert.ToDouble(statistics.TotalCalories);
                        }
                    }
                    var __params = _context.Statistics.FirstOrDefault();
                    __params.AverageSpeed = Convert.ToString(averageSpeed);
                    __params.TotalCalories = Convert.ToString(totalDistance);
                    __params.TotalDistance = Convert.ToString(totalCalories);
                    return Ok(__params);
                }
                else if (simbol == "2")
                {
                    try { 
                    var device = _context.Device.FirstOrDefault(m => m.Number == id);
                    var _params = _context.Statistics.FirstOrDefault(m => m.IdDevice == device.IdDevice);
                    var __params = _context.Statistics.FirstOrDefault();

                    string averageSpeed;
                    string totalDistance;
                    string totalCalories;
                    string dateTime = "";
                    foreach (Statistics statistics in _context.Statistics)
                    {

                        dateTime = statistics.Date.ToShortDateString();
                        dateTime = dateTime.Replace(".", "/");
                        statistics.Date = Convert.ToDateTime(dateTime);
                        ugo = ugo.Replace(".", "/");

                        if (statistics.IdDevice == device.IdDevice && dateTime == Convert.ToString(ugo))
                        {
                            
                            averageSpeed = Convert.ToString(statistics.AverageSpeed);
                            totalDistance = Convert.ToString(statistics.TotalDistance);
                            totalCalories = Convert.ToString(statistics.TotalCalories);

                            __params.AverageSpeed = averageSpeed;
                            __params.TotalCalories = totalDistance;
                            __params.TotalDistance = totalCalories;
                            return Ok(_params);
                        }
                    }
                    }
                    catch
                    {
                        return Ok("Error");
                    }

                }


            }
            catch(Exception e) {
                return Ok(e);
            }
            
            return NotFound();
        }


        // PUT: api/Params/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutParams(int id, Params @params)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @params.IdParams)
            {
                return BadRequest();
            }

            _context.Entry(@params).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParamsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Params
        [ResponseType(typeof(Params))]
        public async Task<IHttpActionResult> PostParams(Params @params)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Params.Add(@params);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = @params.IdParams }, @params);
        }

        // DELETE: api/Params/5
        [ResponseType(typeof(Params))]
        public async Task<IHttpActionResult> DeleteParams(int id)
        {
            Params @params = await _context.Params.FindAsync(id);
            if (@params == null)
            {
                return NotFound();
            }

            _context.Params.Remove(@params);
            await _context.SaveChangesAsync();

            return Ok(@params);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParamsExists(int id)
        {
            return _context.Params.Count(e => e.IdParams == id) > 0;
        }
    }
}