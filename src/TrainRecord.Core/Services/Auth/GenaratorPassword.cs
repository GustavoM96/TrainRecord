using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using TrainRecord.Core.Entities;

namespace TrainRecord.Core.Services.Auth
{
    public class GenaratorHash : IGenaratorHash
    {
        public string Generate(User user, string password)
        {
            var hasher = new PasswordHasher<User>();
            return hasher.HashPassword(user, password);
        }

        public PasswordVerificationResult VerifyHashedPassword(
            User user,
            string password,
            string hashedPassword
        )
        {
            var hasher = new PasswordHasher<User>();
            return hasher.VerifyHashedPassword(user, hashedPassword, password);
        }
    }
}
