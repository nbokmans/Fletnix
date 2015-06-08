using FletnixDatabase.Models;
using Postal;

namespace Fletnix.Models.ViewModels
{
    public class VerificationMail: Email
    {
        public User User { get; set; }
        public string VerificationUrl { get; set; }
    }
}