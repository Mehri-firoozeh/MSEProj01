using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

namespace DatabaseSetup
{
    class Program
    {
        /// <summary>
        /// This program only needs to run when a new blank database needs to be created.
        /// It is stored in disabled form to prevent accidental DB options.
        /// </summary>
        /// <param name="args">Not Reguired</param>
        static void Main(string[] args)
        {
          
            string input = "";//__change this to empty string "" to re-enable use

            while (input != "q")
            {
                Console.WriteLine("Enter option (c=create database, w=write sample data, q=quit");
                input = Console.ReadLine();

                if (input == "c")
                {
                    try
                    {
                        CreateDB();
                        Console.WriteLine("Database Successfully Created");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("*** ERROR ***");
                        Console.WriteLine(e.Message);
                        return;
                    }
                }

                if (input == "w")
                {
                    try
                    {
                        AddSampleData();
                        Console.WriteLine("Sample Data Successfully Added");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("*** ERROR ***");
                        Console.WriteLine(e.Message);
                        return;
                    }
                }

                if (input == "d")
                {
                    Console.WriteLine("This will delete all DB entities, are you sure?!! (y/n)");
                    string reply = Console.ReadLine();
                    if (reply == "y")
                    {
                        using (SqlConnection sqlConnection = new SqlConnection())
                        {
                            sqlConnection.ConnectionString = ConnectionString;

                            string commandPath = Directory.GetCurrentDirectory();
                            commandPath += "\\DeleteAll.sql";
                            string sqlCommandText = File.ReadAllText(commandPath);
                            SqlCommand sqlCommand = sqlConnection.CreateCommand();
                            sqlCommand.CommandText = sqlCommandText;
                            sqlCommand.CommandTimeout = 30;


                            sqlConnection.Open();
                            sqlCommand.BeginExecuteNonQuery();
                            sqlConnection.Close();
                        }
                    }
                }

                if (input == "q" || string.IsNullOrEmpty(input)) return;
            }

        }

        private static void CreateDB()
        {
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = ConnectionString;

                string commandPath = Directory.GetCurrentDirectory();
                commandPath += "\\CostcoDBCreator.sql";
                string sqlCommandText = File.ReadAllText(commandPath);
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = sqlCommandText;
                sqlCommand.CommandTimeout = 30;


                sqlConnection.Open();
                sqlCommand.BeginExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        private static void AddSampleData()
        {
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = ConnectionString;

                string commandPath = Directory.GetCurrentDirectory();
                commandPath += "\\InjectTestData.sql";
                string sqlCommandText = File.ReadAllText(commandPath);
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = sqlCommandText;
                sqlCommand.CommandTimeout = 30;

                sqlConnection.Open();
                sqlConnection.Close();
            }
        }

        public static string ConnectionString = "Server=tcp:costcosu.database.windows.net,1433;Database=CostcoDevStatus;User ID=SUAdmin@costcosu;Password=39ffbJeo;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


}
}
