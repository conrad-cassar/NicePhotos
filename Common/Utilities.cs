using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Data.Objects;
using System.IO;

namespace Common
{
    public class EarningsView
    {
        public string ImageUrl { get; set; }
        public int EarnedCredits { get; set; }
    }

    public class ReportNotificationData
    {
        public string Username { get; set; }
        public string EmailAddress  { get; set; }
        public string ImageTitle { get; set; }
    }

    public static class Utilities
    {
        public static Dictionary<string, byte[]> ValidImageHeaders
        {
            get 
            {
                Dictionary<string, byte[]> vih = new Dictionary<string, byte[]>();
                
                vih.Add("JPG", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 });
                vih.Add("JPEG", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 });
                vih.Add("PNG", new byte[] { 0x89, 0x50, 0x4E, 0x47 });
                vih.Add("TIF", new byte[] { 0x49, 0x49, 0x2A, 0x00 });
                vih.Add("TIFF", new byte[] { 0x49, 0x49, 0x2A, 0x00 });
                vih.Add("GIF", new byte[] { 0x47, 0x49, 0x46, 0x38 });
                vih.Add("BMP", new byte[] { 0x42, 0x4D });
                vih.Add("ICO", new byte[] { 0x00, 0x00, 0x01, 0x00 });
                return vih; 
            }
        }

        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }

        public static string HashPassword(string password, string username){
            SHA256 sha256 = SHA256.Create();

            StringBuilder sb = new StringBuilder(password);
            sb.Append(username);

            byte[] data = GetBytesFromString(sb.ToString());
            byte[] result;
            SHA256 shaM = new SHA256Managed();
            result = shaM.ComputeHash(data);

            return GetStringFromBytes(result);
        }

        public static byte[] GetBytesFromString(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetStringFromBytes(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length / 2;
            byte[] bytes = new byte[NumberChars];
            StringReader sr = new StringReader(hex);
            for (int i = 0; i < NumberChars; i++)
                bytes[i] = Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
            sr.Dispose();
            return bytes;
        }

        public static Hashtable DecryptParameters(string encParams)
        {
            string[] arr = new NicePhotosEntities().DecryptString(encParams).ToArray();

            if (arr == null)
                return null;

            if (arr.Count() < 1)
                return null;

            if (arr[0] == null)
                return null;

            string chunk = arr[0];

            string[] fullParams = chunk.Split('&');
            Hashtable res = new Hashtable();

            for (int i = 0; i < fullParams.Count(); i++)
            {
                string[] tmp = fullParams[i].Split('=');
                res.Add(tmp[0], tmp[1]);
            }

            return res;
        }

        public static string EncryptTextForParameters(string decParams)
        {
            string[] or = new NicePhotosEntities().EncryptString(decParams).ToArray();

            if (or == null)
                return null;

            if (or.Count() < 1)
                return null;

            return or[0];
        }

        public static byte[] HypridEncrypt(byte[] unencryptedBytes, string publicKey)
        {
            try
            {
                //assymetric algo instance
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                
                //get assymetric public key
                RSA.FromXmlString(publicKey);

                //symetric algo instance
                RijndaelManaged rm = new RijndaelManaged();

                //get symetric key
                rm.GenerateKey();

                //get symetric IV
                rm.GenerateIV();

                //encrypt symetric key
                byte[] encryptedKey = RSA.Encrypt(rm.Key, false);

                //encrypt symetric IV
                byte[] encryptedIV = RSA.Encrypt(rm.IV, false);

                //encrypt bytes using symmetric
                MemoryStream stream = new MemoryStream();
                ICryptoTransform transform = rm.CreateEncryptor();
                CryptoStream cstream = new CryptoStream(stream, transform, CryptoStreamMode.Write);
                cstream.Write(unencryptedBytes, 0, unencryptedBytes.Length);

                //clean after symetric encryption
                cstream.FlushFinalBlock();
                stream.Flush();
                stream.Close();
                cstream.Close();

                byte[] encryptedBytes = stream.ToArray();

                //finalise
                byte[] finalReturningBytes = new byte[encryptedKey.Length + encryptedIV.Length + encryptedBytes.Length];
                Array.Copy(encryptedKey, 0, finalReturningBytes, 0, encryptedKey.Length);
                Array.Copy(encryptedIV, 0, finalReturningBytes, encryptedKey.Length, encryptedIV.Length);
                Array.Copy(encryptedBytes, 0, finalReturningBytes, encryptedKey.Length + encryptedIV.Length, encryptedBytes.Length);

                return finalReturningBytes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] HybridDecrypt(byte[] encryptedBytes, string privateKey)
        {
            try
            {
                //assymetric algo instace
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                //retrieve private key
                RSA.FromXmlString(privateKey);

                //retrieve encrypted key 
                byte[] encryptedKey = new byte[128];
                Array.Copy(encryptedBytes, 0, encryptedKey, 0, 128);

                //decrypt key
                byte[] decreyptedKey = RSA.Decrypt(encryptedKey, false);

                //retrieve encrpted IV
                byte[] encryptedIV = new byte[128];
                Array.Copy(encryptedBytes, 128, encryptedIV, 0, 128);

                //decrypt IV
                byte[] decryptedIV = RSA.Decrypt(encryptedIV, false);

                //decrypt image
                byte[] encreptedImage = new byte[encryptedBytes.Length - 256];
                Array.Copy(encryptedBytes, 256, encreptedImage, 0, encryptedBytes.Length - 256);

                MemoryStream stream = new MemoryStream();
                RijndaelManaged rm = new RijndaelManaged();
                rm.Key = decreyptedKey;
                rm.IV = decryptedIV;

                CryptoStream cstream = new CryptoStream(stream, rm.CreateDecryptor(), CryptoStreamMode.Write);
                cstream.Write(encreptedImage, 0, encreptedImage.Length);
                cstream.Close();

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] GenerateDigitalSignature(byte[] bytes, string privateKey)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] hashValue = sha1.ComputeHash(bytes);
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(privateKey);
            byte[] signiture = RSA.SignHash(hashValue, "SHA1");
            return signiture;
        }

        public static bool VerifyDigitalSignature(byte[] bytes, byte[] digitalSigniture, string publicKey)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] hashValue = sha1.ComputeHash(bytes);
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(publicKey);
            return RSA.VerifyHash(hashValue, "SHA1", digitalSigniture);
        }
    }
}
