using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Security;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SkineroMotors.Controllers.Resources;
using SkineroMotors.Core;
using SkineroMotors.Extensions;
using System;
using SkineroMotors.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;

namespace SkineroMotors.Controllers {
    [Authorize]
    [ApiController]
    [Route("/api/skineroVehicles/user")]
    public class UsersController : Controller {
        private IMapper _mapper { get; }
        private IUserRepository _repository { get; }
        private AppSettings _appSettings { get; }
        private IUnitOfWork _unitOfWork { get; }
        public UsersController (IMapper mapper, IUserRepository repository, IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
            _repository = repository;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserResource userResource)
        {
            var user = _repository.Authenticate(userResource.Email, userResource.Password);

            if (user == null)
                return Unauthorized();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                // Expires = DateTime.UtcNow.AddDays(7),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok( new {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Token = tokenString
            });
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserResource userResource)
        {
            var user = _mapper.Map<User>(userResource);
            try
            {
                _repository.CreateUser(user, userResource.Password);
                return Ok($"User with email {user.Email} Created");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _repository.GetUsers();
            var usersResource = _mapper.Map<IEnumerable<UserResource>>(users);
            return Ok(usersResource);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repository.GetUser(id);
            var userResource = _mapper.Map<UserResource>(user);
            return Ok(userResource);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserResource userResource)
        {
            var user = _mapper.Map<User>(userResource);
            user.Id = id;
            try
            {
                _repository.UpdateUser(user, userResource.Password);
                return Ok(userResource);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _repository.GetUser(id);
            _repository.DeleteUser(user);
            return Ok(id);
        }
        [AllowAnonymous]
        [HttpPost("sendEmail")]
        public async Task<IActionResult> SendMail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Adesokan Tayyib", "adesokantayyib@gmail.com"));
            message.To.Add(new MailboxAddress("LABULE", "liadi.omotola@gmail.com"));
            message.Subject = "Sending an email with Dotnet is super cool";
            message.Body = new TextPart("plain"){
                Text = "This is my very first Dotnet email message"
            };
            using (var client = new SmtpClient()) {
                client.Connect("smtp.gmail.com", 587);
                await client.AuthenticateAsync("adesokantayyib@gmail.com", "Adenike1913");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            return Ok(message);
        }
    }
}