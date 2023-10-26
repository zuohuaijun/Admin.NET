// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

namespace Admin.NET.Core;

public static class ComputerUtil
{
    /// <summary>
    /// 内存信息
    /// </summary>
    /// <returns></returns>
    public static MemoryMetrics GetComputerInfo()
    {
        MemoryMetricsClient client = new();
        MemoryMetrics memoryMetrics = IsUnix() ? client.GetUnixMetrics() : client.GetWindowsMetrics();

        memoryMetrics.FreeRam = Math.Round(memoryMetrics.Free / 1024, 2) + "GB";
        memoryMetrics.UsedRam = Math.Round(memoryMetrics.Used / 1024, 2) + "GB";
        memoryMetrics.TotalRam = Math.Round(memoryMetrics.Total / 1024, 2) + "GB";
        memoryMetrics.RamRate = Math.Ceiling(100 * memoryMetrics.Used / memoryMetrics.Total).ToString() + "%";
        memoryMetrics.CpuRate = Math.Ceiling(GetCPURate().ParseToDouble()) + "%";
        return memoryMetrics;
    }

    /// <summary>
    /// 磁盘信息
    /// </summary>
    /// <returns></returns>
    public static List<DiskInfo> GetDiskInfos()
    {
        List<DiskInfo> diskInfos = new();

        if (IsUnix())
        {
            var output = ShellHelper.Bash(@"df -mT | awk '/^\/dev\/(sd|vd|xvd|nvme|sda|vda)/ {print $1,$2,$3,$4,$5,$6}'");
            var disks = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            if (disks.Length == 0) return diskInfos;

            var rootDisk = disks[1].Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
            if (rootDisk == null || rootDisk.Length == 0)
                return diskInfos;

            foreach (var item in disks)
            {
                var disk = item.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                if (disk == null || disk.Length == 0)
                    continue;

                var diskInfo = new DiskInfo()
                {
                    DiskName = disk[0],
                    TypeName = disk[1],
                    TotalSize = long.Parse(disk[2]) / 1024,
                    Used = long.Parse(disk[3]) / 1024,
                    AvailableFreeSpace = long.Parse(disk[4]) / 1024,
                    AvailablePercent = decimal.Parse(disk[5].Replace("%", ""))
                };
                diskInfos.Add(diskInfo);
            }
        }
        else
        {
            var driv = DriveInfo.GetDrives().Where(u => u.IsReady);
            foreach (var item in driv)
            {
                if (item.DriveType == DriveType.CDRom) continue;
                var obj = new DiskInfo()
                {
                    DiskName = item.Name,
                    TypeName = item.DriveType.ToString(),
                    TotalSize = item.TotalSize / 1024 / 1024 / 1024,
                    AvailableFreeSpace = item.AvailableFreeSpace / 1024 / 1024 / 1024,
                };
                obj.Used = obj.TotalSize - obj.AvailableFreeSpace;
                obj.AvailablePercent = decimal.Ceiling(obj.Used / (decimal)obj.TotalSize * 100);
                diskInfos.Add(obj);
            }
        }
        return diskInfos;
    }

    /// <summary>
    /// 获取外网IP地址
    /// </summary>
    /// <returns></returns>
    public static string GetIpFromOnline()
    {
        var url = "http://myip.ipip.net";
        var stream = url.GetAsStreamAsync().GetAwaiter().GetResult();
        var streamReader = new StreamReader(stream.Stream, stream.Encoding);
        var html = streamReader.ReadToEnd();
        return !html.Contains("当前 IP：") ? "未知" : html.Replace("当前 IP：", "").Replace("来自于：", "");
    }

    public static bool IsUnix()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }

    public static string GetCPURate()
    {
        string cpuRate;
        if (IsUnix())
        {
            string output = ShellUtil.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'");
            cpuRate = output.Trim();
        }
        else
        {
            string output = ShellUtil.Cmd("wmic", "cpu get LoadPercentage");
            cpuRate = output.Replace("LoadPercentage", string.Empty).Trim();
        }
        return cpuRate;
    }

    /// <summary>
    /// 获取系统运行时间
    /// </summary>
    /// <returns></returns>
    public static string GetRunTime()
    {
        string runTime = string.Empty;
        if (IsUnix())
        {
            string output = ShellUtil.Bash("uptime -s").Trim();
            runTime = DateTimeUtil.FormatTime((DateTime.Now - output.ParseToDateTime()).TotalMilliseconds.ToString().Split('.')[0].ParseToLong());
        }
        else
        {
            string output = ShellUtil.Cmd("wmic", "OS get LastBootUpTime/Value");
            string[] outputArr = output.Split('=', (char)StringSplitOptions.RemoveEmptyEntries);
            if (outputArr.Length == 2)
                runTime = DateTimeUtil.FormatTime((DateTime.Now - outputArr[1].Split('.')[0].ParseToDateTime()).TotalMilliseconds.ToString().Split('.')[0].ParseToLong());
        }
        return runTime;
    }
}

/// <summary>
/// 内存信息
/// </summary>
public class MemoryMetrics
{
    [JsonIgnore]
    public double Total { get; set; }

    [JsonIgnore]
    public double Used { get; set; }

    [JsonIgnore]
    public double Free { get; set; }

    /// <summary>
    /// 已用内存
    /// </summary>
    public string UsedRam { get; set; }

    /// <summary>
    /// CPU使用率%
    /// </summary>
    public string CpuRate { get; set; }

    /// <summary>
    /// 总内存 GB
    /// </summary>
    public string TotalRam { get; set; }

    /// <summary>
    /// 内存使用率 %
    /// </summary>
    public string RamRate { get; set; }

    /// <summary>
    /// 空闲内存
    /// </summary>
    public string FreeRam { get; set; }
}

/// <summary>
/// 磁盘信息
/// </summary>
public class DiskInfo
{
    /// <summary>
    /// 磁盘名
    /// </summary>
    public string DiskName { get; set; }

    /// <summary>
    /// 类型名
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// 总剩余
    /// </summary>
    public long TotalFree { get; set; }

    /// <summary>
    /// 总量
    /// </summary>
    public long TotalSize { get; set; }

    /// <summary>
    /// 已使用
    /// </summary>
    public long Used { get; set; }

    /// <summary>
    /// 可使用
    /// </summary>
    public long AvailableFreeSpace { get; set; }

    /// <summary>
    /// 使用百分比
    /// </summary>
    public decimal AvailablePercent { get; set; }
}

public class MemoryMetricsClient
{
    /// <summary>
    /// windows系统获取内存信息
    /// </summary>
    /// <returns></returns>
    public MemoryMetrics GetWindowsMetrics()
    {
        string output = ShellUtil.Cmd("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");
        var metrics = new MemoryMetrics();
        var lines = output.Trim().Split('\n', (char)StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length <= 0) return metrics;

        var freeMemoryParts = lines[0].Split('=', (char)StringSplitOptions.RemoveEmptyEntries);
        var totalMemoryParts = lines[1].Split('=', (char)StringSplitOptions.RemoveEmptyEntries);

        metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
        metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);//m
        metrics.Used = metrics.Total - metrics.Free;

        return metrics;
    }

    /// <summary>
    /// Unix系统获取
    /// </summary>
    /// <returns></returns>
    public MemoryMetrics GetUnixMetrics()
    {
        string output = ShellUtil.Bash("free -m | awk '{print $2,$3,$4,$5,$6}'");
        var metrics = new MemoryMetrics();
        var lines = output.Split('\n', (char)StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length <= 0) return metrics;

        if (lines != null && lines.Length > 0)
        {
            var memory = lines[1].Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
            if (memory.Length >= 3)
            {
                metrics.Total = double.Parse(memory[0]);
                metrics.Used = double.Parse(memory[1]);
                metrics.Free = double.Parse(memory[2]);//m
            }
        }
        return metrics;
    }
}

public class ShellUtil
{
    /// <summary>
    /// linux 系统命令
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
    /// windows系统命令
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string Cmd(string fileName, string args)
    {
        string output = string.Empty;

        var info = new ProcessStartInfo();
        info.FileName = fileName;
        info.Arguments = args;
        info.RedirectStandardOutput = true;

        using (var process = Process.Start(info))
        {
            output = process.StandardOutput.ReadToEnd();
        }
        return output;
    }
}

public class ShellHelper
{
    /// <summary>
    /// Linux 系统命令
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
    /// Windows 系统命令
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string Cmd(string fileName, string args)
    {
        string output = string.Empty;

        var info = new ProcessStartInfo();
        info.FileName = fileName;
        info.Arguments = args;
        info.RedirectStandardOutput = true;

        using (var process = Process.Start(info))
        {
            output = process.StandardOutput.ReadToEnd();
        }
        return output;
    }
}