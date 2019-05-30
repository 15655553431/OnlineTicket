using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Md5
{
    public static class Md5Helper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="txt">明文</param>
        /// <returns>密文</returns>
        public static string GetMd5(this string txt)
        {
            //创建md5对象
            MD5 md5 = MD5.Create();

            //将字符串转成字节数组
            byte[] bs = Encoding.UTF8.GetBytes(txt);

            //加密
            byte[] bs2 = md5.ComputeHash(bs);

            //将字节数组转换成字符串
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bs2.Length; i++)
            {
                sb.Append(bs2[i].ToString("x2").ToLower());
            }
            return sb.ToString();
        }
    }
}
