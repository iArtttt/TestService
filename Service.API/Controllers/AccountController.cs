using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.API.Dtos;
using Service.Common.Enums;
using Service.Common.Interfaces.Infrastructure;
using Service.Common.Interfaces.Service;
using Service.Common.Models;

namespace Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : MainBaseController
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        public AccountController(IAccountService accountService, ITokenService tokenService, ILogger<AccountController> logger) 
            : base(null!, logger)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpPost("register/{userType}")]
        public async Task<ActionResult<AccountDto>> Register(string userType, AccountRegisterDto userRegisterDto)
        {
            var isExist = await _accountService.IsAccountExist(userRegisterDto.Login);

            if (isExist)
                return BadRequest("User with this login already exists.");

            AccountModel accountModel;

            if (userType == "customer")
            {
                accountModel = new CustomerAccountModel
                {
                    Login = userRegisterDto.Login,
                    Email = userRegisterDto.Email,
                    Role = RoleType.Customer,
                    Address = userRegisterDto.Address,
                    PhoneNumder = userRegisterDto.PhoneNumber,
                    Name = userRegisterDto.Name,
                    LastName = userRegisterDto.LastName
                };
            }
            else
            {
                accountModel = new AccountModel
                {
                    Login = userRegisterDto.Login,
                    Email = userRegisterDto.Email,
                    Role = userType.ToLower() switch { "admin" => RoleType.Admin, "vendor" => RoleType.Vendor },
                };
            }

            accountModel = await _accountService.AddAccount(accountModel, userRegisterDto.Password);
            var result = new AccountDto(accountModel.Id, accountModel.Login, _tokenService.GetToken(accountModel), accountModel.Role.ToString());
            return result;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AccountDto>> Login(AccountLoginDto userLogin)
        {
            var account = await _accountService.GetAccount(userLogin.Login, userLogin.Password);

            if (account == null) 
                return Unauthorized();

            var result = new AccountDto(account.Id, account.Login, _tokenService.GetToken(account), account.Role.ToString());
            return Ok(result);
        }
    }
}
