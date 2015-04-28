using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.IO;

namespace FXFW.License
{
    public partial class LicenseKeyFrm : Form
    {
        private static string PasswordKey = "ByTheNameOfAllah";
        private string AppName = string.Empty;
        #region -   Functions   -
        public LicenseKeyFrm(string ApplicationName)
        {
            InitializeComponent();
            AppName = ApplicationName;
        }
        public static string CUPID_Deleted()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            using (ManagementObjectCollection moc = mc.GetInstances())
            {
                foreach (ManagementObject mo in moc)
                {
                    if (cpuInfo == "")
                    {
                        //Get only the first CPU's ID
                        cpuInfo = mo.Properties["processorID"].Value.ToString();
                        break;
                    }
                }
            }
            return cpuInfo;
        }
        public static bool Compare(string Data, string ApplicationName)
        {
            try
            {
                if (EncDec.Decrypt(Data, PasswordKey) == BiosId() + ApplicationName)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error",  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
        public static bool Compare2(string Data, string ApplicationName)
        {
            try
            {
                string Data2 = EncDec.Encrypt(BiosId() + ApplicationName, PasswordKey);
                if (Data == Data2)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
        public static bool CompareBios(string Data, string ApplicationName)
        {
            try
            {
                string Data2 = EncDec.Encrypt(BiosId() + ApplicationName, PasswordKey);
                if (Data == Data2)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
        //Return a hardware identifier
        public static string Identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementBaseObject mo in moc)
            {
                if (mo[wmiMustBeTrue].ToString() != "True") continue;
                //Only get the first one
                if (result != "") continue;
                try
                {
                    result = mo[wmiProperty].ToString();
                    break;
                }
                catch
                {
                }
            }
            return result;
        }
        //Return a hardware identifier
        public static string Identifier(string wmiClass, string wmiProperty)
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementBaseObject mo in moc)
            {
                //Only get the first one
                if (result != "") continue;
                try
                {
                    result = mo[wmiProperty].ToString();
                    break;
                }
                catch { }
            }
            return result;
        }
        public static string BiosId()
        {
            return Identifier("Win32_BIOS", "Manufacturer") + Identifier("Win32_BIOS", "SMBIOSBIOSVersion") + Identifier("Win32_BIOS", "IdentificationCode") + Identifier("Win32_BIOS", "SerialNumber") + Identifier("Win32_BIOS", "ReleaseDate") + Identifier("Win32_BIOS", "Version");
        }
        #endregion
        #region -   Event Handlers   -
        private void LicenseKeyFrm_Load(object sender, EventArgs e)
        {
            //EncDec.Encrypt(CUPID(), PasswordKey);
            tbID.Text = BiosId();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "LicenseKey(*.key)|*.key|All Files(*.*)|*.*" };
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            StreamReader sr = new StreamReader(ofd.FileName);
            string KeyString = sr.ReadToEnd();
            sr.Close();


            if (KeyString == EncDec.Encrypt(BiosId() + AppName, PasswordKey))
            {
                File.Copy(ofd.FileName, String.Format("{0}\\{1}", Application.StartupPath, ofd.SafeFileName));
                Close();
            }
            else
                MessageBox.Show("Key is wrong", "error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }
}
