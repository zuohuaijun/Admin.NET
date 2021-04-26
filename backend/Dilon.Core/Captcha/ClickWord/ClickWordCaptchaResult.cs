using Furion.DependencyInjection;
using System.Collections.Generic;

namespace Dilon.Core
{
    /// <summary>
    /// 验证码输出参数
    /// </summary>
    [SkipScan]
    public class ClickWordCaptchaResult
    {
        public string repCode { get; set; } = "0000";
        public string repMsg { get; set; }
        public RepData repData { get; set; } = new RepData();
        public bool error { get; set; }
        public bool success { get; set; } = true;
    }

    [SkipScan]
    public class RepData
    {
        public string captchaId { get; set; }
        public string projectCode { get; set; }
        public string captchaType { get; set; }
        public string captchaOriginalPath { get; set; }
        public string captchaFontType { get; set; }
        public string captchaFontSize { get; set; }
        public string secretKey { get; set; }
        public string originalImageBase64 { get; set; }
        public List<PointPosModel> point { get; set; } = new List<PointPosModel>();
        public string jigsawImageBase64 { get; set; }
        public List<string> wordList { get; set; } = new List<string>();
        public string pointList { get; set; }
        public string pointJson { get; set; }
        public string token { get; set; }
        public bool result { get; set; }
        public string captchaVerification { get; set; }
    }

    [SkipScan]
    public class WordList
    {
    }
}