using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaoLayer.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APITEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IApiServiceDao dao;
        private readonly IDbResult dbResult;

        public StudentController(IApiServiceDao dao,IDbResult dbResult)
        {
            this.dao = dao;
            this.dbResult = dbResult;
            
        }
        [HttpGet]
        public IActionResult GetData()
        {
            var a=dao.GetConnectionString();
            return Ok("Hello");
        }
    }
}