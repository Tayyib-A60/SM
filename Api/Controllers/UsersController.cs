using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Core.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using SkineroMotors.Controllers.Resources;
using SkineroMotors.Core;
using SkineroMotors.Core.Models;
using SkineroMotors.Extensions;

namespace SkineroMotors.Controllers {

    [ApiController]
    [Authorize]
    [Route ("/api/skineroVehicles/user")]
    public class UsersController : Controller {
        private IMapper _mapper { get; }
        private IUserRepository _repository { get; }
        private AppSettings _appSettings { get; }
        private IUnitOfWork _unitOfWork { get; }
        private Role role { get; }
        private IConfiguration _configuration { get; }
        public UsersController (IMapper mapper, IUserRepository repository, IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings, IConfiguration configuration) {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
            _repository = repository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost ("authenticate")]
        public IActionResult Authenticate ([FromBody] UserResource userResource) {
            var user = _repository.Authenticate (userResource.Email, userResource.Password);

            if (user == null)
                return Unauthorized ();
            var tokenHandler = new JwtSecurityTokenHandler ();
            var key = Encoding.ASCII.GetBytes (_appSettings.Secret);
            var sub = new ClaimsIdentity ();
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity (new Claim[] {
                new Claim (ClaimTypes.Name, user.Id.ToString ()),
                new Claim (ClaimTypes.Role, user.Role.ToString ())
                }),
                // Expires = DateTime.UtcNow.AddDays(7),
                Expires = DateTime.UtcNow.AddMinutes (1),
                SigningCredentials = new SigningCredentials (new SymmetricSecurityKey (key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken (tokenDescriptor);
            var tokenString = tokenHandler.WriteToken (token);
            return Ok (new {
                Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Token = tokenString,
                    Roles = user.Role.ToString ()
            });
        }

        [AllowAnonymous]
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword ([FromBody] UserResource userResource) {
            // if (!await _repository.ForgotPassword (email.ToString()))
            //     return BadRequest ("User does not exist");
            var user = _mapper.Map<User>(userResource);
            var token = _repository.CreateToken (user);

            var apiKey = _configuration.GetSection ("SkineroMotorsSendGridApiKey").Value;
            var sendGridclient = new SendGridClient (apiKey);
            var from = new EmailAddress ("info@skineromotors.com", "SkineroMotors");
            var subject = "Skinero Motors Contact us";
            var to = new EmailAddress (user.Email, user.Name);
            // var buttonName = "Reset Password";
            var plainTextContent1 = $"<strong>http://localhost:4200/user/resetPassword?token={token}</strong>";
            var htmlContent = $"http://localhost:4200/user/resetpassword?token={token}";
            var msg = MailHelper.CreateSingleEmail (from, to, subject, plainTextContent1, htmlContent);
            var response = await sendGridclient.SendEmailAsync (msg);

            return Ok ();
        }

        [AllowAnonymous]
        [HttpPost ("register")]
        public async Task<IActionResult> Register ([FromBody] UserResource userResource) {
            var user = _mapper.Map<User> (userResource);
            if (await _repository.UserExists (user))
                return BadRequest ("User already exists");
            try {
                await _repository.CreateUser (user, userResource.Password);
                return Ok ($"User with email {user.Email} Created");
            } catch (Exception ex) {
                return BadRequest (ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll () {
            var users = await _repository.GetUsers ();
            var usersResource = _mapper.Map<IEnumerable<UserResource>> (users);
            return Ok (usersResource);
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetUser (int id) {
            var user = await _repository.GetUser (id);
            var userResource = _mapper.Map<UserResource> (user);
            return Ok (userResource);
        }
        [HttpGet ("{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser (string email) {
            var user = await _repository.GetUser (email);
            if (user == null)
                return NotFound("Specified user doesn't exist");
            var userResource = _mapper.Map<UserResource> (user);
            return Ok (userResource);
        }

        [HttpPut ("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUser (int id, [FromBody] UserResource userResource) {
            if (userResource == null)
                return BadRequest("User cannot be null");
            var userToUpdate = await _repository.GetUser(id);
            if (userToUpdate == null)
                return NotFound("User does not exist");
             _mapper.Map<UserResource, User> (userResource, userToUpdate);
             _repository.UpdateUser(userResource.Password, userToUpdate);
             return Ok("User update was succesfull");
        }

        [HttpDelete ("{id}")]
        public async Task<IActionResult> Delete (int id) {
            var user = await _repository.GetUser (id);
            _repository.DeleteUser (user);
            return Ok (id);
        }

        [AllowAnonymous]
        [HttpPost ("sendEmail")]
        public async Task<IActionResult> SendMail () {
            var message = new MimeMessage ();
            message.From.Add (new MailboxAddress ("Adesokan Tayyib", "adesokantayyib@gmail.com"));
            message.To.Add (new MailboxAddress ("LABULE", "liadi.omotola@gmail.com"));
            message.Subject = "Sending an email with Dotnet is super cool";
            message.Body = new TextPart ("plain") {
                Text = "This is my very first Dotnet email message"
            };
            using (var client = new SmtpClient ()) {
                client.Connect ("smtp.gmail.com", 587);
                await client.AuthenticateAsync ("adesokantayyib@gmail.com", "Adenike1913");
                await client.SendAsync (message);
                await client.DisconnectAsync (true);
            }
            return Ok (message);
        }
    }
}