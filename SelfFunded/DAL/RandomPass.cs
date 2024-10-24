using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using System;
using System.Text;
namespace SelfFunded.DAL
{
    public class RandomPass
    {
 

        public  string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;:,.<>?";
            Random random = new Random();
            StringBuilder password = new StringBuilder();

            // Generate random characters
            for (int i = 0; i < 8; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }

            return password.ToString();
        }
    }
}
