using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Sql;
using System.Data;

namespace WindowsFormsApp1
{
    class Staffs
    {
        public static SQLiteConnection DB;
        public static DateTime dateZp;
        

        public int id, seniority, group;
        public double base_rate, percent_sub, percent_sub_all, percent_year, percent_year_max,rate_sub1,rate_sub_all,money_tree;
        public DateTime date;
        public List<int> subs = new List<int>();//Список подчинненых

       
        public Staffs()
        {

        }//Пустой конструктор
        public Staffs(int id)// Основной конструктор
        {
            this.id = id;
            this.rate_sub1 = 0;
            this.rate_sub_all = 0;
            this.money_tree = 0;          
            SQLiteDataReader reader = getReader($"select date,staffs.id_group,base_rate,percentage_subordinate,percentage_subordinates_all,percent_year,percent_year_max from staffs,groups where staffs.rowid = {this.id} and staffs.id_group = groups.id_group");        
            if (reader.Read())
            {
                this.date = (DateTime)reader.GetValue(0);
                this.group = reader.GetInt32(1);
                this.base_rate = reader.GetInt32(2);
                this.percent_sub = reader.GetDouble(3);
                this.percent_sub_all = reader.GetDouble(4);
                this.percent_year = reader.GetDouble(5);
                this.percent_year_max = reader.GetDouble(6);
                this.getSeniority();               
            }
            reader.Close();
            this.getSub();
        }
       
        public static double getTopCount()// Подсчет общей зарплаты
        {
            double sum = 0;
            SQLiteDataReader reader = Staffs.getReader("select rowid from staffs where id_n is null");
            while (reader.Read())
            {
                
                Staffs man = new Staffs(reader.GetInt32(0));
                man.count();
                sum += man.money_tree;
            }
            
            return sum;
        }
        
        public static SQLiteDataReader getReader(string s)
        {
            SQLiteCommand CMD =  Staffs.DB.CreateCommand();
            CMD.CommandText = s;
            return CMD.ExecuteReader();
        }
        public static void getDate(DateTime date)
        {
            Staffs.dateZp = date;
        }
        public static bool inId(int a)// проверка на наличие сотрудника с id = a
        {
            
            return (a <= Convert.ToInt32(Staffs.getScalar("select max(staffs.rowid) from staffs")) & (a > 0));                           
        }
        public static string getScalar(string s)
        {
            Staffs man = new Staffs();
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = s;
            return CMD.ExecuteScalar().ToString();
        }
        public static void staffsConnection(SQLiteConnection DB, DateTime date)
        {
            Staffs.DB = DB;
            Staffs.dateZp = date;
        }
        public static void staffsConnection(SQLiteConnection DB)
        {
            Staffs.DB = DB;
        }
        public static void insertStaff(string name, DateTime time, int id_group, int id_n,string login,string password)// добавление сотрудника
        {
            SQLiteCommand CMD = Staffs.DB.CreateCommand();
            CMD.CommandText = $"insert into staffs(name, date, id_group, id_n,login, password)  values('{name}', @date, {id_group},{id_n}, '{login}','{password}')";
            if (id_n < 0) CMD.CommandText = $"insert into staffs(name, date, id_group, id_n,login, password)  values('{name}', @date, {id_group},null, '{login}','{password}')";
            CMD.Parameters.Add("@date", DbType.Date).Value = time;
            CMD.ExecuteNonQuery();
        }
        public void getSub()//заполнение списка подчинненых
        {
            SQLiteCommand CMD = Staffs.DB.CreateCommand();
            CMD.CommandText = $"select rowid from staffs where id_n = {this.id}";
            SQLiteDataReader reader = CMD.ExecuteReader();
            while (reader.Read())
                this.subs.Add(reader.GetInt32(0));
            reader.Close();
        }
        
        public void getSeniority()//получение стажа работы
        {
            
            DateTime a = Staffs.dateZp;
            DateTime b = this.date;
           

            int count = a.Year - b.Year;
            if (count <= 0)
            {
                this.seniority = count;
            }
            if (a.Month < b.Month)
            {
                count--;
            }
            else if (a.Month == b.Month)
            {
                if (a.Day < b.Day) count--;
            }
            this.seniority = count;

            

        }
        public string info()
        {

            return $"id = {this.id}\n Базовая ставка = {this.base_rate}\n Дата поступления = {this.date}\n Стаж = {this.seniority}\n Номер группы = {this.group}\n Процент за подчинненых = {this.percent_sub + this.percent_sub_all}\n" +
                $"Зарплата подчинненых 1 уровня {this.rate_sub1}\n " +
                $"Зарплата всех подчинненых {this.rate_sub_all}\n" +
                $"Зарплата ветви {this.money_tree}"; 
            
        }
        
        public double countStaff()
        {
            if (this.seniority < 0)
                return 0;
            double perc_sen = this.seniority * this.percent_year;
            if (perc_sen > this.percent_year_max) perc_sen = this.percent_year_max;
            perc_sen = perc_sen / 100;
            double perc_sub = this.rate_sub_all * this.percent_sub_all / 100;
            if (this.group == 2)
            {
                perc_sub = this.percent_sub * this.rate_sub1 / 100;
            } else if (this.group == 1)
            {
                perc_sub = 0;
            }
                
            return this.base_rate + this.base_rate * perc_sen +  perc_sub  ;
        }
        public Money count()
        {
            
            foreach (int item in this.subs)
            {
                Staffs man = new Staffs(item);
                Money money = man.count();
                this.rate_sub_all += money.all;
                this.rate_sub1 += money.first_lvl;
                this.money_tree += man.money_tree;
                

            }

            Money ret_money = new Money(0,0,0);
            double count_staff = this.countStaff();

            if (this.group == 1)
            {
                ret_money.first_lvl += count_staff;
            }
            ret_money.all += count_staff;
            ret_money.all_rate += count_staff;
            this.money_tree += count_staff;

            return ret_money;
        }

        public double countAll()
        {
            return 0;
        }



    }
    struct Money
    {
        public double all, first_lvl,all_rate;
        public Money(double all, double first_lvl,double all_rate)
        {
            this.all = all;
            this.first_lvl = first_lvl;
            this.all_rate = all_rate;
        }
        public void Add(Money money)
        {
            this.all += money.all;
            this.first_lvl += money.first_lvl;
            this.all_rate += money.all_rate;

        }
        public void Refresh()
        {
            this.all = 0;
            this.first_lvl = 0;
            this.all_rate = 0;
        }

    }
}
