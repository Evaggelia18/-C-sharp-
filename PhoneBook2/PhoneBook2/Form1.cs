using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace PhoneBook2
{
    public partial class Form1 : Form
    {
        String conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Database1.mdb";
        OleDbConnection connect;
        bool search_res;
        int fl = 0;
        public string music = " ";
        public string photo = " ";
        DateTime datetime = DateTime.Now;
        WindowsMediaPlayer muse = new WindowsMediaPlayer();
        int min = 0;
        string current;
        

        public Form1()
        {
            InitializeComponent();
            timer1.Start();
        }

        //Function That Returns True If A Contact Or Name Exists And False If It Doesnt Exist
        private bool Search()
        {
            listBox1.Items.Clear();
            int x = 0;
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter("select * from Table1", connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Table1");
                foreach (DataRow dataRow in ds.Tables[0].Rows)
                {
                    if (dataRow["FName"].ToString() == textBox1.Text && dataRow["LName"].ToString() == textBox2.Text)
                    {
                        x++;
                    }

                    if ( (dataRow["BirthDay1"].ToString()).Contains(current) && min == 1 )
                    {
                        MessageBox.Show("Happy BirthDay" + dataRow["FName"].ToString() + dataRow["LName"].ToString());
                    }
                    min = 0;
                }
                
            return (x == 0);
        }

        //This Function Adds All The Contacts To The ComboBox So The User Can Pick The Contact He Wants To Edit
        private bool Combo()
        {
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter("select * from Table1", connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Table1");
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                comboBox1.Items.Add(dataRow["FirstLastN"].ToString());
            }
            return true;
        }

        //Submit Contact
        private void button1_Click(object sender, EventArgs e)
        {
            
            //Checks If textbox3 ,Which Contains The Phone Number ,Contains Numbers Or Letters
            Regex regex = new Regex("^[a-zA-Z]+$");
            bool abc = regex.IsMatch(textBox3.Text);
            search_res = Search();

            if (!String.IsNullOrEmpty(textBox1.Text) && !String.IsNullOrEmpty(textBox2.Text) && !String.IsNullOrEmpty(textBox3.Text) && abc == false && music != " " && photo != " ")
            {
                if (search_res == true)
                {
                    connect.Open();
                    string x = textBox1.Text + textBox2.Text;
                    String query = "insert into Table1(FName,LName,Phone,Address,BirthDay1,EMail1,FirstLastN,Pic,Music)"
                        + "values('" + textBox1.Text + "','" + textBox2.Text + "','" + Convert.ToInt64(textBox3.Text) + "','" + textBox5.Text + "','" + dateTimePicker1.Text + "','" + textBox4.Text + "','" + x + "','" + photo + "','" + music + "')";

                    
                    OleDbCommand command = new OleDbCommand(query, connect);
                    int count = command.ExecuteNonQuery();
                    connect.Close();
                    if (fl == 0)
                    {
                        MessageBox.Show(count.ToString() + " Contact Succesfully Submited!", "Contact Submitted");
                        fl = 0;
                    }
                    else
                    {
                        MessageBox.Show(count.ToString() + " Contact Succesfully Edited!", "Contact Edit");
                    }
                    textBox1.Text = string.Empty;
                    textBox2.Text = string.Empty;
                    textBox3.Text = string.Empty;
                    textBox4.Text = string.Empty;
                    textBox5.Text = string.Empty;
                    dateTimePicker1.Text = string.Empty;
                    pictureBox1.Image = null;
                }
                else
                {
                    MessageBox.Show("That Name Already Exists.Please Choose Another One");
                    textBox1.Text = string.Empty;
                    textBox2.Text = string.Empty;
                }
            }
            else if(String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrEmpty(textBox2.Text) || String.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Please Enter The First And Last Name As Well As The Phone Number Of The Contact You Are Processing.","Error");
            }
            else if(music == " " || photo == " ")
            {
                MessageBox.Show("Please Type Both The Ringtone And The Profil Picture For The Contact You Wish To Process.", "Error");
            }
            else
                {
                    MessageBox.Show("Please Type Only Numbers On The Phone Text Box.","Error");
                textBox3.Text = string.Empty;
            }
            comboBox1.Items.Clear();
            Combo();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            connect = new OleDbConnection(conString);
            Combo();

            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            current = datevalue.Month.ToString() + "/" + datevalue.Day.ToString();
            min = 1;
            Search();

        }


        //Search Contact
        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            int x = 0;
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter("select * from Table1", connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Table1");
            if (String.IsNullOrEmpty(textBox1.Text) && String.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please Enter The First And Last Name Of The Contact You Wish To Search.","Error");
            }
            else
            {
                foreach (DataRow dataRow in ds.Tables[0].Rows)
                {
                    if (dataRow["FName"].ToString() == textBox1.Text && dataRow["LName"].ToString() == textBox2.Text)
                    {
                        listBox1.Items.Add(dataRow["FName"].ToString() + " || " + dataRow["LName"].ToString() + " || " + dataRow["Phone"].ToString()
                            + " || " + dataRow["Address"].ToString() + " || " + dataRow["BirthDay1"].ToString() + " || " + dataRow["EMail1"].ToString());

                        x++;
                    }
                }
                if(x == 0)
                {
                    listBox1.Items.Add("No Such Contact Was Found");
                }
            }
        }

        //Show All Contacts
        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter("select * from Table1", connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Table1");
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                listBox1.Items.Add(dataRow["FName"].ToString() + " || " + dataRow["LName"].ToString() + " || " + dataRow["Phone"].ToString()
                + " || " + dataRow["Address"].ToString() + " || " + dataRow["BirthDay1"].ToString() + " || " + dataRow["EMail1"].ToString());
            }
        }

        //Delete All Contact
        private void button4_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are You Sure You Want To Delete All Of Your Contacts?", "Delete All Contact", buttons);
            if (result == DialogResult.Yes)
            {
                connect.Open();
                OleDbCommand cmd = new OleDbCommand("DELETE * FROM Table1", connect);
                cmd.ExecuteNonQuery();
                connect.Close();
            }
            comboBox1.Items.Clear();
            Combo();
        }

        //Application Exit
        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Delete A Single Contact
        private void button6_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text) && String.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please Enter The First And Last Name Of The Contact You Wish To Search.", "Error");
                search_res = Search();
            }
            else
            {
                search_res = Search();
                
                    connect.Open();
                    OleDbCommand deleteCmd = new OleDbCommand("delete from Table1 where FirstLastN = @fln", connect);
                    deleteCmd.Parameters.AddWithValue("@fln", textBox1.Text + textBox2.Text);
                    deleteCmd.ExecuteNonQuery();
                    connect.Close();
                
            }

            //Checks To See If The Contact Exists Or Not And Displays A Message That It Was Deleted
            if (fl == 0)
            {
                if (search_res == true)
                {
                    listBox1.Items.Add("No Such Contact Was Found");
                }
                else
                {
                    MessageBox.Show("Contact Succesfully Deleted", "Contact Deleted");
                }
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                fl = 0;
            }
            //Since This Function Deletes A Contact,The Combo Function Is Called To Refresh The Contacts In The Combobox
            comboBox1.Items.Clear();
            Combo();
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter("select * from Table1", connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Table1");
            
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                if (dataRow["FirstLastN"].ToString() == comboBox1.Text )
                {
                    textBox1.Text = dataRow["FName"].ToString();
                    textBox2.Text = dataRow["LName"].ToString();
                    textBox3.Text = dataRow["Phone"].ToString();
                    textBox4.Text = dataRow["EMail1"].ToString();
                    textBox5.Text = dataRow["Address"].ToString();
                    dateTimePicker1.Text = dataRow["BirthDay1"].ToString();
                    if (dataRow["Pic"].ToString() != "")
                    {
                        pictureBox1.Image = Image.FromFile(@dataRow["Pic"].ToString());
                    }
                    if (dataRow["Music"].ToString() != "")
                    {
                        music = dataRow["Music"].ToString();
                        muse.URL = @music;
                        muse.controls.play();
                    }
                }
            }
            //--------
            fl = 1;
            button6.PerformClick();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            fl = 1;
            button1.PerformClick();
        }

        //The User Can Select His Ringtone
        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All audio filters(*.mp3)|*.mp3|(*.wav)|*.wav|(*.flac)|*.flac| All Files(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                music = dialog.FileName;
                MessageBox.Show(music);
            }
        }

        //The User Can Select His Profil Picture
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All image filters(*.jpeg)|*.jpeg|(*.png)|*.png|(*.jpg)|*.jpg| All Files(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                photo = dialog.FileName;
                pictureBox1.Image = Image.FromFile(@photo);
                MessageBox.Show(photo);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label3.Text = datetime.ToString();
        }
    }
}
