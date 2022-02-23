using Akka.Actor;
using Microsoft.AspNetCore.Mvc;

namespace ASPAkka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IActorRef __userActor;

        public UserController(UserActorProvider userActorProvider)
        {
            this.__userActor = userActorProvider();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this.__userActor.Ask<IEnumerable<User>>(new GetUsers());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await this.__userActor.Ask(new GetUserById(id));
            return result switch
            {
                User user => Ok(user),
                _ => BadRequest()
            };
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateUser createUser)
        {
            this.__userActor.Tell(createUser);
            return Accepted();
        }
    }
}
