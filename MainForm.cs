using FadedInjector.Utility;
using System;
using System.IO;
using DiscordRPC;
using System.Diagnostics;
using System.Windows.Forms;

namespace FadedInjector
{
    public partial class MainForm : Form
    {
        public string FileToInjectDir;
        public DiscordRpcClient client;
        public readonly string InjectorDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string AssetDirectory = $@"{Directory.GetCurrentDirectory()}\Assets\";
        public readonly string AssetGitClient = "https://github.com/FadedKow/Assets/blob/main/MCBE%20Clients/";
        Utils Util = new Utils();

        public MainForm()
        {
            InitializeComponent();
        }

        private void Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Injector - Founder\nInjection Functionality - EchoHackCMD", "Credits");
        }

        private void Discord_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/KGGX69sXqf");
        }

        private void SelectDLL_Click(object sender, EventArgs e)
        {
            Util.CheckForDirectory();
            OpenFileDialog FileIn = new OpenFileDialog();
            if (FileIn.ShowDialog() == DialogResult.OK)
            {
                if (FileIn.SafeFileName.ToLower().EndsWith(".dll"))
                {
                    if (AutoInject.Checked)
                        Util.InjectDLL(FileIn.FileName);
                    else
                        FileToInjectDir = FileIn.FileName;
                }
                else
                {
                    MessageBox.Show("You did not specify a DLL!");
                }
            }
        }

        private void Inject_Click(object sender, EventArgs e)
        {
            Util.CheckForDirectory();
            if (!AutoInject.Checked)
            {
                if (FileToInjectDir == "" || FileToInjectDir == null) { MessageBox.Show("Select a DLL", "Error"); return; }
                Util.InjectDLL(FileToInjectDir);
            } else
            {
                if (ClientList.SelectedIndex == -1) { MessageBox.Show("Select a Client", "Error"); return; }
                Util.InjectClient();
            }
        }

        private void ClientList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //It works so I'm not gonna bother changing it, if someone can make it better create a pull request
            if (ClientList.SelectedIndex == 0)
            {
                Util.DownloadFile($"https://horion.download/bin/Horion.dll", $"{AssetDirectory}Horion.dll");
            }
            else if (ClientList.SelectedIndex == 1)
            {
                Util.DownloadFile($"https://github.com/PacketDeveloper/PacketClient/raw/main/PacketClientFree.dll", $"{AssetDirectory}Packet.dll");
            }
            else if (ClientList.SelectedIndex == 2)
            {
                Util.DownloadFile($"https://github.com/bernarddesfosse/onixclientautoupdate/raw/main/OnixClient.dll", $"{AssetDirectory}Onix.dll");
            }
            else if (ClientList.SelectedIndex == 3)
            {
                Util.DownloadFile($"{AssetGitClient}NG.dll", $"{AssetDirectory}NG.dll");
            }
            else if (ClientList.SelectedIndex == 4)
            {
                Util.DownloadFile($"{AssetGitClient}Fadeaway_1.16.201.dll", $"{AssetDirectory}Fadeaway1.16.201.dll");
            }
            else if (ClientList.SelectedIndex == 5)
            {
                Util.DownloadFile($"{AssetGitClient}Fadeaway_1.17.11.1.dll", $"{AssetDirectory}Fadeaway1.17.11.1.dll");
            }
            else if (ClientList.SelectedIndex == 6)
            {
                Util.DownloadFile($"{AssetGitClient}Zephyr.dll", $"{AssetDirectory}Zephyr.dll");
            }
            else if (ClientList.SelectedIndex == 7)
            {
                Util.DownloadFile($"{AssetGitClient}KekClub.dll", $"{AssetDirectory}Kek.dll");
            }
            else if (ClientList.SelectedIndex == 8)
            {
                Util.DownloadFile($"{AssetGitClient}Bom.dll", $"{AssetDirectory}Bom.dll");
            }
        }

        private void AutoInject_Click(object sender, EventArgs e)
        {
            if (AutoInject.Checked)
                Inject.Text = "Inject Client";
            else
                Inject.Text = "Inject DLL";
        }

        private void DiscordRpc_Click(object sender, EventArgs e)
        {
            if (DiscordRpc.Checked)
            {
                client = new DiscordRpcClient("961004803843031040");
                client.Initialize();
                client.SetPresence(new RichPresence()
                {
                    Details = "Injecting Gamer Fluid",
                    State = "Committing Large Amounts Of Trolling",
                    Assets = new Assets()
                    {
                        LargeImageKey = "shorturl.at/oMZ25", //Don't go above 32 characters
                        LargeImageText = "FadedInjector - Founder#8300",
                        SmallImageKey = "shorturl.at/lxzA0"
                    }
                });
            }
            else
            {
                client.Dispose();
            }
        }
    }
}
