using Furion.RemoteRequest.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dilon.Core
{
    /// <summary>
    /// 获取服务器信息
    /// </summary>
    public static class MachineUtil
    {
        /// <summary>
        /// 获取资源使用信息
        /// </summary>
        /// <returns></returns>
        public static dynamic GetMachineUseInfo()
        {
            var ramInfo = GetRamInfo();
            return new
            {
                TotalRam = Math.Ceiling(ramInfo.Total / 1024).ToString() + " GB", // 总内存
                RamRate = Math.Ceiling(100 * ramInfo.Used / ramInfo.Total), // 内存使用率
                CpuRate = Math.Ceiling(double.Parse(GetCPURate())), // cpu使用率
                RunTime = GetRunTime()
            };
        }

        /// <summary>
        /// 获取基本参数
        /// </summary>
        /// <returns></returns>
        public static async Task<dynamic> GetMachineBaseInfo()
        {
            var assemblyName = typeof(Furion.App).Assembly.GetName();
            //var networkInfo = NetworkInfo.GetNetworkInfo();
            //var (Received, Send) = networkInfo.GetInternetSpeed(1000);
            return new
            {
                WanIp = await GetWanIpFromPCOnline(), // 外网IP
                SendAndReceived = "",// "上行" + Math.Round(networkInfo.SendLength / 1024.0 / 1024 / 1024, 2) + "GB 下行" + Math.Round(networkInfo.ReceivedLength / 1024.0 / 1024 / 1024, 2) + "GB", // 上下行流量统计
                LanIp = "",//networkInfo.AddressIpv4.ToString(), // 局域网IP
                IpMac = "",//networkInfo.Mac, // Mac地址
                HostName = Environment.MachineName, // HostName
                SystemOs = RuntimeInformation.OSDescription, // 系统名称
                OsArchitecture = Environment.OSVersion.Platform.ToString() + " " + RuntimeInformation.OSArchitecture.ToString(), // 系统架构
                ProcessorCount = Environment.ProcessorCount.ToString() + "核", // CPU核心数
                FrameworkDescription = RuntimeInformation.FrameworkDescription + " + " + assemblyName.Name.ToString() + assemblyName.Version.ToString(), // .NET和Furion版本
                NetworkSpeed = ""//"上行" + Send / 1024 + "kb/s 下行" + Received / 1024 + "kb/s" // 网络速度
            };
        }

        /// <summary>
        /// 动态获取网络信息
        /// </summary>
        /// <returns></returns>
        public static dynamic GetMachineNetWorkInfo()
        {
            //var networkInfo = NetworkInfo.GetNetworkInfo();
            //var (Received, Send) = networkInfo.GetInternetSpeed(1000);
            ////int Send, Received;
            ////while (true)
            ////{
            ////    var tmp = networkInfo.GetInternetSpeed(1000);
            ////    if (tmp.Send > 0 || tmp.Received > 0)
            ////    {
            ////        Send = tmp.Send;
            ////        Received = tmp.Received;
            ////        break;
            ////    }
            ////    Thread.Sleep(500);
            ////}

            return new
            {
                SendAndReceived = "",// "上行" + Math.Round(networkInfo.SendLength / 1024.0 / 1024 / 1024, 2) + "GB 下行" + Math.Round(networkInfo.ReceivedLength / 1024.0 / 1024 / 1024, 2) + "GB", // 上下行流量统计
                NetworkSpeed = ""//"上行" + Send / 1024 + "kb/s 下行" + Received / 1024 + "kb/s" // 网络速度
            };
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
        /// 获取CPU使用率
        /// </summary>
        /// <returns></returns>
        private static string GetCPURate()
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
        /// 获取系统运行时间
        /// </summary>
        /// <returns></returns>
        private static string GetRunTime()
        {
            return FormatTime((long)(DateTimeOffset.Now - Process.GetCurrentProcess().StartTime).TotalMilliseconds);
            //return DateTimeUtil.FormatTime(Environment.TickCount);
        }

        /// <summary>
        /// 获取内存信息
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
        /// 获取外网IP和地理位置
        /// </summary>
        /// <returns></returns>
        private static async Task<string> GetWanIpFromPCOnline()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var url = "http://whois.pconline.com.cn/ipJson.jsp";
            var stream = await url.GetAsStreamAsync();
            var streamReader = new StreamReader(stream, Encoding.GetEncoding("GBK"));
            var html = streamReader.ReadToEnd();
            var tmp = html[(html.IndexOf("({") + 2)..].Split(",");
            var ipAddr = tmp[0].Split(":")[1] + "【" + tmp[7].Split(":")[1] + "】";
            return ipAddr.Replace("\"", "");
        }
    }

    /// <summary>
    /// 系统Shell命令
    /// </summary>
    public class ShellUtil
    {
        /// <summary>
        /// Bash命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string Bash(string command)
        {
            var escapedArgs = command.Replace("\"", "\\\"");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();
            return result;
        }

        /// <summary>
        /// cmd命令
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Cmd(string fileName, string args)
        {
            string output = string.Empty;
            var info = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = args,
                RedirectStandardOutput = true
            };
            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }
            return output;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class NetworkInfo
    {
        private readonly NetworkInterface _instance;
        public NetworkInterface NetworkInterface => _instance;
        private readonly Lazy<IPInterfaceStatistics> _statistics;
        private readonly Lazy<IPAddress> _addressIpv4;

        public IPAddress AddressIpv4 => _addressIpv4.Value; // IPv4 地址
        public string Mac => _instance?.GetPhysicalAddress().ToString(); // Mac地址
        public string Id => _instance?.Id; // 网络适配器的标识符
        public long ReceivedLength => _statistics.Value.BytesReceived; // 网络下载总量
        public long SendLength => _statistics.Value.BytesSent; // 网络上传总量

        private NetworkInfo(NetworkInterface network)
        {
            _instance = network;
            _statistics = new Lazy<IPInterfaceStatistics>(() => _instance?.GetIPStatistics());
            //_Ipv4Statistics = new Lazy<IPv4InterfaceStatistics>(() => _instance.GetIPv4Statistics());
            //_AddressIpv6 = new Lazy<IPAddress>(() => _instance.GetIPProperties().UnicastAddresses
            // .FirstOrDefault(x => x.IPv4Mask.ToString().Equals("0.0.0.0")).Address);
            _addressIpv4 = new Lazy<IPAddress>(() => _instance?.GetIPProperties().UnicastAddresses
            .FirstOrDefault(x => !x.IPv4Mask.ToString().Equals("0.0.0.0")).Address);
        }

        /// <summary>
        /// 当前正在联网的网卡信息
        /// </summary>
        /// <returns></returns>
        public static NetworkInfo GetNetworkInfo()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
                return new NetworkInfo(NetworkInterface.GetAllNetworkInterfaces()
                                  .FirstOrDefault(x => x.NetworkInterfaceType != NetworkInterfaceType.Loopback
                                  && x.NetworkInterfaceType != NetworkInterfaceType.Ethernet));

            return new NetworkInfo(NetworkInterface.GetAllNetworkInterfaces()
                                              .FirstOrDefault(x => x.OperationalStatus == OperationalStatus.Up
                                              && x.NetworkInterfaceType != NetworkInterfaceType.Loopback
                                              && x.NetworkInterfaceType != NetworkInterfaceType.Ethernet));
        }

        /// <summary>
        /// 获取当前网卡的网络速度
        /// </summary>
        /// <param name="Milliseconds"></param>
        /// <returns></returns>
        public (int Received, int Send) GetInternetSpeed(int Milliseconds)
        {
            var newNetwork = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(x => x.Id == Id).GetIPStatistics();

            long rec = ReceivedLength;
            long send = SendLength;
            Thread.Sleep(Milliseconds);
            return ((int)(newNetwork.BytesReceived - rec), (int)(newNetwork.BytesSent - send));
        }
    }
}