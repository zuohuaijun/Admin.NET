using Furion;
using Furion.DependencyInjection;
using Furion.JsonSerialization;
using Furion.Snowflake;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Dilon.Core
{
    /// <summary>
    /// 点选验证码
    /// </summary>
    public class ClickWordCaptcha : IClickWordCaptcha, ITransient
    {
        private readonly IMemoryCache _memoryCache;

        public ClickWordCaptcha(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="code"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public ClickWordCaptchaResult CreateCaptchaImage(string code, int width, int height)
        {
            var rtnResult = new ClickWordCaptchaResult();

            // 变化点: 3个字
            int rightCodeLength = 3;

            Bitmap bitmap = null;
            Graphics g = null;
            MemoryStream ms = null;
            Random random = new Random();

            Color[] colorArray = { Color.Black, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };

            string bgImagesDir = Path.Combine(App.WebHostEnvironment.WebRootPath, "Captcha/Image");
            string[] bgImagesFiles = Directory.GetFiles(bgImagesDir);
            //if (bgImagesFiles == null || bgImagesFiles.Length == 0)
            //    throw Oops.Oh("背景图片文件丢失");

            // 字体来自：https://www.zcool.com.cn/special/zcoolfonts/
            string fontsDir = Path.Combine(App.WebHostEnvironment.WebRootPath, "Captcha/Font");
            string[] fontFiles = new DirectoryInfo(fontsDir)?.GetFiles()
                ?.Where(m => m.Extension.ToLower() == ".ttf")
                ?.Select(m => m.FullName).ToArray();
            //if (fontFiles == null || fontFiles.Length == 0)
            //    throw Oops.Oh("字体文件丢失");

            int imgIndex = random.Next(bgImagesFiles.Length);
            string randomImgFile = bgImagesFiles[imgIndex];
            var imageStream = Image.FromFile(randomImgFile);

            bitmap = new Bitmap(imageStream, width, height);
            imageStream.Dispose();
            g = Graphics.FromImage(bitmap);
            Color[] penColor = { Color.Red, Color.Green, Color.Blue };
            int code_length = code.Length;
            List<string> words = new List<string>();
            for (int i = 0; i < code_length; i++)
            {
                int colorIndex = random.Next(colorArray.Length);
                int fontIndex = random.Next(fontFiles.Length);
                Font f = LoadFont(fontFiles[fontIndex], 15, FontStyle.Regular);
                Brush b = new SolidBrush(colorArray[colorIndex]);
                int _y = random.Next(height);
                if (_y > (height - 30))
                    _y = _y - 60;

                int _x = width / (i + 1);
                if ((width - _x) < 50)
                {
                    _x = width - 60;
                }
                string word = code.Substring(i, 1);
                if (rtnResult.repData.point.Count < rightCodeLength)
                {
                    // (int, int) percentPos = ToPercentPos((width, height), (_x, _y));
                    // 添加正确答案 位置数据
                    rtnResult.repData.point.Add(new PointPosModel()
                    {
                        X = _x, //percentPos.Item1,
                        Y = _y  //percentPos.Item2,
                    });
                    words.Add(word);
                }
                g.DrawString(word, f, b, _x, _y);
            }
            rtnResult.repData.wordList = words;

            ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);
            g.Dispose();
            bitmap.Dispose();
            ms.Dispose();
            rtnResult.repData.originalImageBase64 = Convert.ToBase64String(ms.GetBuffer()); //"data:image/jpg;base64," +
            rtnResult.repData.token = IDGenerator.NextId().ToString();

            // 缓存验证码正确位置集合
            var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30));
            _memoryCache.Set(CommonConst.CACHE_KEY_CODE + rtnResult.repData.token, rtnResult.repData.point, cacheOptions);

            rtnResult.repData.point = null; // 清空位置信息
            return rtnResult;
        }

        /// <summary>
        /// 转换为相对于图片的百分比单位
        /// </summary>
        /// <param name="widthAndHeight">图片宽高</param>
        /// <param name="xAndy">相对于图片的绝对尺寸</param>
        /// <returns>(int:xPercent, int:yPercent)</returns>
        private (int, int) ToPercentPos((int, int) widthAndHeight, (int, int) xAndy)
        {
            (int, int) rtnResult = (0, 0);
            // 注意: int / int = int (小数部分会被截断)
            rtnResult.Item1 = (int)(((double)xAndy.Item1) / ((double)widthAndHeight.Item1) * 100);
            rtnResult.Item2 = (int)(((double)xAndy.Item2) / ((double)widthAndHeight.Item2) * 100);

            return rtnResult;
        }

        /// <summary>
        /// 加载字体
        /// </summary>
        /// <param name="path">字体文件路径,包含字体文件名和后缀名</param>
        /// <param name="size">大小</param>
        /// <param name="fontStyle">字形(常规/粗体/斜体/粗斜体)</param>
        private Font LoadFont(string path, int size, FontStyle fontStyle)
        {
            var pfc = new System.Drawing.Text.PrivateFontCollection();
            pfc.AddFontFile(path);// 字体文件路径
            Font myFont = new Font(pfc.Families[0], size, fontStyle);
            return myFont;
        }

        /// <summary>
        /// 随机绘制字符串
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string RandomCode(int number)
        {
            var str = "左怀军天地玄黄宇宙洪荒日月盈昃辰宿列张寒来暑往秋收冬藏闰余成岁律吕调阳云腾致雨露结为霜金生丽水玉出昆冈剑号巨阙珠称夜光果珍李柰菜重芥姜海咸河淡鳞潜羽翔龙师火帝鸟官人皇始制文字乃服衣裳推位让国有虞陶唐吊民伐罪周发殷汤坐朝问道垂拱平章爱育黎首臣伏戎羌遐迩体率宾归王";
            char[] str_char_arrary = str.ToArray();
            Random rand = new Random();
            HashSet<string> hs = new HashSet<string>();
            bool randomBool = true;
            while (randomBool)
            {
                if (hs.Count == number)
                    break;
                int rand_number = rand.Next(str_char_arrary.Length);
                hs.Add(str_char_arrary[rand_number].ToString());
            }
            return string.Join("", hs);
        }

        /// <summary>
        /// 验证码验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public dynamic CheckCode(ClickWordCaptchaInput input)
        {
            var res = new ClickWordCaptchaResult();

            var rightVCodePos = _memoryCache.Get(CommonConst.CACHE_KEY_CODE + input.Token) as List<PointPosModel>;
            if (rightVCodePos == null)
            {
                res.repCode = "6110";
                res.repMsg = "验证码已失效，请重新获取";
                return res;
            }

            var userVCodePos = JSON.GetJsonSerializer().Deserialize<List<PointPosModel>>(input.PointJson);
            if (userVCodePos == null || userVCodePos.Count < rightVCodePos.Count)
            {
                res.repCode = "6111";
                res.repMsg = "验证码无效";
                return res;
            }

            int allowOffset = 20; // 允许的偏移量(点触容错)
            for (int i = 0; i < userVCodePos.Count; i++)
            {
                var xOffset = userVCodePos[i].X - rightVCodePos[i].X;
                var yOffset = userVCodePos[i].Y - rightVCodePos[i].Y;
                xOffset = Math.Abs(xOffset); // x轴偏移量                
                yOffset = Math.Abs(yOffset); // y轴偏移量
                // 只要有一个点的任意一个轴偏移量大于allowOffset，则验证不通过
                if (xOffset > allowOffset || yOffset > allowOffset)
                {
                    res.repCode = "6112";
                    res.repMsg = "验证码错误";
                    return res;
                }
            }

            _memoryCache.Remove(CommonConst.CACHE_KEY_CODE + input.Token);
            res.repCode = "0000";
            res.repMsg = "验证成功";
            return res;
        }
    }

    /// <summary>
    /// 记录正确位置
    /// </summary>
    public class PointPosModel
    {
        public int X { get; set; }

        public int Y { get; set; }
    }
}