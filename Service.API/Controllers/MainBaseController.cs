using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Service.Common.Interfaces.Service;
using Service.Common.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Service.API.Controllers
{
    public class MainBaseController : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;

        public MainBaseController(IMapper mapper, ILogger logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        protected string? GetCurrentAccountLogin()
        {
            var login = User.FindFirst(u => u.Properties.Values.Contains(JwtRegisteredClaimNames.NameId));
            return login?.Value;
        }

        protected async Task<AccountModel?> GetCurrentAccount()
        {
            var login = GetCurrentAccountLogin();
            if (login == null) return null;

            var services = HttpContext.RequestServices;
            var accountService = services.GetRequiredService<IAccountService>();
            var user = await accountService.GetAccount(login, null);
            if (user == null) return null;

            return user;
        }
    }
}
