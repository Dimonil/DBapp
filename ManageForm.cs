using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class ManageForm 
    {
        Form1 form1 = new Form1();
        Form2 form2 = new Form2();       
            
        public ManageForm()
        {
            Application.Run(form1);
            form1.Hide();
            form2.Show();
        }

        public void checkId()
        {
            form2.Hide();
            form1.Show();
            
        }
    }
}
