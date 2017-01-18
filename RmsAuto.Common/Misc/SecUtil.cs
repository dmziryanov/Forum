using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;

namespace RmsAuto.Common.Misc
{
    public static class SecUtil
    {
        public static byte[] ToSecureByteArray(object obj, string password)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (password == null)
                throw new ArgumentNullException("password");
            

            using (var stream = new MemoryStream())
            using (var encStream = CreateEncrypting(stream, string.Empty))
            {
                new BinaryFormatter().Serialize(encStream, obj);
                encStream.FlushFinalBlock();
                return stream.GetBuffer();
            }
        }

        public static object FromSecureByteArray(byte[] bytes, string password)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");
            if (password == null)
                throw new ArgumentNullException("password");

            using (var stream = new MemoryStream(bytes))
            using (var decStream = CreateDecrypting(stream, string.Empty))
            {
                return new BinaryFormatter().Deserialize(decStream);
            }
        }
        
        private static CryptoStream CreateEncrypting(Stream stream, string password)
        {
            return new CryptoStream(
                stream,
                new RijndaelManaged() 
                .CreateEncryptor(new PasswordDeriveBytes(password, null).GetBytes(16), new byte[16]),
                CryptoStreamMode.Write);
        }

        private static CryptoStream CreateDecrypting(Stream stream, string password)
        {
            return new CryptoStream(
                 stream,
                 new RijndaelManaged() { Padding = PaddingMode.None }
                 .CreateDecryptor(new PasswordDeriveBytes(password, null).GetBytes(16), new byte[16]),
                 CryptoStreamMode.Read);
        }
    }
}
