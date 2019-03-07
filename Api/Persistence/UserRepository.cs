using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkineroMotors.Core;
using SkineroMotors.Core.Models;

namespace SkineroMotors.Persistence {
    public class UserRepository : IUserRepository {
        private SMDbContext _context { get; }
        public UserRepository (SMDbContext context) {
            _context = context;
        }
        public  User Authenticate(string email, string password)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;
            var user = _context.Users.SingleOrDefault(u => u.Email == email);
            if (user == null)
                return null;
            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            return user;
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetUser(int id)
        {
            return await _context.Users
                    .Where(u => u.Id == id)
                    .SingleOrDefaultAsync();
        }
        public async Task<User> CreateUser(User user, string password)
        {
            if(string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password is Required");
            if(_context.Users.Any(u => u.Email == user.Email))
                throw new Exception("Email " + user.Email + " is already taken");
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public void UpdateUser(User user, string password = null)
        {
            var userToUpdate = _context.Users.Find(user.Id);
            if (userToUpdate == null) throw new ArgumentNullException("User not found");

            if (user.Email != userToUpdate.Email)
            {
                if (_context.Users.Any(u => u.Email == user.Email))
                    throw new Exception("Username " + user.Email + " is already taken");
            }
            userToUpdate.Name = user.Name;
            userToUpdate.Email = user.Email;
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);
                userToUpdate.PasswordHash = passwordHash;
                userToUpdate.PasswordSalt = passwordSalt;
            }
            _context.Users.Update(user);
            _context.SaveChangesAsync();
        }
        public void DeleteUser(User user)
        {
            // var user = _context.Users
            //             .Where(u => u.Id == id)
            //             .SingleOrDefaultAsync();
            // if(user != null) {
                _context.Remove(user);
                _context.SaveChangesAsync();
            // }
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if(password == null) throw new ArgumentNullException("password");
            if(string.IsNullOrWhiteSpace(password)) throw new ArgumentException("value cannot be empty or whitespace, on string is allowed ", "password");
           
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); 
            }
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if(password == null) throw new ArgumentNullException("password");
            if(string.IsNullOrWhiteSpace(password)) throw new ArgumentException("value cannot be empty or whitespace, on string is allowed ", "password");
            if(storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if(storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using(var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++) {
                    if (computedHash[i] != storedHash[i])   return false;
                }
            }
            return true;
        }
   }
}