using System.Runtime.InteropServices;

namespace Windows.Win32
{
    namespace System.ProcessStatus
    {
        partial struct PERFORMANCE_INFORMATION
        {
            public void Initialize()
            {
                this.cb = (uint)Marshal.SizeOf(typeof(PERFORMANCE_INFORMATION));
            }

            ulong Page
                => PageSize;

            public ulong Total
                => PhysicalTotal * Page;

            public ulong Available
                => PhysicalAvailable * Page;

            public ulong Cache
                => SystemCache * Page;

            public ulong KernelNonPage
                => KernelNonpaged * Page;

            public ulong KernelPage
                => KernelPaged * Page;

            public ulong Commit
                => CommitTotal * Page;
        }
    }

    namespace System.SystemInformation
    {
        partial struct MEMORYSTATUSEX
        {
            public void Initialize()
            {
                this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }
    }
}