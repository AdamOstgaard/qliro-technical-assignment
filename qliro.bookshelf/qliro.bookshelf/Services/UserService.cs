#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using qliro.bookshelf.Data;
using qliro.bookshelf.Models;

namespace qliro.bookshelf.Services
{
    public class UserService
    {
        private readonly ApplicationContext _context;

        public UserService(ApplicationContext context)
        {
            _context = context;
        }

        public User? Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return null;
            }

            return VerifyPassword(password, user.PasswordHash, user.PasswordSalt) ? user : null;
        }

        private static bool VerifyPassword(string password, IReadOnlyList<byte> storedHash, byte[] storedSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return !computedHash.Where((t, i) => t != storedHash[i]).Any();
        }

        public User Create(User user, string password)
        {
            if (_context.Users.Any(u => u.UserName == user.UserName))
            {
                throw new InvalidOperationException("User name already exists.");
            }

            // this should use a salt but is left out for simplicity.
            using var hmac = new System.Security.Cryptography.HMACSHA512(); 

            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            user.PasswordHash = hash;
            user.PasswordSalt = hmac.Key;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }
    }
}
