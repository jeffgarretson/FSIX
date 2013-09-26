using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FSIX.Web.Controllers
{
    public class ExampleController : ApiController
    {
        // GET api/example
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/example/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/example
        public void Post([FromBody]string value)
        {
        }

        // PUT api/example/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/example/5
        public void Delete(int id)
        {
        }
    }
}
