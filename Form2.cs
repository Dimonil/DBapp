using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
              
        private static SQLiteConnection DB;
        public Form2()
        {
            InitializeComponent();
           

        }
        public  void getUserForm(int id)
        {
            Hide();
            Staffs user = new Staffs(id);
            Form3 form3 = new Form3();
            form3.getUser(id);
            form3.ShowDialog();
            Close();
        }
        public void getAdminrForm()
        {
            Hide();

            Form1 form1 = new Form1();
            form1.ShowDialog();
            Close();
        }

        private void textLogin_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            Class1 check = new Class1(textLogin.Text, textPass.Text);
          
            if (textLogin.Text == "admin" && textPass.Text == "admin")
            {
                getAdminrForm();
            }
            else
            {
                
                if (check.auth)
                {
                    MessageBox.Show($"Это user {check.id}!") ;
                    getUserForm(check.id);
                    
                }
                

            }
            
            
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            DB = new SQLiteConnection("Data source = DBTest.db");
            DB.Open();
            Class1 conn = new Class1(DB);
            Staffs.staffsConnection(DB);

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            DB.Close();
        }
    }
}
