using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using RehabMakerAPI.Models;

namespace RehabMakerAPI.Controllers
{

    public class DevicesController : ApiController
    {
        private RehabApiDataBaseContext db = new RehabApiDataBaseContext();


        //[HttpGet]
        //[Route("{paramOne}/{paramTwo}")]
        //public string Get(int paramOne, int paramTwo)
        //{
        //    return "The [Route] with multiple params worked";
        //}


        //[Route("number={number}&speed={speed}", Name = "Set")]
        //[Route("{number}/{speed}", Name = "Set")]
        [HttpGet]
        [Route("{number}/{speed}",Name="Set")]
        public async Task<IHttpActionResult> SetDevice([FromUri]string number,[FromUri]string speed)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var device = db.Device.Include(m => m.Params).FirstOrDefault(m => m.Number == number);

            try
            {
                //---------------Если устр-во не найдено - создается новая сущность в БД
                if(device == null)
                {
                    Device device_ = new Device();
                    device_.Number = number;
                    db.Device.Add(device_);
                    db.SaveChanges();
                    device = db.Device.Include(m => m.Params).FirstOrDefault(m => m.Number == number);
                }


                double Distance = double.Parse(speed) * 0.5;
                double Calories = Distance * 0.5 * 70;
                string dateTime = DateTime.Today.ToShortDateString();

                ////---------------Запись данных в Params-----------------------------------

                var param_dev = db.Params.FirstOrDefault(a => a.IdDevice == device.IdDevice);
                Params params_;
                if (param_dev != null) {
                    params_ = await db.Params.FirstOrDefaultAsync(model => model.IdDevice == param_dev.IdDevice);
                    params_.IdDevice = device.IdDevice;
                    params_.Speed = decimal.Parse(speed);
                    params_.Distance = decimal.Parse(Distance.ToString());
                    params_.Сalories = decimal.Parse(Calories.ToString());
                    params_.Date = Convert.ToDateTime(dateTime);
                    db.Entry(params_).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    params_ = new Params();
                    params_.IdDevice = device.IdDevice;
                    params_.Speed = decimal.Parse(speed);
                    params_.Distance = decimal.Parse(Distance.ToString());
                    params_.Сalories = decimal.Parse(Calories.ToString());
                    params_.Date = Convert.ToDateTime(dateTime);
                    db.Params.Add(params_);
                    db.SaveChanges();
                }
                param_dev = db.Params.FirstOrDefault(a => a.IdDevice == device.IdDevice);
                ////---------------Запись данных в Statistics-----------------------------------

                if(Convert.ToDecimal(speed) + 1 == 1)
                {
                    return Ok("Ошибка нуля");
                }

                var stat_dev = db.Statistics.FirstOrDefault(a => a.IdDevice == device.IdDevice && a.Date == param_dev.Date);
                Statistics statistics;

                if (stat_dev != null)
                {
                    statistics = await db.Statistics.FirstOrDefaultAsync(model => model.IdDevice == stat_dev.IdDevice && model.Date == param_dev.Date);
                    decimal pi = (Convert.ToDecimal(statistics.AverageSpeed) + params_.Speed) / 2;
                    pi = Math.Round(pi, 2);
                    //statistics.AverageSpeed = Convert.ToString(Convert.ToDecimal(statistics.AverageSpeed) + params_.Speed);'
                    statistics.AverageSpeed = Convert.ToString(pi);
                    statistics.TotalDistance = Convert.ToString(Convert.ToDecimal(statistics.TotalDistance) + params_.Distance);
                    statistics.TotalCalories = Convert.ToString(Convert.ToDecimal(statistics.TotalCalories) + params_.Сalories);
                    statistics.Date = params_.Date;
                    statistics.IdDevice = params_.IdDevice;
                    db.Entry(statistics).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    statistics = new Statistics();
                    statistics.AverageSpeed = Convert.ToString(params_.Speed);
                    statistics.TotalDistance = Convert.ToString(params_.Distance);
                    statistics.TotalCalories = Convert.ToString(params_.Сalories);
                    statistics.Date = params_.Date;
                    statistics.IdDevice = params_.IdDevice;
                    db.Statistics.Add(statistics);
                    db.SaveChanges();
                }

                return Ok($"Данные успешно зафиксированы(Speed = {params_.Speed}, Distance = {params_.Distance}, Calories = {params_.Сalories})");
            }
            catch(Exception e)
            {
                return BadRequest(Convert.ToString(e));

            }
           
        }
        
        // GET: api/Devices
        public IQueryable<Device> GetDevice()
        {
            return db.Device;
        }

        // GET: api/Devices/5
        [ResponseType(typeof(Device))]
        public async Task<IHttpActionResult> GetDevice(int id)
        {
            Device device = await db.Device.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }

        // PUT: api/Devices/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDevice(int id, Device device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != device.IdDevice)
            {
                return BadRequest();
            }

            db.Entry(device).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
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

        // POST: api/Devices
        [ResponseType(typeof(Device))]
        public async Task<IHttpActionResult> PostDevice(Device device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Device.Add(device);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = device.IdDevice }, device);
        }

        // DELETE: api/Devices/5
        [ResponseType(typeof(Device))]
        public async Task<IHttpActionResult> DeleteDevice(int id)
        {
            Device device = await db.Device.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            db.Device.Remove(device);
            await db.SaveChangesAsync();

            return Ok(device);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeviceExists(int id)
        {
            return db.Device.Count(e => e.IdDevice == id) > 0;
        }
    }
}