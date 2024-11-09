using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Lab_4
{
    class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source = DESKTOP-1MJHOSE\SQLEXPRESS; Initial Catalog = Likhoy429191-12; Integrated Security = True");
        public static string text; 
        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {

                sqlConnection.Open();

            }
        }
        public void closeConection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {

                sqlConnection.Close();

            }
        }

        public SqlConnection getConnection()
        {
            return sqlConnection;

        }

    }
}
