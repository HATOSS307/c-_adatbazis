using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace adatlekeres_adatbazis
{
    class Program
    {
        
        static void Main(string[] args)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.UserID = "root";
            builder.Password = "";
            builder.Database = "pizza";


            MySqlConnection connection = new MySqlConnection(builder.ConnectionString);
            Console.WriteLine("23.Feladat:\nHány házhoz szállítása volt az egyes futároknak?");
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT futar.fnev,count(rendeles.fazon)
                                        FROM vevo 
                                        JOIN rendeles USING(vazon)
                                        JOIN tetel USING(razon)
                                        JOIN pizza USING(pazon)
                                        JOIN futar USING(fazon)
                                        GROUP by futar.fnev;";
                //Adatok lekérdezése
                using (MySqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string fnev = dr.GetString("fnev");
                        int fazon = dr.GetInt32("count(rendeles.fazon)");
                        
                        Console.WriteLine($"{fnev}\t{fazon}");
                    }
                }
                Console.WriteLine("\n24.Feladat:\nA fogyasztás alapján mi a pizzák népszerűségi sorrendje?");
                command.CommandText = @"SELECT DISTINCT pizza.pnev
                                        FROM vevo 
                                        JOIN rendeles USING(vazon)
                                        JOIN tetel USING(razon)
                                        JOIN pizza USING(pazon)
                                        JOIN futar USING(fazon)
                                        ORDER BY tetel.pazon DESC;";
                
                using (MySqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string pnev = dr.GetString("pnev");
                        

                        Console.WriteLine($"\t{pnev}");
                    }
                }
                Console.WriteLine("\n25.Feladat:\nA rendelések összértéke alapján mi a vevők sorrendje?");
                command.CommandText = @"SELECT DISTINCT vevo.vnev 
                                        FROM vevo 
                                        JOIN rendeles USING(vazon)
                                        JOIN tetel USING(razon)
                                        JOIN pizza USING(pazon)
                                        JOIN futar USING(fazon)
                                        GROUP by vnev
                                        ORDER BY sum(tetel.db) DESC;";
                
                using (MySqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string vnev = dr.GetString("vnev");


                        Console.WriteLine($"\t{vnev}");
                    }
                }
                Console.WriteLine("\n26.Feladat:\nMelyik a legdrágább pizza?");
                command.CommandText = @"SELECT pizza.pnev
                                        FROM pizza
                                        ORDER BY par DESC LIMIT 1;";

                using (MySqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string pnev = dr.GetString("pnev");


                        Console.WriteLine($"\t{pnev}");
                    }
                }
                Console.WriteLine("\n27.Feladat:\nKi szállította házhoz a legtöbb pizzát?");
                command.CommandText = @"SELECT futar.fnev
                                        FROM vevo 
                                        JOIN rendeles USING(vazon)
                                        JOIN tetel USING(razon)
                                        JOIN pizza USING(pazon)
                                        JOIN futar USING(fazon)
                                        GROUP by futar.fnev
                                        ORDER BY COUNT(rendeles.fazon) DESC LIMIT 1;";

                using (MySqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string fnev = dr.GetString("fnev");
                        

                        Console.WriteLine($"\t{fnev}");
                    }
                }
                Console.WriteLine("\n28.Feladat:\nKi ette a legtöbb pizzát?");
                command.CommandText = @"SELECT vevo.vnev
                                        FROM vevo 
                                        JOIN rendeles USING(vazon)
                                        JOIN tetel USING(razon)
                                        JOIN pizza USING(pazon)
                                        JOIN futar USING(fazon)
                                        GROUP BY vevo.vnev
                                        ORDER BY SUM(tetel.db) DESC LIMIT 1;";

                using (MySqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string vnev = dr.GetString("vnev");


                        Console.WriteLine($"\t{vnev}");
                    }
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            Console.ReadKey();

        }
    }
}