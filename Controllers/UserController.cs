using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RKdigitalsAPI.Models;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace RKdigitalsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly RKdigitalsDBContext _db;
        private readonly IConfiguration _config;
        public UserController(IConfiguration config)
        {
            _db = new RKdigitalsDBContext();
            _config = config;
        }
        [HttpGet("Users")]
        public async Task<List<User>> Get()
        {
           return await _db.Users.ToListAsync();
        }
        [HttpGet("Users/{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            if (username != "null")
            {
                var data = await _db.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
                return Ok(new
                {
                    UserId = data.Id,
                    Fullname = data.Fullname,
                    Username = data.Username,
                    Email = data.Email,
                    Mobilenumber = data.Mobilenumber
                });
            }
            else
            {
                return Ok(new
                {
                    Message = "Username Not found"
                }); ;
            }
            
        }
        [HttpPost("register")]
        public async Task<IActionResult> register(registeruser registeruser)
        {
            if (registeruser == null)
                return BadRequest();

            byte[] passwordhash, passwordkey;
            using (var hmac = new HMACSHA512())
            {
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registeruser.Password));
                passwordkey = hmac.Key;
            }
            User u = new User();
            u.Fullname = registeruser.Fullname;
            u.Username = registeruser.Username;
            u.Email = registeruser.Email;
            u.Mobilenumber = registeruser.Mobilenumber;
            u.Passwordhash = passwordhash;
            u.Passwordkey = passwordkey;
            u.Token = "";
            u.Role = "user";
            await _db.Users.AddAsync(u);
            await _db.SaveChangesAsync();
            return Ok(new
            {
                Username = u.Username,
                Passwordhash = u.Passwordhash,
                Passwordkey = u.Passwordkey,
                Message = "Your Successfully Register"
            });
        }
        [HttpPost("login")]
        public async Task<IActionResult> login(loginuser loginuser)
        {

            if (loginuser == null)
                return BadRequest(new
                {
                    Message = "Enter fills"
                });
            var data = await _db.Users.Where(x => x.Username == loginuser.Username).FirstOrDefaultAsync();
            if (data == null)
                return BadRequest(new
                {
                    Message = "Username is not Found"
                });

            if (!matchpassword(loginuser.password, data.Passwordhash, data.Passwordkey) && data.Username == loginuser.Username)
            {
                var token = CreateToken(loginuser.Username);
                    return Ok(new
                {
                    Username = data.Username,
                    Role= data.Role,
                    Token = token,
                    Message = "Login Successfully"
                });
            }

            return BadRequest(new
            {
                Message = "Password incorrect"
            });
        }

        private bool matchpassword(string password, byte[] userpasswordhash, byte[] passwordkey)
        {
            byte[] passwordhash;

            using (var hmac = new HMACSHA512(passwordkey))
            {
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < passwordhash.Length; i++)
                {
                    if (passwordhash[i] == userpasswordhash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        private string CreateToken(string username)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var cliams = new[]
            {
                new Claim("Username",username)
            };
            var token = new JwtSecurityToken(
                issuer:_config["Jwt:Issuer"],
                audience:_config["Jwt:Audience"],
                cliams,
                expires:DateTime.Now.AddDays(1),
                signingCredentials:credential);
            var t= tokenhandler.WriteToken(token);
            return t;
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(User user)
        {
            User u = new User();
            var data = await _db.Users.Where(x => x.Id == user.Id).FirstOrDefaultAsync();
            var newUser = new User
            {
                Id = user.Id,
                Fullname = user.Fullname,
                Username = user.Username,
                Email = user.Email,
                Mobilenumber = user.Mobilenumber,
                Passwordhash = data.Passwordhash,
                Passwordkey = data.Passwordkey,
                Token = data.Token,
                Role = data.Role
            };
            if (u != null)
            {
                 _db.Users.Attach(newUser);
                 _db.Entry(newUser).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Updated Your Changes"
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Not Updated Successfully Your Changes"
                });
            }
        }
        [HttpGet("Roles")]
        public Object Role()
        {
           return _db.Users.Select(o => o.Role).Distinct();
        }
    }

}
