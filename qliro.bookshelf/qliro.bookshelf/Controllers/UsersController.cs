using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Qliro.BookShelf.Dtos;
using Qliro.BookShelf.Models;
using Qliro.BookShelf.Services;

namespace Qliro.BookShelf.Controllers
{
    [Route("/Users/")]
    public class UsersController : Controller
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {
            var user = _userService.Authenticate(userDto.UserName, userDto.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim(ClaimTypes.Name, user.ID)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                // NOTE: The key is null, iat should be set from app config but is removed here for the sake of simplicity.
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("a4b6b324-001d-4bb3-b328-f3fd85de656e")), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody]UserDto userDto)
        {
            // map dto to entity
            var user = new User{Name = userDto.UserName, UserName = userDto.UserName};

            try
            {
                _userService.Create(user, userDto.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

    }
}
