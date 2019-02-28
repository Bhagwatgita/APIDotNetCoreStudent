using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaoLayer.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace APITEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IApiServiceDao dao;
        private readonly IDbResult dbResult;

        public ValuesController(IApiServiceDao dao, IDbResult dbResult)
        {
            this.dao = dao;
            this.dbResult = dbResult;

        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var a = dao.GetConnectionString();
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
