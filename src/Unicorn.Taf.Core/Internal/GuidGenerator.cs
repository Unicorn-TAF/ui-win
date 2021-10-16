using System;
using System.Security.Cryptography;
using System.Text;

namespace Unicorn.Taf.Core.Internal
{
    internal static class GuidGenerator
    {
        /// <summary>
        /// Generates Id for the test which will be the same each time for this test
        /// </summary>
        /// <param name="data">string value</param>
        /// <returns>unique test method <see cref="Guid"/></returns>
        internal static Guid FromString(string data)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                return new Guid(md5.ComputeHash(Encoding.Unicode.GetBytes(data)));
            }
        }
    }
}
