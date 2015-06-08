using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FletnixDatabase.Models;
using SimpleCrypto;

namespace Fletnix.Models.Authentication
{
    public static class UserAuthenticationService
    {
        public static bool UserIsAuthorized(User user, string password)
        {
            return ValidatePassword(user, password); 
        }
        
        private static Boolean ValidatePassword(User user, string inputPassword)
        {
            var cryptoService = new PBKDF2();
            var inputPasswordHashed = cryptoService.Compute(inputPassword, user.PasswordSalt);
            return cryptoService.Compare(user.Password, inputPasswordHashed);
        }

        public static void SaltAndHashUserPassword(User user)
        {
            var cryptoService = new PBKDF2();
            user.PasswordSalt = cryptoService.GenerateSalt();
            user.Password = cryptoService.Compute(user.Password, user.PasswordSalt);
        }

    }
}