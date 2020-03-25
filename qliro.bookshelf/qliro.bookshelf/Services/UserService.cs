#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Qliro.BookShelf.Data;
using Qliro.BookShelf.Models;

namespace Qliro.BookShelf.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User? Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == username);

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
            if (_dbContext.Users.Any(u => u.UserName == user.UserName))
            {
                throw new InvalidOperationException("User name already exists.");
            }

            // this should use a salt but is left out for simplicity.
            using var hmac = new System.Security.Cryptography.HMACSHA512(); 

            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            user.PasswordHash = hash;
            user.PasswordSalt = hmac.Key;

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user;
        }
    }
}
