using StationTireInspection.Classes;
using StationTireInspection.Forms.MessageBoxes;
using StationTireInspection.JDO.DataToServer;
using StationTireInspection.UDT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace StationTireInspection.Forms
{
    public partial class Login : Form
    {
        private readonly Color SELECTED_BUTTON_COLOR = Color.FromArgb(128, 0, 128);
        private readonly Color DEFAULT_BUTTON_COLOR = Color.FromArgb(64, 64, 64);
        private readonly Color SELECTED_TEXTBOX_COLOR = Color.Red;
        private readonly Color DEFAULT_TEXTBOX_COLOR = Color.White;

        private Dictionary<Button, int> nonOperationsButtons = new Dictionary<Button, int>();
        private Dictionary<int, TextBox> nonOperationsTextBoxes = new Dictionary<int, TextBox>();

        MySQLDatabase MySQLDatabase;
        SettingsJDO Settings;
        ChangePassword ChangePassword;
        DataToServerJDO DataToServer;

        NonOperationInformations NonOperationInformations;

        string ErrorTitle = "";
        string[] Errors = new string[3];

        string MessageTitle = "";
        string[] Messages = new string[2];

        private int _NonOperation;
        private bool OnlyChangeNonOP = false;

        //System.Timers.Timer NonOperationTimer = new System.Timers.Timer();

        public event EventHandler<LoginResult> LoginResultChanged;
        public event EventHandler<int> NonOperationChanged;

        private int nonOperation;

        public int NonOperation
        {
            get { return nonOperation; }
            set
            {
                NonOperationChanged?.Invoke(this, value);
                nonOperation = value;
            }
        }

        private LoginResult loginResult;

        public LoginResult LoginResult
        {
            get { return loginResult; }
            set
            {
                LoginResultChanged?.Invoke(this, value);

                loginResult = value; 
            }
        }

        private TextBox activeTextBox;
        public TextBox ActiveTextBox
        {
            get { return activeTextBox; }
            set
            {
                if (activeTextBox != null)
                {
                    activeTextBox.BackColor = DEFAULT_TEXTBOX_COLOR;
                }

                if (!(value is null))
                {
                    value.BackColor = SELECTED_TEXTBOX_COLOR;
                }

                activeTextBox = value;
            }
        }

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
                if (!(value is null))
                {
                    value.BackColor = SELECTED_BUTTON_COLOR;
                }

                activeButton = value;
            }
        }

        public Login(MySQLDatabase mySQLDatabase, SettingsJDO settings, ChangePassword changePassword, DataToServerJDO dataToServer)
        {
            InitializeComponent();

            MySQLDatabase = mySQLDatabase;
            Settings = settings;
            ChangePassword = changePassword;
            DataToServer = dataToServer;

            Translator.LanguageChanged += Translate;

            EnableControls(true);
            EnableNonOPControls(true);

            nonOperationsButtons.Add(btnTireShortage, 1);
            nonOperationsButtons.Add(btnSafetyBreak, 2);
            nonOperationsButtons.Add(btnLunchBreak, 3);
            nonOperationsButtons.Add(btnCVError, 4);
            nonOperationsButtons.Add(btnPlanedStop, 5);
            nonOperationsButtons.Add(btnPlantShutdown, 6);

            nonOperationsTextBoxes.Add(1, tbTireShortage);
            nonOperationsTextBoxes.Add(2, tbSafetyBreak);
            nonOperationsTextBoxes.Add(3, tbLunchBreak);
            nonOperationsTextBoxes.Add(4, tbCVError);
            nonOperationsTextBoxes.Add(5, tbPlanedStop);
            nonOperationsTextBoxes.Add(6, tbPlantShutdown);

            NonOperationInformations = new NonOperationInformations();

            //NonOperationTimer.Interval = 1000;
        }

        private void Translate(object sender, Language e)
        {
            if (Translator.Language == Language.CZ)
            {
                lblUserName.Text = "Uživatelské Jméno:";
                lblPassword.Text = "Heslo:";
                btnLogin.Text = "Přihlásit";
                btnLogoff.Text = "Odhlásit";

                ErrorTitle = "Chyba";

                Errors[0] = "Uživatelské jméno musí být číslo. Např. 40156312";
                Errors[1] = "Data o uživately nelze vyhledat v databázi.";
                Errors[2] = "Heslo není správné.";

                MessageTitle = "Zpráva";

                Messages[0] = "Přihlášení uživatele proběhlo v pořádku.";
                Messages[1] = "Je vyžadována změna hesla. Bez změny hesla se již nelze přihlásit.";
            }
            else if (Translator.Language == Language.ENG)
            {
                lblUserName.Text = "User Name:";
                lblPassword.Text = "Password:";
                btnLogin.Text = "Login";
                btnLogoff.Text = "Logoff";

                ErrorTitle = "Error";

                Errors[0] = "User name must be number. Eg. 40156312";
                Errors[1] = "User data was not be search in database.";
                Errors[2] = "Password is not correct.";

                MessageTitle = "Message";

                Messages[0] = "User was be correctly logged.";
                Messages[1] = "Changed password required. Without changing your passwor you will not be able to loggin!";
            }
        }

        private void EnableControls(bool Enable)
        {
            tbUserName.Enabled = Enable;
            tbPassword.Enabled = Enable;
            btnLogin.Enabled = Enable;
            btnLogoff.Enabled = !Enable;
        }

        private void EnableNonOPControls(bool Enable)
        {
            btnTireShortage.Enabled = Enable;
            btnSafetyBreak.Enabled = Enable;
            btnLunchBreak.Enabled = Enable;
            btnCVError.Enabled = Enable;
            btnPlanedStop.Enabled = Enable;
            btnPlantShutdown.Enabled = Enable;
            // Spare Buttons (Disable)
            btnSpare_1.Enabled = false;
            btnSpare_2.Enabled = false;
            btnSpare_3.Enabled = false;
            // --------------------------
            btnCancel.Enabled = Enabled;
            btnConfirnm.Enabled = Enabled;
        }

        public void ClearInputs()
        {
            tbUserName.Text = "";
            tbPassword.Text = "";
        }

        private LoginResult LoginCMD()
        {
            if (TextBoxHelper.TbInputIsNumber(tbUserName) == false)
            {
                CustomMessageBox.ShowPopup(ErrorTitle, Errors[0]);
                return LoginResult.Error;
            }

            UserInformations UserInformations = MySQLDatabase.ReadUserInformation(Settings.DatabaseSettings.UsersTableName, int.Parse(tbUserName.Text));

            if (UserInformations == null)
            {
                CustomMessageBox.ShowPopup(ErrorTitle, Errors[1]);
                return LoginResult.Error;
            }

            if(UserInformations.AskPasswordChanged == 0)
            {
                if (CheckPassword(tbPassword.Text, UserInformations.Password))
                {
                    EnableControls(false);

                    DataToServer.UserInformation.FirstName = UserInformations.NameAndID.FirstName;
                    DataToServer.UserInformation.LastName = UserInformations.NameAndID.LastName;
                    DataToServer.UserInformation.PersonalID = UserInformations.NameAndID.ID;

                    CustomMessageBox.ShowPopup(MessageTitle, Messages[0]);

                    return LoginResult.Logged;
                }
            }
            else
            {
                if (CheckPassword(tbPassword.Text, UserInformations.Password))
                {
                    ChangePassword.ID_ChangePassCMD = tbUserName.Text;
                    ChangePassword.ActualPassword_ChangePassCMD = tbPassword.Text;
                    ClearInputs();

                    CustomMessageBox.ShowPopup(MessageTitle, Messages[1]);

                    return LoginResult.ChangePassword;
                }
            }

            CustomMessageBox.ShowPopup(ErrorTitle, Errors[2]);
            tbPassword.Text = "";
            return LoginResult.Error;
        }

        private bool CheckPassword(string UserInputPassword, string PasswordFromDB)
        {
            if (UserInputPassword == PasswordFromDB)
            {
                return true;
            }

            return false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginResult = LoginCMD();
        }

        private void btnLogoff_Click(object sender, EventArgs e)
        {
            ClearInputs();

            DataToServer.UserInformation.FirstName = "First Name";
            DataToServer.UserInformation.LastName = "Last Name";
            DataToServer.UserInformation.PersonalID = 40180000;

            LoginResult = LoginResult.NoLogged;
            EnableControls(true);
        }

        private void btnNonOperation_Click(object sender, EventArgs e)
        {
            ActiveButton = sender as Button;
            _NonOperation = nonOperationsButtons[ActiveButton];
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (NonOperation == 0) return;

            _NonOperation = 0;
            ActiveButton = null;
            ActiveTextBox = null;
            NonOperation = 0;

            SetStopNonOPInfosAndSendToDB();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (_NonOperation == 0 || _NonOperation > 6 || NonOperation == _NonOperation) return;

            if (NonOperation != 0) OnlyChangeNonOP = true;

            ActiveTextBox = nonOperationsTextBoxes[_NonOperation];
            NonOperation = _NonOperation;

            if (OnlyChangeNonOP)
            {
                SetStopNonOPInfosAndSendToDB();
                OnlyChangeNonOP = false;
            }

            SetStartingNonOPInfos();
        }

        public void SetStartingNonOPInfos()
        {
            NonOperationInformations.IDNonOperation = NonOperation;
            NonOperationInformations.IDUserSelectNonOp = DataToServer.UserInformation.PersonalID;
            NonOperationInformations.IDStation = Settings.StationSettings.StationID;
            NonOperationInformations.StartNonOPDateTime = DateTime.Now;
        }

        public void SetStopNonOPInfosAndSendToDB()
        {
            if (NonOperation == 0) return;

            NonOperationInformations.IDUserClearNonOp = DataToServer.UserInformation.PersonalID;
            NonOperationInformations.StopNonOPDateTime = DateTime.Now;

            NonOperationInformations.NonOperationTime = GetNonOperationTime(NonOperationInformations.StartNonOPDateTime, NonOperationInformations.StopNonOPDateTime);

            MySQLDatabase.WriteNonOperationToDB(Settings.DatabaseSettings.NonOPsDataTableName, NonOperationInformations);
        }

        private string GetNonOperationTime(DateTime StartNonOP, DateTime StopNonOP)
        {
            int TimeMilisecons = TimeToMiliseconds(StopNonOP.ToLongTimeString()) - TimeToMiliseconds(StartNonOP.ToLongTimeString());

            return MilisecondsToStringTime(TimeMilisecons);
        }

        private int TimeToMiliseconds(string Time)
        {
            string[] time = Time.Split(":");

            return (int.Parse(time[0]) * 3600) + (int.Parse(time[1]) * 60) + int.Parse(time[2]);
        }

        private string MilisecondsToStringTime(int Time)
        {
            int _Time = Time;
            string[] time = new string[3];

            time[0] = (_Time / 3600).ToString();
            _Time %= 3600;

            time[1] = (_Time / 60).ToString();
            _Time %= 60;

            time[2] = _Time.ToString();

            return BuildTimeString(time, ':');
        }

        private string BuildTimeString(string[] Time, char Separator)
        {
            return Time[0] + Separator + Time[1] + Separator + Time[2];
        }
    }
}
