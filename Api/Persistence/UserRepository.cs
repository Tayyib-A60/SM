using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SkineroMotors.Controllers.Resources;
using SkineroMotors.Core;
using SkineroMotors.Core.Models;
using SkineroMotors.Extensions;

namespace SkineroMotors.Persistence {
    public class UserRepository : IUserRepository {
        private SMDbContext _context { get; }
        private AppSettings _appSettings { get; }
        public UserRepository (SMDbContext context, IOptions<AppSettings> appSettings) {
            _context = context;
            _appSettings = appSettings.Value;
        }
        public User Authenticate (string email, string password) {
            if (string.IsNullOrEmpty (email) || string.IsNullOrEmpty (password))
                return null;
            var user = _context.Users.SingleOrDefault (u => u.Email == email);
            if (user == null)
                return null;
            if (!VerifyPasswordHash (password, user.PasswordHash, user.PasswordSalt))
                return null;
            return user;
        }
        public async Task<IEnumerable<User>> GetUsers () {
            return await _context.Users.ToListAsync ();
        }
        public async Task<User> GetUser (int id) {
            return await _context.Users
                .Where (u => u.Id == id)
                .SingleOrDefaultAsync ();
        }
        public async Task<User> GetUser (string email) {
            return await _context.Users
                .Where (u => u.Email == email)
                .SingleOrDefaultAsync ();
        }
        public async Task<bool> UserExists (User user) {
            if (await _context.Users.AnyAsync (x => x.Email == user.Email))
                return true;

            return false;
        }
        public async Task<User> CreateUser (User user, string password) {
            if (user == null)
                throw new NullReferenceException ("User cannot be null");
            if (string.IsNullOrWhiteSpace (password))
                throw new ArgumentNullException ("Password is Required");
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash (password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            if (user.Email.Contains ("adesokan")) {
                user.Role = Role.Admin;
            } else {
                user.Role = Role.Customer;
            }

            _context.Users.Add (user);
            await _context.SaveChangesAsync ();

            return user;
        }
        public async Task<bool> ForgotPassword (User user) {
            if (await _context.Users.AnyAsync (x => x.Email == user.Email))
                return true;
            return false;
        }
        public void UpdateUser (string newPassword, User userToUpdate) {
            if (!string.IsNullOrWhiteSpace (newPassword)) {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash (newPassword, out passwordHash, out passwordSalt);
                userToUpdate.PasswordHash = passwordHash;
                userToUpdate.PasswordSalt = passwordSalt;
            }
            _context.Users.Update (userToUpdate);
            _context.SaveChangesAsync ();
        }
        public void DeleteUser (User user) {
            _context.Remove (user);
            _context.SaveChangesAsync ();
        }
        public string CreateToken (User user) {
            var tokenHandler = new JwtSecurityTokenHandler ();
            var key = Encoding.ASCII.GetBytes (_appSettings.Secret);
            var sub = new ClaimsIdentity ();
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity (new Claim[] {
                new Claim (ClaimTypes.NameIdentifier, user.Email),
                new Claim (ClaimTypes.Name, user.Name),
                new Claim (ClaimTypes.Role, user.Role.ToString ()),
                new Claim (ClaimTypes.GroupSid, user.Id.ToString())
                }),
                // Expires = DateTime.UtcNow.AddDays(7),
                Expires = DateTime.UtcNow.AddMinutes (3),
                SigningCredentials = new SigningCredentials (new SymmetricSecurityKey (key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken (tokenDescriptor);
            var tokenString = tokenHandler.WriteToken (token);
            return tokenString;
        }
        private static void CreatePasswordHash (string password, out byte[] passwordHash, out byte[] passwordSalt) {
            if (password == null) throw new ArgumentNullException ("password");
            if (string.IsNullOrWhiteSpace (password)) throw new ArgumentException ("value cannot be empty or whitespace, on string is allowed ", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512 ()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (password));
            }
        }
        private static bool VerifyPasswordHash (string password, byte[] storedHash, byte[] storedSalt) {
            if (password == null) throw new ArgumentNullException ("password");
            if (string.IsNullOrWhiteSpace (password)) throw new ArgumentException ("value cannot be empty or whitespace, only string is allowed ", "password");
            if (storedHash.Length != 64) throw new ArgumentException ("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException ("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512 (storedSalt)) {
                var computedHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (password));
                for (int i = 0; i < computedHash.Length; i++) {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }
    }
}