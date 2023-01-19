using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Identity;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Core.Services.Auth
{
    public class GenaratorHash : IGenaratorHash
    {
        public string Generate(User user)
        {
            var hasher = new PasswordHasher<User>();
            return hasher.HashPassword(user, user.Password);
        }

        public User SetUserWithRehashedPassword(User user)
        {
            var reHashedPassword = Generate(user);
            return (user, reHashedPassword).Adapt<User>();
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
