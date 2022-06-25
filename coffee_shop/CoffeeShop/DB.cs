using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop
{
    class DB
    { 
        MySqlConnection connection = new MySqlConnection("server=127.0.0.1;port=3306;username=root;password=;database=restaurant");

    //create a function to open the connection
    public void openConnection()
    {
        if (connection.State == System.Data.ConnectionState.Closed)
        {
            connection.Open();
        }
    }

    //create a function to close the connection
    public void closeConnection()
    {
        if (connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
        }
    }

    public MySqlConnection getConnection()
    {
        if (connection.State != System.Data.ConnectionState.Open)
        {
            openConnection();
        }
        return connection;
    }
}
}
