using System.Security.Cryptography;
using System.Text;

namespace api.Services.Extensions
{
    public static class StringTransformExtensions
    {
        public static string ToMD5Hash(this string text)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] input = Encoding.ASCII.GetBytes(text);
                byte[] hashBytes = md5.ComputeHash(input);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
