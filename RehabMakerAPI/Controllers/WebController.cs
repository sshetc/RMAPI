using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RehabMakerAPI.Controllers
{
   
    public class WebController : ApiController
    {
        // GET api/<controller>
        public string Get()
        {
            return "Oks";
        }

        // GET api/<controller>/5
        [Route("{id}")]
        public string GetOk([FromUri]string id)
        {
            return "Ok";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}