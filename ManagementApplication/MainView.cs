using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using ZedGraph;

namespace ManagementApplication
{
    public partial class MainView : Form
    {
        public Login login;
        public static SqlConnection conn = new SqlConnection("Server=localhost;Database=testdb;User ID=SA;Password=Password1!");
        public string sql = String.Format("select power,building from admin_acc where username='{0}'", Login.usernow);
        public static string power;
        public static string building;
        public string sno;
        public string idcard;
        public string vbuilding;
        
        public MainView()
        {
            
			InitializeComponent();
           
			textBox1.Text = Login.usernow;
            textBox16.ReadOnly = false;

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();//打开数据库                    
                SqlDataReader reader=cmd.ExecuteReader();//执行sql语句 
                while (reader.Read())  
                {
                    power = String.Format("{0}", reader[0]);
                    building = String.Format("{0}", reader[1]);
                }
                reader.Close();
                conn.Close();//关闭数据库                     
             }


			dgv_refresh();

			switch (power)
            {
                case "C": tabPage6.Parent = null;
                          break;
                case "B": tabPage6.Parent = tabControl1;
                          textBox13.Text = building;
                          sql = String.Format("select username as 'account',password as 'password',building as 'building',power as 'power' from admin_acc where building='{0}'", building);      
                          conn.Open();
                          SqlDataAdapter sd1 = new SqlDataAdapter(sql, conn);
                          DataTable dt1 = new DataTable();
                          sd1.Fill(dt1);
                          
                          dataGridView1.DataSource = dt1;
                          
                          conn.Close();
                          break;
                case "A": tabPage6.Parent = tabControl1;
                          textBox13.Text = "All";
                          sql = String.Format("select username as 'account',password as 'password',building as 'building',power as 'power' from admin_acc");
                          conn.Open();
                          SqlDataAdapter sd2 = new SqlDataAdapter(sql, conn);
						  DataTable dt2 = new DataTable();
					      sd2.Fill(dt2);
						  dataGridView1.DataSource = dt2;
						  conn.Close();
                          break;
                default: break;
            }

            if(building=="")
            { textBox9.Text = "All";
              textBox14.Text = "All";
              textBox15.Text = "All";
            }
            else
            { textBox9.Text = "#"+building;
              textBox14.Text = "#" + building;
              textBox15.Text = "#" + building;
              textBox16.Text = "#" + building;
              textBox16.ReadOnly = true;
            }
        }

 
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                sql = String.Format("insert into student_indor values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", maskedTextBox3.Text, textBox3.Text, comboBox1.Text, textBox4.Text, textBox5.Text, maskedTextBox7.Text, maskedTextBox8.Text, textBox16.Text, maskedTextBox4.Text);
                using (SqlCommand cmd = new SqlCommand(sql.ToString(), conn))
                {
                    conn.Open();//打开数据库                    
                    cmd.ExecuteNonQuery();//执行sql语句 
                    cmd.Dispose();
                    conn.Close();
                }
                MessageBox.Show("successfully add visitor info!", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgv_refresh();
                tabp1_clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
              }
         }

        private void button4_Click(object sender, EventArgs e)
        {
            
                if (building != "")
                {
                    sql = String.Format("insert into visitor values('{0}','{1}','{2}','{3}','{4}','{5}')", maskedTextBox1.Text, textBox6.Text, textBox7.Text, maskedTextBox2.Text, maskedTextBox9.Text, building);
                    SqlCommand cmd = new SqlCommand(sql.ToString(), conn);
                    conn.Open();                   
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                else
                {
                    sql = String.Format("select building from student_indor where sno='{0}'", maskedTextBox2.Text);
                    using (SqlCommand cmd1 = new SqlCommand(sql, conn))
                    {
                        conn.Open();                   
                        SqlDataReader reader = cmd1.ExecuteReader();
                        while (reader.Read())
                        {
                            vbuilding = String.Format("{0}", reader[0]);
                        }
                        reader.Close();
                        conn.Close();
                    }

                    sql = String.Format("insert into visitor values('{0}','{1}','{2}','{3}','{4}','{5}')", maskedTextBox1.Text, textBox6.Text, textBox7.Text, maskedTextBox2.Text, maskedTextBox9.Text, vbuilding);
                    SqlCommand cmd2 = new SqlCommand(sql, conn);
                    conn.Open();
                    cmd2.ExecuteNonQuery();
                    conn.Close();
                    
                }
                dgv_refresh();
                tabp2_clear();
                MessageBox.Show("successfully add visitor info!", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            

         
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string date1 = maskedTextBox10.Text;
            string date2 = maskedTextBox11.Text;
            if (building == "")
            { sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sno as'visited student id',sname as'visited student name',vtime as 'visit time',vbuilding as 'visited building.no' from visitor where vtime between'{0}'and'{1}'",date1, date2); }
            else
            { sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sno as'visited student id',sname as'visited student name',vtime as 'visit time',vbuilding as 'visited building.no' from visitor where vbuilding='{0}'vtime between'{1}'and'{2}'", building, date1, date2); }
            sqlselect(sql, conn, dataGridView4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            if (maskedTextBox5.Text.Length>4)
            {
                string vname = textBox2.Text;
                if (vname.Length>4)
                { if(building=="")
                  {sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sno as'visited student id',sname as'visited student name',vtime as 'visit time',vbuilding as 'visited building.no' from visitor where vname='{0}'", vname); }
                  else
                  {sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sno as'visited student id',sname as'visited student name',vtime as 'visit time',vbuilding as 'visited building.no' from visitor where vname='{0}'and vbuilding='{1}'", vname,building); }}
                else 
                { if(building=="")
                  {sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sno as'visited student id',sname as'visited student name',vtime as 'visit time' from visitor where vname like'{0}'", vname+"%"); }
                  else
                  {sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sno as'visited student id',sname as'visited student name',vtime as 'visit time' from visitor where vbuilding='{0}'and vname like'{1}'", vname+"%",building); }
                
            }
            }
            else
            {
                string idcard = maskedTextBox5.Text;
                if(building=="")
                {sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sno as'visited student id',sname as'visited student name',vtime as 'visit time',vbuilding as 'visited building.no' from visitor where idcard='{0}'", idcard);}
                else
                {sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sno as'visited student id',sname as'visited student name',vtime as 'visit time',vbuilding as 'visited building.no' from visitor where idcard='{0}'and vbuilding", idcard,building);}
            }
            sqlselect(sql, conn, dataGridView5);
            }


        private void button7_Click(object sender, EventArgs e)
        {
            if (maskedTextBox6.Text == "")
            {
                string sname = textBox8.Text;
                if (sname.Length < 4)
                {
                    if (building == "")
                    { sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sno as'visited student id',sname as'visited student name',vtime as 'visit time',vbuilding as 'visited building.no' from visitor where sname like'{0}'", sname + "%"); }
                    else
                    { sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sno as'visited student id',sname as'visited student name',vtime as 'visit time',vbuilding as 'visited building.no' from visitor where sname like'{0}'and vbuilding='{1}'", sname + "%", building); }
                }
                else
                {
                    if (building == "")
                    { sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sno as'visited student id',sname as'visited student name',vtime as 'visit time',vbuilding as 'visited building.no' from visitor where sname ='{0}'", sname); }
                    else
                    { sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sno as'visited student id',sname as'visited student name',vtime as 'visit time',vbuilding as 'visited building.no' from visitor where sname ='{0}'and vbuilding='{1}'", sname, building); }
                }
            }
            else
            {
                string sno = maskedTextBox6.Text;
                if (building == "")
                { sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sno as'visited student id',sname as'visited student name',vtime as 'visit time',vbuilding as 'visited building.no' from visitor where sno ='{0}'", sno); }
                else
                {
                    sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sno as'visited student id',sname as'visited student name',vtime as 'visit time',vbuilding as 'visited building.no' from visitor where sno ='{0}'and vbuilding='{1}'", sno, building);
                }
            }
			sqlselect(sql, conn, dataGridView6);
			//Console.WriteLine(sql);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            
            this.login.Show();
            this.Close();
            this.Dispose();

            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (building == "")
            { sql= "select count(*) from student_indor"; }
            else
            { sql= String.Format("select count(*) from student_indor where building='{0}'", building); }
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();//打开数据库                    
                SqlDataReader reader = cmd.ExecuteReader();//执行sql语句 
                while (reader.Read())
                {
                    textBox10.Text = String.Format("{0}", reader[0]);
                 }
                reader.Close();
                conn.Close();//关闭数据库                     
            }

            if (building == "")
            { sql= "select count(*) from visitor"; }
            else
            { sql = String.Format("select count(*) from visitor where vbuilding='{0}'", building); }
            
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();//打开数据库                    
                SqlDataReader reader = cmd.ExecuteReader();//执行sql语句 
                while (reader.Read())
                {
                    textBox11.Text = String.Format("{0}", reader[0]);
                }
                reader.Close();
                conn.Close();//关闭数据库                     
            }

            string now = DateTime.Now.ToShortDateString();
            string start = Convert.ToDateTime(now).ToString("yyyy-MM-01");
            if (building == "")
            { sql=String.Format("select count(*) from visitor where vtime between '{0}'and'{1}'",start,now); }
            else
            { sql = String.Format("select count(*) from visitor where vbuilding='{0}' and vtime between '{1}'and'{2}'", building,start,now); }
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();//打开数据库                    
                SqlDataReader reader = cmd.ExecuteReader();//执行sql语句 
                while (reader.Read())
                {
                    textBox12.Text = String.Format("{0}", reader[0]);
                }
                reader.Close();
                conn.Close();//关闭数据库                     
            }
            

            // get a reference to the GraphPane
            GraphPane myPane1 = zgc1.GraphPane;
			// Set the Titles
			myPane1.Title.Text = "visit of this year";
            myPane1.XAxis.Title.Text = "month";
            myPane1.YAxis.Title.Text = "total visit";

            // Make up some data arrays based on the Sine function
            double x;
            double y;
            PointPairList list1 = new PointPairList();
			myPane1.XAxis.Scale.Min = 1;
			myPane1.XAxis.Scale.Max = 12;//http://www.cnblogs.com/gywei/p/3340827.html
            myPane1.XAxis.Scale.MajorStepAuto = true;
            string month =DateTime.Now.Year.ToString() + "-01-01";
            int i = 1;
			string monthend;
			string end = DateTime.Now.AddMonths(1).ToString("yyyy-MM-01");
            while(month!=end)
            {
				x = i;
				if (i != 12)
				{ monthend = String.Format("{0:D2}-{1:D2}-01", DateTime.Now.Year.ToString(), i + 1); }
				else
				{ monthend = DateTime.Now.AddMonths(1).ToString("yyyy-MM-01"); }

                if (building == "")
                { sql=String.Format("select count(*) from visitor where vtime between'{0}'and'{1}'",month,monthend); }
                else
                { sql=String.Format("select count(*) from visitor where vbuilding='{0}' and vtime between'{1}'and'{2}' ", building,month,monthend); }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
					conn.Open();            
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
						y = double.Parse(String.Format("{0}", reader[0]));
                        list1.Add(x, y);
						Console.WriteLine(sql);
						Console.WriteLine(x);
						Console.WriteLine(y);
                    }
                reader.Close();
                conn.Close();             
                }
                i++;
                month = monthend;
            }
                
            LineItem myCurve = myPane1.AddCurve("", list1, Color.Blue, SymbolType.Circle);
            zgc1.AxisChange();
            zgc1.Invalidate(); //update the graph
        }

        public void sqlselect(string arg1, SqlConnection arg2, DataGridView arg3)
        {
			arg2.Open();
            SqlDataAdapter sd = new SqlDataAdapter(arg1,arg2);
			DataTable dt = new DataTable();
            sd.Fill(dt);
            DataTable ndt = new DataTable();
            DataColumn dc = new DataColumn();
            dc.ColumnName = "sequence";
            dc.AutoIncrement = true;
            dc.AutoIncrementSeed = 1;
            dc.AutoIncrementStep = 1;
            ndt.Columns.Add(dc);
			ndt.Merge(dt);
			arg3.DataSource = ndt;
            arg2.Close();

		}

        public void dgv_refresh()
        {
            dataGridView2.DataSource = null; 
            dataGridView3.DataSource = null;   
            if(power=="A")
            {sql = String.Format("select sno as'student id',sname as 'student name',sex as'gender',school as'school',major as 'major',stime as 'when enroll',ltime as 'when check-in',building as 'building.no',dor as 'dormitory' from student_indor");
            sqlselect(sql, conn, dataGridView2);

            sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sname as'visited student name',sno as'visited student id',vtime as 'visit time',vbuilding as 'visited building.no' from visitor");
            sqlselect(sql, conn, dataGridView3);}
            else
            {sql = String.Format("select sno as'student id',sname as 'student name',sex as'gender',school as'school',major as 'major',stime as 'when enroll',ltime as 'when check-in',building as 'building.no',dor as 'dormitory' from student_indor where building='{0}'",building);
            sqlselect(sql, conn, dataGridView2);

            sql = String.Format("select idcard as'visitor id',vname as 'visitor name',sname as'visited student name',sno as'visited student id',vtime as 'visit time',vbuilding as 'visited building.no' from visitor where vbuilding='{0}'", building);
            sqlselect(sql, conn, dataGridView3);
             }
            }

        private void button1_Click(object sender, EventArgs e)
        {
            string sname = textBox7.Text;
            sql = String.Format("select count(*) from student_indor where sname = '{0}' ", sname);
            int a = DBHelper.GetSqlResult(sql);
            if (a < 1)
            {
                MessageBox.Show("no such student!", "检测提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("student exist!", "检测提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sno = maskedTextBox2.Text;
            string sname = textBox7.Text;
            sql = String.Format("select count(*) from student_indor where sname = '{0}'and sno='{1}' ", sname,sno);
            int a = DBHelper.GetSqlResult(sql);
            if (a < 1)
            {
                MessageBox.Show("student id not match", "检测提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("student id match", "检测提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DataSet dt = new DataSet();
            dt.Tables.Add((DataTable)dataGridView1.DataSource);
            sql = String.Format("select * from admin_acc");
            conn.Open();
            SqlDataAdapter sd= new SqlDataAdapter(sql, conn);
            sd.Update(dt);
            SqlCommandBuilder buder = new SqlCommandBuilder(sd);   //用于生成SQL语句
            sd.Update(dt); 
            conn.Close();

        }

        private void dgv1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string username = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string password=dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            if(building=="")
            {building = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();}
            string power = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            Admin_acc adc = new Admin_acc();
            adc.Show();
            adc.textU.Text = username;
            adc.username = username;
            adc.textP.Text = password;
            
            adc.textB.Text = building;
            adc.textB.ReadOnly = true;
            adc.textPR.Text = power;
            adc.button1.Visible = true;
            adc.button2.Visible = true;
            adc.button3.Visible = false;
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            
                Admin_acc adc = new Admin_acc();
                adc.Show();
                adc.button1.Visible = false;
                adc.button2.Visible = false;
                adc.button3.Visible = true;
                if (power == "B")
                {
                    adc.textB.Text = building;
                    adc.textB.ReadOnly = true;
                }
            
        }

       private void dgv2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
       {
           //tabPage6.Parent = null;
           //tabPage2.Parent = null;
			tabControl1.SelectedTab = tabPage1; // replace modifing parent
			tabPage6.Enabled = false;
			tabPage2.Enabled = false;

           button3.Visible = false;
           button11.Visible = true;
           button12.Visible = true;
           sno= dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
           maskedTextBox3.Text = sno;
           textBox3.Text = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString(); 
           comboBox1.Text =dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
           textBox4.Text = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();
           textBox5.Text = dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString();
           maskedTextBox7.Text = dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString();
           maskedTextBox8.Text = dataGridView2.Rows[e.RowIndex].Cells[7].Value.ToString();
           textBox16.Text = dataGridView2.Rows[e.RowIndex].Cells[8].Value.ToString();
           maskedTextBox4.Text = dataGridView2.Rows[e.RowIndex].Cells[9].Value.ToString();

       }

       private void dgv3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
       {
			//tabPage6.Parent = null;
			//tabPage1.Parent = null			
			tabControl1.SelectedTab = tabPage2; // replace modifing parent.
			tabPage6.Enabled = false;
			tabPage1.Enabled = false;

           	button4.Visible = false;             //button4.Enabled = false;
			button13.Visible = true;
           	button14.Visible = true;

           idcard = dataGridView3.Rows[e.RowIndex].Cells[1].Value.ToString();
           maskedTextBox1.Text = idcard;
           textBox6.Text = dataGridView3.Rows[e.RowIndex].Cells[2].Value.ToString();
           textBox7.Text = dataGridView3.Rows[e.RowIndex].Cells[3].Value.ToString();
           maskedTextBox2.Text = dataGridView3.Rows[e.RowIndex].Cells[4].Value.ToString();
           maskedTextBox9.Text= dataGridView3.Rows[e.RowIndex].Cells[5].Value.ToString();

        }

       private void tabp1_clear()
       {
           maskedTextBox3.Text = "";
           textBox3.Text = "";
           comboBox1.Text = "";
           textBox4.Text = "";
           textBox5.Text = "";
           maskedTextBox7.Text = "";
           maskedTextBox8.Text = "";
           textBox16.Text = "";
           maskedTextBox4.Text = "";
       }

      private void tabp2_clear()
       {
           maskedTextBox1.Text = "";
           textBox6.Text = "";
           textBox7.Text = "";
           
           maskedTextBox2.Text = "";
           maskedTextBox9.Text = "";

       }

      private void button11_Click(object sender, EventArgs e)
      {
          sql = String.Format("update student_indor set sno='{0}',sname='{1}',sex='{2}',school='{3}',major='{4}',stime='{5}',ltime='{6}',building='{7}',dor='{8}' where sno='{9}'", maskedTextBox3.Text, textBox3.Text, comboBox1.Text, textBox4.Text, textBox5.Text, DateTime.Parse(maskedTextBox7.Text), DateTime.Parse(maskedTextBox8.Text), textBox16.Text, maskedTextBox4.Text, sno);
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              conn.Open();
              cmd.ExecuteNonQuery();
              conn.Close();
          }
          //tabPage2.Parent = tabControl1;
          //tabPage6.Parent = tabControl1;
			tabPage6.Enabled = true;
			tabPage2.Enabled = true;

          button3.Visible = true;
          button11.Visible = false;
          button12.Visible = false;
          tabp1_clear();
          dgv_refresh();
      }

      private void button12_Click(object sender, EventArgs e)
      {
          string sql = String.Format("delete from student_indor where sno='{0}'", maskedTextBox3.Text);
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              conn.Open();
              cmd.ExecuteNonQuery();
              conn.Close();
          }
          //tabPage2.Parent = tabControl1;
          //tabPage6.Parent = tabControl1;
			tabPage6.Enabled = true;
			tabPage2.Enabled = true;
          
          button3.Visible = true;
          button11.Visible = false;
          button12.Visible = false;
          tabp1_clear();
          dgv_refresh();
      }

      private void button14_Click(object sender, EventArgs e)
      {
          if (building != "")
          {
              sql = String.Format("update visitor set idcard='{0}',vname='{1}',sname='{2}',sno='{3}',vtime='{4}',vbuilding='{5}' where idcard='{6}'", maskedTextBox1.Text, textBox6.Text, textBox7.Text, maskedTextBox2.Text, maskedTextBox9.Text, building, idcard);
          }
          else
          {
				sql = String.Format("select building from student_indor where sno='{0}'", maskedTextBox2.Text);
              SqlDataAdapter sd = new SqlDataAdapter(sql, conn);
              DataTable dt = new DataTable();
              sd.Fill(dt);
              vbuilding = (string)dt.Rows[0][0];
              sql = String.Format("update visitor set idcard='{0}',vname='{1}',sname='{2}',sno='{3}',vtime='{4}',vbuilding='{5}' where idcard='{6}'", maskedTextBox1.Text, textBox6.Text, textBox7.Text, maskedTextBox2.Text, maskedTextBox9.Text, vbuilding, idcard);
          }
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              conn.Open();
              cmd.ExecuteNonQuery();
              conn.Close();
          }
          //tabPage1.Parent = tabControl1;
          //tabPage6.Parent = tabControl1;
			tabPage6.Enabled = true;
			tabPage1.Enabled = true;

          button4.Visible = true;
          button13.Visible = false;
          button14.Visible = false;
          tabp2_clear();
          dgv_refresh();
      }

      private void button13_Click(object sender, EventArgs e)
      {
          sql = String.Format("delete from visitor where idcard='{0}'", maskedTextBox1.Text);
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              conn.Open();
              cmd.ExecuteNonQuery();
              conn.Close();
          }
          //tabPage1.Parent = tabControl1;
          //tabPage6.Parent = tabControl1;
			tabPage6.Enabled = true;
			tabPage1.Enabled = true;

          button4.Visible = true;
          button13.Visible = false;
          button14.Visible = false;
          tabp2_clear();
          dgv_refresh();
      }

      private void button16_Click(object sender, EventArgs e)
      {
          if (building != "")
          {
              sql = String.Format("select username as 'account',password as 'password',building as 'building',power as 'power' from admin_acc where building='{0}'", building);
              conn.Open();
              SqlDataAdapter sd1 = new SqlDataAdapter(sql, conn);
              DataTable dt1 = new DataTable();
              sd1.Fill(dt1);
              dataGridView1.DataSource = dt1;
              conn.Close();
          }
          else
          {
              sql = String.Format("select username as 'account',password as 'password',building as 'building',power as 'power' from admin_acc");
              conn.Open();

              SqlDataAdapter sd2 = new SqlDataAdapter(sql, conn);
              DataTable dt2 = new DataTable();
              sd2.Fill(dt2);
              dataGridView1.DataSource = dt2;
              conn.Close();

          }
      }

		private void fuzzy_search_start_Click(object sender, EventArgs e)
		{
			MessageBox.Show("This function is ongoing! :)");
		}

  }
}
