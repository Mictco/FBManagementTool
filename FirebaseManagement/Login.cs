using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirebaseManagement
{
    public partial class Login : Form
    {
        #region Constructor
        public Login()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        private string PasswordfindMethod()
        {
            string sDay = "", sMonth = "";
            int iYear = DateTime.Now.Year, iMonth = DateTime.Now.Month;
            sMonth = iMonth <= 9 ? "0" + iMonth.ToString() : iMonth.ToString();
            int iDayofMonth = DateTime.Now.Day;
            sDay = iDayofMonth <= 9 ? "0" + iDayofMonth.ToString() : iDayofMonth.ToString();
            return sDay + sMonth + iYear.ToString();
        }
        #endregion

        #region Events
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string sUserName = "Admin";
            if(txtUserName.Text.ToLower()== sUserName.ToLower() && txtPassword.Text==PasswordfindMethod())
            {
                FB_Management frm = new FB_Management();
                this.Hide();
                frm.Show();
            }
            else
            {
                MessageBox.Show("The Password you entered is not correct.");
                txtPassword.Focus();
                txtPassword.SelectAll();
            }
        }
        #endregion
    }
}
