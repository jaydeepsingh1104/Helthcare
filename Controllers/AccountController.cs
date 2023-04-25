using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPICore.Dtos;
using WebAPICore.Errors;
using WebAPICore.Interfaces;
using WebAPICore.Model;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using WebAPICore.Data;

namespace WebAPICore.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork iuow;
        private readonly IMapper mapper;
        private readonly IRefreshTokenGeneratorRepository refreshToken;
        private readonly ApplicationDbContext contetx;

        public AccountController(IUnitOfWork iuow, IMapper mapper,IRefreshTokenGeneratorRepository _refreshToken,ApplicationDbContext contetx)
        {
            this.mapper = mapper;
           this.refreshToken = _refreshToken;
            this.contetx = contetx;
            this.iuow = iuow;

        }
        [Authorize(Roles = "admin")]
        [HttpGet("Users")]
        public async Task<IActionResult> GetUserslist()
        {

            var user = await iuow.UserRepositry.GetUserslist();
            var registrationDto = mapper.Map<IEnumerable<RegistrationDto>>(user);
            return Ok(registrationDto);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginReqDto loginReq)
        {
            var user = await iuow.UserRepositry.Authenticate(loginReq.Username, loginReq.Password);

            ApiError apiError = new ApiError();

            if (user == null)
            {
                apiError.ErrorCode = Unauthorized().StatusCode;
                apiError.ErrorMessage = "Invalid user name or password";
                apiError.ErrorDetails = "This error appear when provided user id or password does not exists";
                return Unauthorized(apiError);
            }

            var loginRes = new LoginResDto();
            loginRes.UserName = user.Username;
            loginRes.isactive = user.isactive;
            loginRes.Token = CreateJWT(user);
            loginRes.RefreshToken=refreshToken.GenerateToken(loginReq.Username);
            return Ok(loginRes);
        }
        private string CreateJWT(User user)
        {
            var key = Encoding.ASCII.GetBytes("superSecretKey@345");

            var claims = new Claim[] {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.NameIdentifier,user.email.ToString()),
                 new Claim(ClaimTypes.Role,user.role.ToString())
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(400),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationDto loginReq)
        {
            ApiError apiError = new ApiError();

            if (loginReq.Username == null || loginReq.Password == null)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "User name or password can not be blank";
                return BadRequest(apiError);
            }

            if (await iuow.UserRepositry.UserAlreadyExists(loginReq.Username))
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "User already exists, please try different user name";
                return BadRequest(apiError);
            }

            iuow.UserRepositry.Register(loginReq);
            await iuow.SaveAsync();
            return StatusCode(201);
        }
        [AllowAnonymous]
        [HttpGet("findUserbyuserName/{Username}")]
        public async Task<IActionResult> findUserByUsername(string Username)
        {
            var user = await iuow.UserRepositry.FindUser(Username);
            var registrationDto = mapper.Map<RegistrationDto>(user);
            return Ok(registrationDto);
        }
        [AllowAnonymous]
        [HttpPut("updateuser/{Username}")]
        public async Task<IActionResult> UpdatePatient(string Username, RegistrationDto loginReqdto)
        {
            if (Username != loginReqdto.Username)
                return BadRequest("Update not allowed");

            var userFromDb = await iuow.UserRepositry.FindUser(Username);

            if (userFromDb == null)
            {
                return BadRequest("Update not allowed");
            }
            userFromDb.role = loginReqdto.role;
            userFromDb.isactive = loginReqdto.isactive;

            await iuow.SaveAsync();
            return StatusCode(200);
        }
        [HttpDelete("deleteuser/{Username}")]
        public async Task<IActionResult> Delete(string Username)
        {
            iuow.UserRepositry.DeleteUser(Username);
            await iuow.SaveAsync();
            return Ok(Username);
        }
          [NonAction]
        public LoginResDto Authenticate(string username,Claim[] claims)
        {
            LoginResDto tokenResponse = new LoginResDto();
            var tokenkey = Encoding.ASCII.GetBytes("superSecretKey@345");
            var tokenhandler = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                 signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)

                );
            tokenResponse.Token = new JwtSecurityTokenHandler().WriteToken(tokenhandler);
            tokenResponse.RefreshToken = refreshToken.GenerateToken(username);

            return tokenResponse;
        }
[AllowAnonymous]
        [Route("Refresh")]
        [HttpPost]
        public async Task<IActionResult>  Refresh( LoginResDto token)
        {
           
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token.Token);
            var username = securityToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
          
           // var username = principal.Identity.Name;
            var _reftable =contetx.RefreshtokenTables.FirstOrDefault(o => o.UserId == username && o.RefreshToken == token.RefreshToken);
            if (_reftable == null)
            {
                return Unauthorized();
            }
            LoginResDto _result = Authenticate(username, securityToken.Claims.ToArray());
        //    return Ok(_result);
           return Ok(_result);
        }

    }

}