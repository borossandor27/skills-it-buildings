using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;

namespace skills_it_buildings
{
    class Program
    {
        static MySqlConnection conn = null;
        static MySqlCommand sql = null;
        static void Main(string[] args)
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Server = "localhost";
            sb.UserID = "root";
            sb.Password = "";
            sb.Database = "skills-it-buildings";
            sb.CharacterSet = "UTF8";
            sb.Port = 3306;
            conn = new MySqlConnection(sb.ToString());
            try
            {
                conn.Open();
                sql = conn.CreateCommand();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                Environment.Exit(0);
            }
            ReadBuildings();
            Console.WriteLine("\nProgram vége!");
            Console.ReadKey();
        }

        static void ReadBuildings()
        {
            using (StreamWriter sw = new StreamWriter("buildings.json"))
            {
                string LF = "\n";
                sql.CommandText = "SELECT `id`,`name`,`num_of_floors`,`address` FROM `buildings`";
                string ki= "";
                try
                {

                    ki += "{" + LF;
                    ki += "\"buildings\": [" + LF;
                    using (MySqlDataReader dr = sql.ExecuteReader())
                    {
                    
                        while (dr.Read())
                        {
                            Console.WriteLine($"{dr.GetInt32("id")}, {dr.GetString("name")}, {dr.GetInt32("num_of_floors")}, {dr.GetString("address")}");
                            ki += "{" + LF;
                            ki += $"\"id\": {dr.GetInt32("id")}," + LF;
                            ki += $"\"name\": \"{dr.GetString("name")}\"," + LF;
                            ki += $"\"num_of_floors\": {dr.GetInt32("num_of_floors")}," + LF;
                            ki += $"\"address\": \"{dr.GetString("address")}\"" + LF;
                            ki += "}," + LF;
                        }
                    }
                    ki = ki.Substring(0, ki.Length - 2); //-- levágjuk az utolsó rekord vesszőjét
                    ki += "]" + LF;
                    ki += "}";
                    sw.WriteLine(ki);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey();
                    Environment.Exit(0);
                }

            }
        }
    }
}
