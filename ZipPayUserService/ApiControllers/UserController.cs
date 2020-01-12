using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ZipPayUserService.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("")]
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult> List()
        {
            return Ok(await _userService.GetAllUsersAsync());
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