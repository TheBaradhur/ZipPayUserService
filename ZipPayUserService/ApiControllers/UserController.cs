using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ZipPayUserService.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [Route("")]
        [Route("list")]
        [HttpGet]
        public List<string> List()
        {
            return new List<string> { "list" };
        }

        [Route("get")]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("create")]
        [HttpPost]
        public int Create([FromBody] string value)
        {
            return 1;
        }
    }
}