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
using System.Data.Sql;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        public SQLiteConnection DB;
        

        public Form1()
        {
            
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DB = new SQLiteConnection("Data source = DBTest.db");
            DB.Open();
            Staffs.staffsConnection(DB);                       
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DB.Close();
        }

        private void button2_Click(object sender, EventArgs e)//Добавление человека
        {


            //try
            //{

                int id;
            if (textIdnAdd.Text == "") id = -1;
            else id = Convert.ToInt32(textIdnAdd.Text);
                Staffs.insertStaff(textName.Text, dateTimePickerAdd.Value, Convert.ToInt32(textGroupAdd.Text), id,textLogin.Text, textPass.Text);                
                MessageBox.Show($"Сотрудник {textName.Text} добавлен");
            //}
            //catch
            //{               
                //MessageBox.Show("Ошибка добавления");
            //}
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Staffs.inId(Convert.ToInt32(textId.Text)))
                    MessageBox.Show("Такого ID не существует");
                if (textId.Text != "" && Staffs.inId(Convert.ToInt32(textId.Text)))
                {

                    Staffs.getDate(dateTimePicker1.Value);

                    int id = Convert.ToInt32(textId.Text);
                    Staffs man = new Staffs(id);

                    MessageBox.Show($"{man.count().all} \n");


                }
            }
            catch
            {
                MessageBox.Show($"Неверный ID");
            }
            
           
                       

        }
        
       
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Staffs.dateZp = dateTimePicker1.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Staffs.getDate(dateTimePicker1.Value);               
                int id = Convert.ToInt32(textId.Text);
                Staffs man = new Staffs(id);
                string s = "";
                foreach (int item in man.subs)
                {
                    s += $"{item}\n";
                }
                MessageBox.Show(s);

            }
            catch
            {
                MessageBox.Show("Пользователь не найден");
            }
            
        }

        private void buttonCalcAll_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Общая зарплата : {Staffs.getTopCount().ToString()}");
        }
    }
}
