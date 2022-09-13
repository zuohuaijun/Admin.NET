namespace Admin.NET.Core;

/// <summary>
/// 服务器信息
/// </summary>
public class ServerUtil
{
    /// <summary>
    /// 服务器基本配置
    /// </summary>
    /// <returns></returns>
    public static dynamic GetServerBaseInfo()
    {
        var furionAssembly = typeof(Furion.App).Assembly.GetName();
        var sqlSugarAssembly = typeof(ISqlSugarClient).Assembly.GetName();
        return new
        {
            HostName = Environment.MachineName, // HostName
            SystemOs = RuntimeInformation.OSDescription, // 系统名称
            OsArchitecture = Environment.OSVersion.Platform.ToString() + " " + RuntimeInformation.OSArchitecture.ToString(), // 系统架构
            ProcessorCount = Environment.ProcessorCount.ToString() + " 核", // CPU核心数
            FrameworkDescription = RuntimeInformation.FrameworkDescription + " / " +
            furionAssembly.Name.ToString() + " " + furionAssembly.Version.ToString() + " / " +
            sqlSugarAssembly.Name.ToString() + " " + sqlSugarAssembly.Version.ToString(), // .NET、Furion、SqlSugar
        };
    }

    /// <summary>
    /// 服务器使用资源
    /// </summary>
    /// <returns></returns>
    public static dynamic GetServerUseInfo()
    {
        var ramInfo = GetRamInfo();
        return new
        {
            TotalRam = Math.Ceiling(ramInfo.Total / 1024).ToString() + " GB", // 总内存
            RamRate = Math.Ceiling(100 * ramInfo.Used / ramInfo.Total) + " %", // 内存使用率
            CpuRate = Math.Ceiling(double.Parse(GetCpuRate())) + " %", // Cpu使用率
            RunTime = GetRunTime()
        };
    }

    /// <summary>
    /// 服务器网络信息
    /// </summary>
    /// <returns></returns>
    public static async Task<dynamic> GetServerNetWorkInfo()
    {
        return new
        {
            WanIp = await GetWanIpFromPCOnline(), // 外网IP
            LocalIp = App.HttpContext?.Connection?.LocalIpAddress.ToString(),
            SendAndReceived = "", //"上行" + Math.Round(networkInfo.SendLength / 1024.0 / 1024 / 1024, 2) + "GB 下行" + Math.Round(networkInfo.ReceivedLength / 1024.0 / 1024 / 1024, 2) + "GB", // 上下行流量统计
            NetworkSpeed = "" //"上行" + Send / 1024 + "kb/s 下行" + Received / 1024 + "kb/s" // 网络速度
        };
    }

    /// <summary>
    /// 内存信息
    /// </summary>
    /// <returns></returns>
    private static dynamic GetRamInfo()
    {
        if (IsUnix())
        {
            var output = ShellUtil.Bash("free -m");
            var lines = output.Split("\n");
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return new
            {
                Total = double.Parse(memory[1]),
                Used = double.Parse(memory[2]),
                Free = double.Parse(memory[3])
            };
        }
        else
        {
            var output = ShellUtil.Cmd("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");
            var lines = output.Trim().Split("\n");
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 2);
            var free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 2);
            return new
            {
                Total = total,
                Free = free,
                Used = total - free
            };
        }
    }

    /// <summary>
    /// CPU信息
    /// </summary>
    /// <returns></returns>
    private static string GetCpuRate()
    {
        string cpuRate;
        if (IsUnix())
        {
            var output = ShellUtil.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'");
            cpuRate = output.Trim();
        }
        else
        {
            var output = ShellUtil.Cmd("wmic", "cpu get LoadPercentage");
            cpuRate = output.Replace("LoadPercentage", string.Empty).Trim();
        }
        return cpuRate;
    }

    /// <summary>
    /// 系统运行时间
    /// </summary>
    /// <returns></returns>
    private static string GetRunTime()
    {
        return FormatTime((long)(DateTimeOffset.Now - Process.GetCurrentProcess().StartTime).TotalMilliseconds);
        //return DateTimeUtil.FormatTime(Environment.TickCount);
    }

    /// <summary>
    /// 是否Linux
    /// </summary>
    /// <returns></returns>
    private static bool IsUnix()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }

    /// <summary>
    /// 毫秒转天时分秒
    /// </summary>
    /// <param name="ms"></param>
    /// <returns></returns>
    private static string FormatTime(long ms)
    {
        int ss = 1000;
        int mi = ss * 60;
        int hh = mi * 60;
        int dd = hh * 24;

        long day = ms / dd;
        long hour = (ms - day * dd) / hh;
        long minute = (ms - day * dd - hour * hh) / mi;
        long second = (ms - day * dd - hour * hh - minute * mi) / ss;
        //long milliSecond = ms - day * dd - hour * hh - minute * mi - second * ss;

        string sDay = day < 10 ? "0" + day : "" + day; //天
        string sHour = hour < 10 ? "0" + hour : "" + hour;//小时
        string sMinute = minute < 10 ? "0" + minute : "" + minute;//分钟
        string sSecond = second < 10 ? "0" + second : "" + second;//秒
        //string sMilliSecond = milliSecond < 10 ? "0" + milliSecond : "" + milliSecond;//毫秒
        //sMilliSecond = milliSecond < 100 ? "0" + sMilliSecond : "" + sMilliSecond;
        return string.Format("{0} 天 {1} 小时 {2} 分 {3} 秒", sDay, sHour, sMinute, sSecond);
    }

    /// <summary>
    /// 获取外网IP和位置
    /// </summary>
    /// <returns></returns>
    private static async Task<string> GetWanIpFromPCOnline()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var url = "http://whois.pconline.com.cn/ipJson.jsp";
        var stream = await url.GetAsStreamAsync();
        var streamReader = new StreamReader(stream.Stream, stream.Encoding);
        var html = streamReader.ReadToEnd();
        var tmp = html[(html.IndexOf("({") + 2)..].Split(",");
        var ipAddr = tmp[0].Split(":")[1] + "【" + tmp[7].Split(":")[1] + "】";
        return ipAddr.Replace("\"", "");
    }
}