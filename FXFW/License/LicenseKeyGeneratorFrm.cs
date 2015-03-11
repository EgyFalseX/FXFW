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
    public partial class LicenseKeyGeneratorFrm : Form
    {
        private static string PasswordKey = "ByTheNameOfAllah";
        #region -   Functions   -
        public LicenseKeyGeneratorFrm()
        {
            InitializeComponent();
            tbID.Text = CUPID();
        }
        public static string CUPID()
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
        #endregion
        #region -   Event Handlers   -

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbID.Text.Trim() == string.Empty)
            {
                tbKey.Text = string.Empty;
                return;
            }
            tbKey.Text = LncKey(tbID.Text + tbName.Text);
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            using (StreamWriter sw = new StreamWriter(FBD.SelectedPath + @"\lnc.key"))
            {
                sw.Write(tbKey.Text);
                sw.Close();
            }
        }
        public static string LncKey(string IdData)
        {
            return EncDec.Encrypt(IdData, PasswordKey);
        }
        public static string XXX()
        {
            return EncDec.Decrypt("u9UVX+L7ww+fLlEOoEUoBlF0xoqi3eLSuDhgakL+fwE68DOkXFle88rhEIxSNYb7", "ByTheNameOfAllah");
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
    
    }
}
