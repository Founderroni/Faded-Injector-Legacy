using FadedInjector.Utility;
using System;
using System.IO;
using DiscordRPC;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace FadedInjector
{
    public partial class MainForm : Form
    {
        public string FileToInjectDir;
        public DiscordRpcClient client;
        public readonly string InjectorDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string AssetDirectory = $@"{Directory.GetCurrentDirectory()}\Assets\";
        public readonly string AssetGitClient = "https://github.com/FadedKow/Assets/raw/main/MCBE%20Clients/";
        public string McVersion;
        public int ErrorsCatched = 0;
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
            FileIn.Filter = "dll |*.dll";
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
                if (AutoFocus.Checked) Utils.LaunchGame();
                Util.InjectDLL(FileToInjectDir);
            } else
            {
                if (ClientList.SelectedIndex == -1) { MessageBox.Show("Select a Client", "Error"); return; }
                if (AutoFocus.Checked) Utils.LaunchGame();
                Util.InjectClient();
            }
        }

        private void ClientList_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ClientList.SelectedIndex)
            {
                case 0:
                    Util.DownloadFile($"https://horion.download/bin/Horion.dll", $"{AssetDirectory}Horion.dll");
                    break;
                case 1:
                    Util.DownloadFile($"https://github.com/PacketDeveloper/PacketClient/raw/main/PacketClientFree.dll", $"{AssetDirectory}Packet.dll");
                    break;
                case 2:
                    Util.DownloadFile($"https://github.com/bernarddesfosse/onixclientautoupdate/raw/main/OnixClient.dll", $"{AssetDirectory}Onix.dll");
                    break;
                case 3:
                    Util.DownloadFile($"{AssetGitClient}NG.dll", $"{AssetDirectory}NG.dll");
                    break;
                case 4:
                    Util.DownloadFile($"{AssetGitClient}Fadeaway_1.16.201.dll", $"{AssetDirectory}Fadeaway1.16.201.dll");
                    break;
                case 5:
                    Util.DownloadFile($"{AssetGitClient}Fadeaway_1.17.11.1.dll", $"{AssetDirectory}Fadeaway1.17.11.1.dll");
                    break;
                case 6:
                    Util.DownloadFile($"{AssetGitClient}Zephyr.dll", $"{AssetDirectory}Zephyr.dll");
                    break;
                case 7:
                    Util.DownloadFile($"{AssetGitClient}KekClub.dll", $"{AssetDirectory}Kek.dll");
                    break;
                case 8:
                    Util.DownloadFile($"{AssetGitClient}Bom.dll", $"{AssetDirectory}Bom.dll");
                    break;
                case 9:
                    Util.DownloadFile($"https://github.com/BadMan-Client/BadMan-Releases/releases/latest/download/BadManPublic.dll", $"{AssetDirectory}Badman.dll");
                    break;
                case 10:
                    Util.DownloadFile($"https://cdn.strike.wtf/StrikeInjector.exe", $"{AssetDirectory}StrikeInjector.exe");
                    break;
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
                client.SetPresence(new RichPresence()
                {
                    Details = "Injecting Gamer Fluid",
                    State = "Committing Large Amounts Of Trolling",
                    Assets = new Assets()
                    {
                        LargeImageKey = "https://i.imgur.com/ufZN3k3.png", //Don't go above 32 characters
                        LargeImageText = "FadedInjector - Founder#8300",
                        SmallImageKey = "https://i.imgur.com/vOkR3H5.png"
                    }
                });
                client.Initialize();
            }
            else
            {
                client.Dispose();
            }
        }

        private void FreezeGif_Click(object sender, EventArgs e)
        {
            if (FreezeGif.Checked)
            {
                Animation.AnimatedGIF = false;
                Animation.Animated = false;
            }
            else
            {
                Animation.AnimatedGIF = true;
                Animation.Animated = true;
            }
        }

        private void Animation_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog Open = new OpenFileDialog();
                Open.Filter = "Image Files(*.jpg; *.png; *.jpeg; *.gif; *.bmp)|*.jpg; *.png; *.jpeg; *.gif; *.bmp)";
                if (Open.ShowDialog() == DialogResult.OK)
                {
                    string ImageFile = Open.FileName;
                    Image Img = Image.FromFile(ImageFile);
                    Animation.Image = Img;
                    Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Uh oh, a fucky wucky has occured:\n"+ex.Message, "Error");
            }
        }

        private void Spoof_Click(object sender, EventArgs e)
        {
            if (VersionList.SelectedIndex == -1) { MessageBox.Show("Version not selected", "Error"); return; }
            SpoofStart:
            try
            {
                Process.Start($"{AssetDirectory}Spoofer.exe", $"Spoof -v {McVersion}");
                ErrorsCatched = 0;
            } catch (Exception ex)
            {
                if (ErrorsCatched < 2)
                {
                    ErrorsCatched++;
                    Thread.Sleep(100);
                    goto SpoofStart;
                } else
                {
                    MessageBox.Show("Spoofer has encountered an error, is another program using it?\n" + ex.Message, "Spoof Error");
                    ErrorsCatched = 0;
                }
            }
        }

        private void VersionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Util.DownloadFile("https://github.com/fadeaway-client/FadedToolCLI/releases/latest/download/fadedtoolcli.exe", $"{AssetDirectory}Spoofer.exe");
            switch (VersionList.SelectedIndex)
            {
                case 0:
                    McVersion = "1.16.40";
                    break;
                case 1:
                    McVersion = "1.16.100";
                    break;
                case 2:
                    McVersion = "1.16.201";
                    break;
                case 3:
                    McVersion = "1.16.210";
                    break;
                case 4:
                    McVersion = "1.16.221";
                    break;
                case 5:
                    McVersion = "1.17.0";
                    break;
                case 6:
                    McVersion = "1.17.10";
                    break;
                case 7:
                    McVersion = "1.17.11";
                    break;
                case 8:
                    McVersion = "1.17.40";
                    break;
                case 9:
                    McVersion = "1.17.41";
                    break;
                case 10:
                    McVersion = "1.18.0";
                    break;
                case 11:
                    McVersion = "1.18.2";
                    break;
                case 12:
                    McVersion = "1.18.10.4";
                    break;
                case 13:
                    McVersion = "1.18.12.1";
                    break;
            }
        }

        private void DarknessSlider_Scroll(object sender, ScrollEventArgs e)
        {
            MainForm mf = (MainForm)Application.OpenForms["MainForm"];
            mf.BackColor = Color.FromArgb(DarknessSlider.Value, DarknessSlider.Value, DarknessSlider.Value);
        }
    }
}
