using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.VCode
{
    public class VCodeHelper
    {
        /// <summary>
        /// 随机生成指定长度的验证码字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string RandomCode(int length)
        {
            string s = "0123456789zxcvbnmasdfghjklqwertyuiop";
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            int index;
            for (int i = 0; i < length; i++)
            {
                index = rand.Next(0, s.Length);
                sb.Append(s[index]);
            }
            return sb.ToString();
        }

        private void PaintInterLine(Graphics g, int num, int width, int height)
        {
            Random r = new Random();
            int startX, startY, endX, endY;
            for (int i = 0; i < num; i++)
            {
                startX = r.Next(0, width);
                startY = r.Next(0, height);
                endX = r.Next(0, width);
                endY = r.Next(0, height);
                g.DrawLine(new Pen(Brushes.Red), startX, startY, endX, endY);
            }
        }


        public byte[] CreatVCodeImgs(string vcode)
        {
            byte[] data = null;
            string code = vcode;
            //定义一个画板
            MemoryStream ms = new MemoryStream();
            using (Bitmap map = new Bitmap(100, 40))
            {
                Random r = new Random();
                string[] fontString = new string[] { "黑体", "幼圆", "楷体", "华文仿宋" };
                Color[] colorArray = new Color[] { Color.Blue, Color.Black, Color.Yellow, Color.Green };
                //画笔,在指定画板画板上画图
                //g.Dispose();
                using (Graphics g = Graphics.FromImage(map))
                {
                    g.Clear(Color.White);
                    g.DrawString(code, new Font(fontString[r.Next(fontString.Length)], 24.0F), Brushes.Blue, new Point(10, 8));
                    //绘制干扰线
                    PaintInterLine(g, 10, map.Width, map.Height);

                    //绘制干扰点
                    for (int i = 0; i < 100; i++)
                    {
                        map.SetPixel(r.Next(map.Width), r.Next(map.Height), colorArray[i % 4]);
                    }
                }
                map.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            data = ms.GetBuffer();
            return data;
        }


    }
}
