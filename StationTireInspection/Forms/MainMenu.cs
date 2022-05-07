using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using StationTireInspection.Classes;
using StationTireInspection.Forms;
using StationTireInspection.Forms.MessageBoxes;
using StationTireInspection.Forms.Settings;
using StationTireInspection.Forms.SettingsLogin;
using StationTireInspection.JDO.DataToServer;
using StationTireInspection.UDT;

namespace StationTireInspection
{
    public partial class MainMenu : Form
    {
        private readonly Color SELECTED_BUTTON_COLOR = Color.FromArgb(128, 0, 128);
        private readonly Color DEFAULT_BUTTON_COLOR = Color.FromArgb(64,64,64);

        private readonly string SETTING_FILE_PATH = "settings.json";
        private readonly string ENCRIPTION_KEY = "W]rs6^%]";

        private SettingsJDO Settings { get; set; } = new SettingsJDO();
        private DataToServerJDO DataToServer { get; set; } = new DataToServerJDO();

        private MySQLDatabase mySQLDatabase = new MySQLDatabase();

        private LoginBox loginBox;
        private TCPIPClient readerTCPClient;
        private TCPIPClient serverTCPClient;
        private TCPIPClient interfacTCPIPClient;
        private Login login;
        private ChangePassword changePassword;
        private CommDiagnostics diagnostics;
        private DatabaseSettings databaseSettings;
        private AboutApp aboutApp;
        private BarcodeReaderSettings barcodeReaderSettings;
        private StationSettings stationSettings;
        private MainAppConnectionSettings mainAppConnectionSettings;
        private ServerClient serverClient;
        private PLCStationInterfaceSettings pLCStationInterfaceSettings;

        private bool LoginIcon;

        private bool mouseDown;
        private Point lastLocation;

        private Button activeButton;
        public Button ActiveButton
        {
            get { return activeButton; }
            set 
            {
                if (activeButton != null)
                {
                    activeButton.BackColor = DEFAULT_BUTTON_COLOR;
                }

                activeButton = value;

                activeButton.BackColor = SELECTED_BUTTON_COLOR;
            }
        }

        private Form activePage;
        public Form ActivePage
        {
            get { return activePage; }
            set
            {
                if (activePage != null)
                {
                    activePage.Hide();
                }

                activePage = value;

                activePage.Show();
            }
        }

        private Dictionary<Button, Form> pages = new Dictionary<Button, Form>();

        public MainMenu()
        {
            InitializeComponent();

            ReadSettingsJSON(SETTING_FILE_PATH, ENCRIPTION_KEY);

            Translator.LanguageChanged += Translate;

            loginBox = new LoginBox(Settings.SettingsLogin);
            readerTCPClient = new TCPIPClient();
            serverTCPClient = new TCPIPClient();
            interfacTCPIPClient = new TCPIPClient();
            changePassword = new ChangePassword(mySQLDatabase, Settings);
            login = new Login(mySQLDatabase, Settings, changePassword, DataToServer);
            serverClient = new ServerClient(readerTCPClient, serverTCPClient, DataToServer, login);
            barcodeReaderSettings = new BarcodeReaderSettings(Settings, readerTCPClient, loginBox);
            diagnostics = new CommDiagnostics(mySQLDatabase, readerTCPClient, serverTCPClient, DataToServer, interfacTCPIPClient);
            databaseSettings = new DatabaseSettings(Settings, mySQLDatabase, loginBox);
            aboutApp = new AboutApp();
            pLCStationInterfaceSettings = new PLCStationInterfaceSettings(Settings, interfacTCPIPClient, loginBox);
            stationSettings = new StationSettings(Settings, DataToServer, loginBox);
            mainAppConnectionSettings = new MainAppConnectionSettings(Settings, serverTCPClient, loginBox);

            login.LoginResultChanged += LoginChanged;

            AddPage(login);
            AddPage(changePassword);
            AddPage(diagnostics);
            AddPage(databaseSettings);
            AddPage(stationSettings);
            AddPage(barcodeReaderSettings);
            AddPage(pLCStationInterfaceSettings);
            AddPage(mainAppConnectionSettings);
            AddPage(aboutApp);

            Translator.Language = Language.ENG;

            loginBox.LogedChanged += Login_Changed;
            LoginIcon = true;

            ActiveButton = btnAboutApp;
            ActivePage = aboutApp;

            pages.Add(btnLogin, login);
            pages.Add(btnChangePassword, changePassword);
            pages.Add(btnDiagnostics, diagnostics);
            pages.Add(btnDatabaseSettings, databaseSettings);
            pages.Add(btnStationSettings, stationSettings);
            pages.Add(btnReaderSettings, barcodeReaderSettings);
            pages.Add(btnPLCStationInterfaceSettings, pLCStationInterfaceSettings);
            pages.Add(btnMainAppSettings, mainAppConnectionSettings);
            pages.Add(btnAboutApp, aboutApp);

            DataToServer.StationInformation.StationName = Settings.StationSettings.StationName;
            DataToServer.StationInformation.StationID = Settings.StationSettings.StationID;
            DataToServer.NonOperation = 0;
            DataToServer.UserInformation.Status = LoginResult.NoLogged;

            mySQLDatabase.ExceptionChanged += MySqlExceptionChanged_ShowPopUp;
            readerTCPClient.ExceptionChanged += TCPClientExceptionChanged_ShowPopUp;
            serverTCPClient.ExceptionChanged += TCPClientExceptionChanged_ShowPopUp;
            interfacTCPIPClient.ExceptionChanged += TCPClientExceptionChanged_ShowPopUp;

            mySQLDatabase.ConnectToDB_Async(Settings.DatabaseSettings.IPAddress, Settings.DatabaseSettings.DatabaseUserName, Settings.DatabaseSettings.DatabasePassword);
            //mySQLDatabase.ConnectToDB(Settings.DatabaseSettings.IPAddress, Settings.DatabaseSettings.DatabaseUserName, Settings.DatabaseSettings.DatabasePassword);

            readerTCPClient.IPAddress = Settings.BarcodeReaderSettings.IPAddress;
            readerTCPClient.Port = Settings.BarcodeReaderSettings.Port;
            readerTCPClient.Connect_Async();

            //readerTCPClient.Connect();

            serverTCPClient.IPAddress = Settings.MainAppConnectionSettings.IPAddress;
            serverTCPClient.Port = Settings.MainAppConnectionSettings.Port;
            serverTCPClient.Connect_Async();

            //serverTCPClient.Connect();

            interfacTCPIPClient.IPAddress = Settings.PLCStationInterfaceSettings.IPAddress;
            interfacTCPIPClient.Port = Settings.PLCStationInterfaceSettings.Port;
            interfacTCPIPClient.Connect_Async();

            //interfacTCPIPClient.Connect();
        }

        private void TCPClientExceptionChanged_ShowPopUp(object sender, Exception e)
        {
            Invoke((MethodInvoker)delegate ()
            {
                CustomMessageBox.ShowPopup("TCPIP Client Error", e.Message);
            });
        }

        private void MySqlExceptionChanged_ShowPopUp(object sender, Exception e)
        {
            Invoke((MethodInvoker)delegate ()
            {
                CustomMessageBox.ShowPopup("MySQL Error", e.Message);
            });
        }

        private void AddPage(Form form)
        {
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            pagePanel.Controls.Add(form);
        }

        private void LoginChanged(object sender, LoginResult e)
        {
            if(e == LoginResult.ChangePassword)
            {
                ActiveButton = btnChangePassword;
                ActivePage = changePassword;
            }
        }

        private void Translate(object sender, Language e)
        {
            if (Translator.Language == Language.CZ)
            {
                Text = "NMP Station Tire Inspection";
                lblTitle.Text = "NMP Station Tire Inspection";
                btnLogin.Text = "Přihlášení do VII";
                btnChangePassword.Text = "Změna Hesla";
                btnDiagnostics.Text = "Diagnostika";
                btnDatabaseSettings.Text = "Nastavení Databáze";
                btnReaderSettings.Text = "Nastavení Čtečky Barkódů";
                btnMainAppSettings.Text = "Nastavení Hlavní Aplikace";
                btnPLCStationInterfaceSettings.Text = "Nastavení PLC <--> Station Interfacu";
                btnStationSettings.Text = "Nastavení Stanice";
                btnAboutApp.Text = "O Aplikaci";
            }
            else if (Translator.Language == Language.ENG)
            {
                Text = "NMP Station Tire Inspection";
                lblTitle.Text = "NMP Station Tire Inspection";
                btnLogin.Text = "Login to VII";
                btnChangePassword.Text = "Change Password";
                btnDiagnostics.Text = "Diagnostics";
                btnDatabaseSettings.Text = "Database Settings";
                btnReaderSettings.Text = "Barcode Reader Settings";
                btnMainAppSettings.Text = "Main App Settings";
                btnPLCStationInterfaceSettings.Text = "PLC <--> Station Interface Settings";
                btnStationSettings.Text = "Station Settings";
                btnAboutApp.Text = "About App";
            }
        }

        private void ReadSettingsJSON(string Path, string CryptKey)
        {
            if (File.Exists(Path))
            {
                EncriptionManager.DecryptFile(Path, CryptKey);
                Settings = SettingsJDO.Deserialize(File.ReadAllText(Path));
            }
            else
            {
                File.WriteAllText(Path, Settings.Serialize());
            }

            EncriptionManager.EncryptFile(Path, CryptKey);
        }

        private void WriteSettingsJSON(string Path, string CryptKey)
        {
            EncriptionManager.DecryptFile(Path, CryptKey);
            File.WriteAllText(Path, Settings.Serialize());
            EncriptionManager.EncryptFile(Path, CryptKey);
        }

        private void pbLoged_Click(object sender, EventArgs e)
        {
            if(loginBox.LogedIn == false)
            {
                loginBox.ShowLoginDialog();
            }
            else
            {
                loginBox.LogedIn = false;
            }
        }

        private void Login_Changed(object sender, bool e)
        {
            if (e == true)
            {
                pbLoged.Image.Dispose();
                pbLoged.Image = Properties.Resources.logout;
            }
            else
            {
                pbLoged.Image.Dispose();
                pbLoged.Image = Properties.Resources.login;
            }
        }

        private void pbLanguage_Click(object sender, EventArgs e)
        {
            if (Translator.Language == Language.CZ)
            {
                pbLanguage.Image.Dispose();
                pbLanguage.Image = Properties.Resources.cz;
                Translator.Language = Language.ENG;
            }
            else
            {
                pbLanguage.Image.Dispose();
                pbLanguage.Image = Properties.Resources.gb;
                Translator.Language = Language.CZ;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            login.SetStopNonOPInfosAndSendToDB();

            Application.Exit();
        }

        private void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void lblTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Location = new Point((Location.X - lastLocation.X) + e.X, (Location.Y - lastLocation.Y) + e.Y);
                Update();
            }
        }

        private void lblTitle_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteSettingsJSON(SETTING_FILE_PATH, ENCRIPTION_KEY);
        }

        private void pagesBtn_Click(object sender, EventArgs e)
        {
            ActiveButton = sender as Button;
            ActivePage = pages[ActiveButton];
        }
    }
}
