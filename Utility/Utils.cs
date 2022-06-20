using System;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Security.AccessControl;
using System.Security.Principal;
using System.IO;
using System.Net;
using System.ComponentModel;

namespace FadedInjector.Utility
{
    public class Utils
    {
        //Credits Echo
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
            uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess,
            IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        // privileges
        const int PROCESS_CREATE_THREAD = 0x0002;
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_READ = 0x0010;

        // used for memory allocation
        const uint MEM_COMMIT = 0x00001000;
        const uint MEM_RESERVE = 0x00002000;
        const uint PAGE_READWRITE = 4;

        public void InjectDLL(string DLLPath)
        {
            Process[] targetProcessIndex = Process.GetProcessesByName("Minecraft.Windows");
            if (targetProcessIndex.Length > 0)
            {
                applyAppPackages(DLLPath);

                Process targetProcess = Process.GetProcessesByName("Minecraft.Windows")[0];
                IntPtr procHandle = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, targetProcess.Id);

                IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

                IntPtr allocMemAddress = VirtualAllocEx(procHandle, IntPtr.Zero, (uint)((DLLPath.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);

                UIntPtr bytesWritten;
                WriteProcessMemory(procHandle, allocMemAddress, Encoding.Default.GetBytes(DLLPath), (uint)((DLLPath.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten);
                CreateRemoteThread(procHandle, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);

                //MessageBox.Show("Injected!");
            }
            else
            {
                    MessageBox.Show("Minecraft is not open, launching now...");
                    Process.Start("minecraft://");
                    Thread.Sleep(2000);
                    InjectDLL(DLLPath);
                    MessageBox.Show("You appear to not have Minecraft installed!");
            }
        }

        public static void applyAppPackages(string DLLPath)
        {
            FileInfo InfoFile = new FileInfo(DLLPath);
            FileSecurity fSecurity = InfoFile.GetAccessControl();
            fSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier("S-1-15-2-1"), FileSystemRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            InfoFile.SetAccessControl(fSecurity);
        }

        private void FileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            
        }

        public void DownloadFile(string file, string location)
        {
            new Thread(() =>
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(FileCompleted);
                webClient.DownloadFileAsync(new Uri(file), location);
            }).Start();
        }

        public void CheckForDirectory()
        {
            if (!Directory.Exists(MainForm.AssetDirectory))
                Directory.CreateDirectory(MainForm.AssetDirectory);
        }

        public static void LaunchGame()
        {// Credits Onix
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                FileName = "explorer.exe",
                Arguments = "shell:appsFolder\\Microsoft.MinecraftUWP_8wekyb3d8bbwe!App",
            };
            process.StartInfo = startInfo;
            process.Start();
        }

        public void InjectClient()
        {
            MainForm mf = (MainForm)Application.OpenForms["MainForm"];
            switch (mf.ClientList.SelectedIndex)
            {
                case 0:
                    InjectDLL($"{MainForm.AssetDirectory}Horion.dll");
                    break;
                case 1:
                    InjectDLL($"{MainForm.AssetDirectory}Packet.dll");
                    break;
                case 2:
                    InjectDLL($"{MainForm.AssetDirectory}Onix.dll");
                    break;
                case 3:
                    InjectDLL($"{MainForm.AssetDirectory}NG.dll");
                    break;
                case 4:
                    InjectDLL($"{MainForm.AssetDirectory}Fadeaway1.16.201.dll");
                    break;
                case 5:
                    InjectDLL($"{MainForm.AssetDirectory}Fadeaway1.17.11.1.dll");
                    break;
                case 6:
                    InjectDLL($"{MainForm.AssetDirectory}Zephyr.dll");
                    break;
                case 7:
                    InjectDLL($"{MainForm.AssetDirectory}Kek.dll");
                    break;
                case 8:
                    InjectDLL($"{MainForm.AssetDirectory}Bom.dll");
                    break;
                case 9:
                    InjectDLL($"{MainForm.AssetDirectory}Badman.dll");
                    break;
                case 10:
                    InjectDLL($"{MainForm.AssetDirectory}StrikeInjector.exe");
                    break;
                case 11:
                    InjectDLL($"{MainForm.AssetDirectory}PacketV2.11.dll");
                    break;
            }
        }
    }
}
