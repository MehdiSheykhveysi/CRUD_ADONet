using App.Domain;
using App.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace App.Endpoint.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserApiController : ControllerBase
    {

        private readonly ILogger<UserApiController> _logger;
        private readonly IUserService userService;

        public UserApiController(ILogger<UserApiController> logger,IUserService userService)
        {
            _logger = logger;
            this.userService = userService;
        }

        [HttpGet]
        public Task<IList<User>> Get(CancellationToken cancellationToken) => userService.GetUsersAsync(cancellationToken);

        [HttpPost]
        public Task<User> Post([FromBody] User user, CancellationToken cancellationToken) => userService.CreateAsync(user, cancellationToken);

        [HttpPut]
        public Task<User> Put([FromBody] User user, CancellationToken cancellationToken) => userService.UpdateAsync(user, cancellationToken);

        [HttpDelete]
        public Task<User> Delete([FromBody] User user, CancellationToken cancellationToken) => userService.DeleteAsync(user, cancellationToken);
    }
}