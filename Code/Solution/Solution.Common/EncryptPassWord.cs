﻿using System;
using System.Security.Cryptography;
using System.Text;
namespace Solution.Common
{
    public class EncryptPassWord
    {
        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        public static string CreateSalt()
        {
            byte[] data = new byte[8];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="pwdString"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string EncryptPwd(string pwdString, string salt)
        {
            if (salt == null || salt == "")
            {
                return pwdString;
            }
            byte[] bytes = Encoding.Unicode.GetBytes(salt.ToLower().Trim() + pwdString.Trim());
            return BitConverter.ToString(((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(bytes));
        }
    }
}
