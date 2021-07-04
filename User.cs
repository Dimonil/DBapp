using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    
    class User
    {
        string name;
        int id, id_group, id_n;
        DateTime date;
        public User(string name, int id, int id_group, int id_n, DateTime date)
        {
            this.name = name;
            this.id = id;
            this.id_group = id_group;
            this.id_n = id_n;
            this.date = date;
        }
        int diff_year(DateTime a, DateTime b)
        {
            int days = a.Day - b.Day;
            int month = a.Month - b.Month;
            int years = a.Year - b.Year;
            return 0;
        }

    }
    
}
