using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        Staffs user;
        public Form3()
        {
            InitializeComponent();
            
        }
        public void getUser(int id)
        {
            user = new Staffs(id);

        }
       

        private void buttonCalc_Click(object sender, EventArgs e)//Рассчет
        {
            try
            {
                if (!Staffs.inId(Convert.ToInt32(comboBoxSub.Text)))
                    MessageBox.Show("Такого ID не существует");
                if (comboBoxSub.Text != "" && Staffs.inId(Convert.ToInt32(comboBoxSub.Text)))
                {

                    Staffs.getDate(dateTimePickerCalc.Value);

                    int id = Convert.ToInt32(comboBoxSub.Text);
                    Staffs man = new Staffs(id);

                    MessageBox.Show($" Зарплата на {Staffs.dateZp} = {man.count().all} \n");


                }
            }
            catch
            {
                MessageBox.Show($"Неверный ID");
            }
        }

        private void button1_Click(object sender, EventArgs e)//список подчинненых
        {
            try
            {
                Staffs.getDate(dateTimePickerCalc.Value);
                int id = user.id;
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

        private void Form3_Load(object sender, EventArgs e)
        {
            comboBoxSub.Items.Add(user.id);
            foreach (int item in user.subs)
            {
                comboBoxSub.Items.Add(item);
            }
        }
    }
}
