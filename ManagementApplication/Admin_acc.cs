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
    public partial class Admin_acc : Form
    {
        public string username;
        public Admin_acc()
        {
            InitializeComponent();
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            if (MainView.power != "A" && textPR.Text == "A")
            {
                MessageBox.Show("You don't have permission!", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {string sql = String.Format("update admin_acc set username='{0}',password='{1}',building='{2}',power='{3} 'where username='{4}'", textU.Text, textP.Text, textB.Text, textPR.Text, username);
            using (SqlCommand cmd = new SqlCommand(sql.ToString(), MainView.conn))
            {
                MainView.conn.Open();                  
                cmd.ExecuteNonQuery(); 
                MainView.conn.Close();   
             }
            this.Close();
            this.Dispose();
        }}

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = String.Format("delete from admin_acc where username='{0}'", textU.Text);
            using (SqlCommand cmd = new SqlCommand(sql.ToString(), MainView.conn))
            {
                MainView.conn.Open();                   
                cmd.ExecuteNonQuery(); 
                MainView.conn.Close();                    

            }
            this.Close();
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (MainView.power == "B" && textPR.Text == "A")
            { MessageBox.Show("You don't have permission!", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            else
            {
                string sql = String.Format("insert into admin_acc values('{0}','{1}','{2}','{3}')", textU.Text, textP.Text, textB.Text, textPR.Text);

                using (SqlCommand cmd = new SqlCommand(sql.ToString(), MainView.conn))
                {
                    MainView.conn.Open();
                    cmd.ExecuteNonQuery();
                    MainView.conn.Close();

                }
            }
            this.Close();
            this.Dispose();
        }

        /*public void dgv1_refresh(string args)
        {
            
        }*/
    }
}
