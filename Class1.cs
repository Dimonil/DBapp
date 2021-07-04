using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace WindowsFormsApp1
{
    class Class1 
    {
        public static  SQLiteConnection DB;
        public string login, password;
        public bool auth = false;
        public int id;
        public Class1(SQLiteConnection DB)
        {
            Class1.DB = DB;
            
        }
        public Class1(string login, string password)
        {
            this.login = login;
            this.password = password;
            this.checkId();
            
        }
        
        public string getScalar(string s)
        {
            try
            {
                SQLiteCommand CMD = Class1.DB.CreateCommand();
                CMD.CommandText = s;
                SQLiteDataReader reader = CMD.ExecuteReader();

                if (reader.Read())
                {
                    this.id = reader.GetInt32(1);
                    return reader.GetValue(0).ToString();
                }
            }
            catch
            {
                return null;
            }

            return null;
              
            
        }

        public void checkId()
        {

            string s = $"select password,rowid from staffs where login = {this.login}";
            
                
            if (this.password == this.getScalar(s))
            {
                this.auth = true;
            }
            
            
            

        }


    }
}
