using System;
using System.Diagnostics;
using System.Threading;
using Windows.Win32.System.ProcessStatus;
using Windows.Win32.System.SystemInformation;

// C# - 윈도우 작업 관리자와 리소스 모니터의 메모리 값을 구하는 방법
// http://www.sysnet.pe.kr/2/0/11950

var freeMemory = new PerformanceCounter("Memory", "Free & Zero Page List Bytes", true);
var modifiedMemory = new PerformanceCounter("Memory", "Modified Page List Bytes", true);

while (true)
{
    PERFORMANCE_INFORMATION pi = new PERFORMANCE_INFORMATION();
    pi.Initialize();
    Windows.Win32.PInvoke.GetPerformanceInfo(ref pi, pi.cb);

    Console.WriteLine("[Resource Monitor]");

    Windows.Win32.PInvoke.GetPhysicallyInstalledSystemMemory(out ulong installedMemory);

    double reserved = (installedMemory - (pi.Total / 1024.0));
    ulong modified = (ulong)modifiedMemory.RawValue;
    ulong inuse = pi.Total - pi.Available - modified;

    long reservedMB = (long)Math.Round(reserved / 1024.0);
    Console.WriteLine($"Hardware Reserved: {reservedMB} MB");

    Console.WriteLine($"In Use: {inuse / 1024 / 1024} MB");
    Console.WriteLine($"Modified: {modified / 1024 / 1024} MB");

    ulong free = (ulong)freeMemory.RawValue;
    ulong standby = pi.Available - free;
    Console.WriteLine($"Standby: {standby / 1024 / 1024} MB");
    Console.WriteLine($"Free: {free / 1024 / 1024} MB");
    Console.WriteLine();
    Console.WriteLine($"Available: {pi.Available.MB()} MB");
    Console.WriteLine($"Cached: {(standby + modified).MB()} MB");
    Console.WriteLine($"Total: {pi.Total.MB()} MB");
    Console.WriteLine($"Installed: {installedMemory / 1024} MB");

    MEMORYSTATUSEX globalMemoryStatus = new MEMORYSTATUSEX();
    globalMemoryStatus.Initialize();
    Windows.Win32.PInvoke.GlobalMemoryStatusEx(ref globalMemoryStatus);

    Console.WriteLine();
    Console.WriteLine("[Task Manager]");

    Console.WriteLine($"Memory: {installedMemory / 1024.0 / 1024.0} GB");
    Console.WriteLine($"Memory usage: {pi.Total / 1024.0 / 1024.0 / 1024.0:#.0} GB");
    Console.WriteLine();
    Console.WriteLine($"In use: {inuse / 1024.0 / 1024.0 / 1024.0:#.0} GB");
    Console.WriteLine($"Available: {pi.Available / 1024.0 / 1024.0 / 1024.0:#.0} GB");
    Console.WriteLine($"Committed: {pi.Commit / 1024.0 / 1024.0 / 1024.0:#.0} / {globalMemoryStatus.ullTotalPageFile / 1024.0 / 1024.0 / 1024.0:#.0} GB");
    Console.WriteLine($"Cached: {(standby + modified) / 1024.0 / 1024.0 / 1024.0:#.0} GB");
    Console.WriteLine($"Paged pool: {pi.KernelPage / 1024.0 / 1024.0 / 1024.0:#.0} GB");
    Console.WriteLine($"Non-paged pool: {pi.KernelNonPage / 1024.0 / 1024.0:#} MB");

    Console.WriteLine();
    Thread.Sleep(1000);
}

static class Unit
{
    public static string MB(this ulong size)
    {
        return $"{(size / 1024 / 1024)}";
    }
}
