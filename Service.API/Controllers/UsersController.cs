using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.API.Dtos;
using Service.Common.Interfaces.Service;
using Service.Common.Models;

namespace Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : MainBaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService, IMapper mapper, ILogger logger) 
            : base(mapper, logger)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers([FromQuery] string? login, [FromQuery] string? email)
        {
            var users = await _userService.GetAll(login, email);
            if (users == null || users.Count == 0) return Ok(Array.Empty<UserDto>());

            List<object> result = new List<object>(users?.Count ?? 0);
            foreach (var user in users!)
            {
                if (user is CustomerAccountModel customer)
                    result.Add(_mapper.Map<CustomerDto>(customer));
                else
                    result.Add(_mapper.Map<UserDto>(user));
            }

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userService.Get(id);
            if (user == null) return BadRequest();

            return Ok(user is CustomerAccountModel customer ? _mapper.Map<CustomerDto>(customer) : _mapper.Map<UserDto>(user));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateUser(int id, AccountRegisterDto accountRegister)
        {
            var oldAccaunt = await _userService.Get(id);
            if (oldAccaunt == null) return NotFound("User by id not found");

            AccountModel newAccount = oldAccaunt is CustomerAccountModel
                ? _mapper.Map<CustomerAccountModel>(accountRegister)
                : _mapper.Map<AccountModel>(accountRegister);
            newAccount.Id = id;

            await _userService.Update(newAccount);
            return Ok();
        }

        [HttpDelete("id:int")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _userService.Remove(id);
            if (!result) return NotFound("User by id not found");
            return Ok();
        }
    }
}
