using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using StationTireInspection.Classes;
using StationTireInspection.Forms;
using StationTireInspection.Forms.Settings;
using StationTireInspection.UDT;

namespace StationTireInspection
{
    public partial class MainMenu : Form
    {
        private readonly Color SELECTED_BUTTON_COLOR = Color.FromArgb(128, 0, 128);
        private readonly Color DEFAULT_BUTTON_COLOR = Color.FromArgb(64,64,64);

        private SettingsJDO Settings { get; set; } = new SettingsJDO();

        private MySQLDatabase mySQLDatabase = new MySQLDatabase();

        private Login login;
        private ChangePassword changePassword;
        private Diagnostics diagnostics;
        private DatabaseSettings databaseSettings;
        private AboutApp aboutApp;
        private BarcodeReaderSettings barcodeReaderSettings;
        private StationSettings stationSettings;
        private MainAppConnectionSettings mainAppConnectionSettings;

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
                if(activePage != null)
                {
                    activePage.Hide();
                }

                activePage = value;

                activePage.Show();
            }
        }

        public MainMenu()
        {
            InitializeComponent();

            

            if (File.Exists("settings.json"))
            {
                EncriptionManager.DecryptFile("settings.json", "W]rs6^%]");
                Settings = SettingsJDO.Deserialize(File.ReadAllText("settings.json"));
            }
            else
            {
                File.WriteAllText("settings.json", Settings.Serialize());
            }

            EncriptionManager.EncryptFile("settings.json", "W]rs6^%]");

            //SettingsData = SettingsJDO.Deserialize()

            Translator.LanguageChanged += Translate;

            login = new Login();
            changePassword = new ChangePassword();
            diagnostics = new Diagnostics();
            databaseSettings = new DatabaseSettings(Settings);
            aboutApp = new AboutApp();
            barcodeReaderSettings = new BarcodeReaderSettings(Settings);
            stationSettings = new StationSettings(Settings);
            mainAppConnectionSettings = new MainAppConnectionSettings(Settings);

            //connectToDatabase.clientStatusDot1.Client = mySQLDatabase;

            login.TopLevel = false;
            login.Dock = DockStyle.Fill;
            pagePanel.Controls.Add(login);

            changePassword.TopLevel = false;
            changePassword.Dock = DockStyle.Fill;
            pagePanel.Controls.Add(changePassword);

            diagnostics.TopLevel = false;
            diagnostics.Dock = DockStyle.Fill;
            pagePanel.Controls.Add(diagnostics);

            databaseSettings.TopLevel = false;
            databaseSettings.Dock = DockStyle.Fill;
            pagePanel.Controls.Add(databaseSettings);

            aboutApp.TopLevel = false;
            aboutApp.Dock = DockStyle.Fill;
            pagePanel.Controls.Add(aboutApp);

            stationSettings.TopLevel = false;
            stationSettings.Dock = DockStyle.Fill;
            pagePanel.Controls.Add(stationSettings);

            barcodeReaderSettings.TopLevel = false;
            barcodeReaderSettings.Dock = DockStyle.Fill;
            pagePanel.Controls.Add(barcodeReaderSettings);

            mainAppConnectionSettings.TopLevel = false;
            mainAppConnectionSettings.Dock = DockStyle.Fill;
            pagePanel.Controls.Add(mainAppConnectionSettings);


            Translator.Language = Language.ENG;
            LoginManager.LogedIn = true;

            ActiveButton = btnAboutApp;
            ActivePage = aboutApp;
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
                btnStationSettings.Text = "Station Settings";
                btnAboutApp.Text = "About App";
            }
        }

        private void pbLoged_Click(object sender, EventArgs e)
        {
            if (LoginManager.LogedIn == true)
            {
                pbLoged.Image.Dispose();
                pbLoged.Image = Properties.Resources.logout;
                LoginManager.LogedIn = false;
            }
            else
            {
                pbLoged.Image.Dispose();
                pbLoged.Image = Properties.Resources.login;
                LoginManager.LogedIn = true;
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ActiveButton = sender as Button;
            ActivePage = login;
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            ActiveButton = sender as Button;
            ActivePage = changePassword;
        }

        private void btnDiagnostics_Click(object sender, EventArgs e)
        {
            ActiveButton = sender as Button;
            ActivePage = diagnostics;
        }

        private void btnDatabaseSettings_Click(object sender, EventArgs e)
        {
            ActiveButton = sender as Button;
            ActivePage = databaseSettings;
        }

        private void btnAboutApp_Click(object sender, EventArgs e)
        {
            ActiveButton = sender as Button;
            ActivePage = aboutApp;
        }

        private void btnReaderSettings_Click(object sender, EventArgs e)
        {
            ActiveButton = sender as Button;
            ActivePage = barcodeReaderSettings;
        }

        private void btnStationSettings_Click(object sender, EventArgs e)
        {
            ActiveButton = sender as Button;
            ActivePage = stationSettings;
        }

        private void btnMainAppSettings_Click(object sender, EventArgs e)
        {
            ActiveButton = sender as Button;
            ActivePage = mainAppConnectionSettings;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
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
            EncriptionManager.DecryptFile("settings.json", "W]rs6^%]");
            File.WriteAllText("settings.json", Settings.Serialize());
            EncriptionManager.EncryptFile("settings.json", "W]rs6^%]");
        }
    }
}
