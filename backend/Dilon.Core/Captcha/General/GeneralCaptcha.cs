using Furion.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Dilon.Core
{
    /// <summary>
    /// 常规验证码
    /// </summary>
    public class GeneralCaptcha : IGeneralCaptcha, ITransient
    {
        private readonly IMemoryCache _memoryCache;

        public GeneralCaptcha(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string CreateCaptchaImage(int length = 4)
        {
            return Convert.ToBase64String(Draw(length));
        }

        private static string GenerateRandom(int length)
        {
            var chars = new StringBuilder();
            // 验证码的字符集，去掉了一些容易混淆的字符
            char[] character = { '2', '3', '4', '5', '6', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random rnd = new();
            // 生成验证码字符串
            for (int i = 0; i < length; i++)
            {
                chars.Append(character[rnd.Next(character.Length)]);
            }
            return chars.ToString();
        }

        private byte[] Draw(int length = 4)
        {
            int codeW = 110;
            int codeH = 36;
            int fontSize = 22;

            // 颜色列表，用于验证码、噪线、噪点
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
            // 字体列表，用于验证码
            string[] fonts = new[] { "Times New Roman", "Verdana", "Arial", "Gungsuh", "Impact" };

            var code = GenerateRandom(length); // 随机字符串集合

            using (Bitmap bmp = new Bitmap(codeW, codeH))
            using (Graphics g = Graphics.FromImage(bmp))
            using (MemoryStream ms = new MemoryStream())
            {
                g.Clear(Color.White);
                Random rnd = new Random();
                // 画噪线
                for (int i = 0; i < 1; i++)
                {
                    int x1 = rnd.Next(codeW);
                    int y1 = rnd.Next(codeH);
                    int x2 = rnd.Next(codeW);
                    int y2 = rnd.Next(codeH);
                    var clr = color[rnd.Next(color.Length)];
                    g.DrawLine(new Pen(clr), x1, y1, x2, y2);
                }

                // 画验证码字符串
                string fnt;
                Font ft;
                for (int i = 0; i < code.Length; i++)
                {
                    fnt = fonts[rnd.Next(fonts.Length)];
                    ft = new Font(fnt, fontSize);
                    var clr = color[rnd.Next(color.Length)];
                    g.DrawString(code[i].ToString(), ft, new SolidBrush(clr), (float)i * 24 + 2, (float)0);
                }

                // 缓存验证码正确集合
                var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30));
                _memoryCache.Set(CommonConst.CACHE_KEY_CODE + Guid.NewGuid().ToString("N"), code, cacheOptions);

                // 将验证码图片写入内存流
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 验证码验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public dynamic CheckCode(GeneralCaptchaInput input)
        {
            var res = new ClickWordCaptchaResult();

            var code = _memoryCache.Get(CommonConst.CACHE_KEY_CODE + input.Token);
            if (code == null)
            {
                res.repCode = "6110";
                res.repMsg = "验证码已失效，请重新获取";
                return res;
            }
            if (input.CaptchaCode != (string)code)
            {
                res.repCode = "6112";
                res.repMsg = "验证码错误";
                return res;
            }

            _memoryCache.Remove(CommonConst.CACHE_KEY_CODE + input.Token);
            res.repCode = "0000";
            res.repMsg = "验证成功";
            return res;
        }
    }
}