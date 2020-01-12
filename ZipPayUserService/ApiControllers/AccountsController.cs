using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ZipPayUserService.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        [Route("list")]
        [HttpGet]
        public List<string> List()
        {
            return new List<string> { "list" };
        }

        [Route("create")]
        [HttpPost]
        public int Create([FromBody] string value)
        {
            return 1;
        }
    }
}