using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ManagementApplication
{
    public partial class Login : Form
    {
        public static string usernow=null;
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //标识是否为合法用户
            bool isValidUser = false;
            string message = "错误提示";
            usernow = textBox1.Text;

            if (IsValidataInput())
            {
                //验证用户是否为合法用户
                isValidUser = IsValidataUser(textBox1.Text.Trim(), textBox2.Text, ref message);
                if (isValidUser)
                {
                    //MessageBox.Show("jugg work!", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

					MainView ma = new MainView();

					ma.Show();
                    ma.login = this;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("This user doesn't exist or incorrect password!", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
        }
        
        
        private bool IsValidataInput()
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Please input username!", "登陆提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Focus();
                return false;
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Please input password！", "登陆提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox2.Focus();
                return false;
            }
            return true;
        }
        

        
        //传递用户账号、密码、登陆类型,合法返回true,不合法返回false
        
        private bool IsValidataUser(string UserName, string UserPwd, ref string message)
        {
            string sql = String.Format("select count(*) from admin_acc where username = '{0}' and password = '{1}'", UserName, UserPwd);

            int a = DBHelper.GetSqlResult(sql);
           
            if (a < 1)
            {
                message = "This user doesn't exist or incorrect password!";
                return false;
            }
            else
            {
                
                return true;
            }
          }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Copyright cr = new Copyright();
            cr.Show();
        }
          
    }
}
