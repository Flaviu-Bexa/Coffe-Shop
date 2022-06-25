using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace CoffeeShop
{
    class Globals
    {
        public static User currentUser;
        public static CurrentOrder currentOrder;
        public static List<UserType> types = new List<UserType>();
        public static List<Payment> payments = new List<Payment>();
        public static List<Item> items = new List<Item>();
        public static List<User> users = new List<User>();
        public static List<OrderItem> orderItems = new List<OrderItem>();
        public static List<Order> orders = new List<Order>();
        public static List<StatusOrderType> statusOrderTypes = new List<StatusOrderType>();
    }

    public enum TableNames
    {
        user_types = 0,
        payments = 1,
        items = 2,
        users = 3,
        sales = 4,
        orders = 5,
        order_items = 6,
        status_order_types = 7,
    }

    public enum userTypes 
    {
        Admin = 0,
        Ospatar = 1,
        Curier = 2,
        Client = 3,
    }

    public enum statusOrderTypes
    {
        asteptare = 1,
        realizare = 2,
        curs_de_livrare = 3,
        platita = 4,
    }

    public class CurrentOrder
    {
        public int id;
        public int user_id;
        public int cilent_id;
        public string address;
        public statusOrderTypes status;
        public List<LocalItem> Items;

        public class LocalItem
        {
            public int item_id;
            public string item_name;
            public double price;
            public int amount;
        }
    }

    public class UserType
    {
        public string name;
        public string number;
    }

    public class StatusOrderType
    {
        public int id;
        public string name;
    }

    public class Payment
    {
        public int id;
        public string method;
        public double total_amount;
        public string time;
    }

    public class Item
    {
        public int id;
        public string name;
        public string category;
        public double price_per_quantity;
        public int quantity;
    }

    public class User
    {
        public int id;
        public string firstname;
        public string lastname;
        public string emailaddress;
        public string username;
        public string password;
        public userTypes type_id;
    }
    public class OrderItem
    {
        public int id;
        public int order_id;
        public int item_id;
        public int amount;
    }
        
    public class Order
    {
        public int id;
        public int user_id;
        public int cilent_id;
        public string address;
        public statusOrderTypes status;
    }

    public static class Loaders
    {
        public static DataTable getDBList(string tableName)
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlCommand command = new MySqlCommand("SELECT * FROM " + tableName, db.getConnection());

            db.openConnection();

            MySqlDataReader rdr = command.ExecuteReader();

            table.Load(rdr);

            return table;
        }

        public static void LoadAllUserTypes()
        {
            Globals.types.Clear();
            DataTable typesTable = getDBList(TableNames.user_types.ToString());
            foreach (DataRow row in typesTable.Rows)
            {
                UserType dbType = new UserType();
                dbType.name = row.ItemArray[0].ToString();
                dbType.number = row.ItemArray[1].ToString();
                Globals.types.Add(dbType);
            }
        }

        public static void LoadAllPayments()
        {
            Globals.payments.Clear();
            DataTable paymentsTable = getDBList(TableNames.payments.ToString());
            foreach (DataRow row in paymentsTable.Rows)
            {
                Payment dbPayment = new Payment();
                dbPayment.id = Convert.ToInt32(row.ItemArray[0].ToString());
                dbPayment.method = row.ItemArray[1].ToString();
                dbPayment.total_amount = Convert.ToDouble(row.ItemArray[2].ToString());
                dbPayment.time = row.ItemArray[3].ToString();
                Globals.payments.Add(dbPayment);
            }
        }

        public static void LoadAllItems()
        {
            Globals.items.Clear();
            DataTable itemsTable = getDBList(TableNames.items.ToString());
            foreach (DataRow row in itemsTable.Rows)
            {
                Item dbItem = new Item();
                dbItem.id = Convert.ToInt32(row.ItemArray[0].ToString());
                dbItem.name = row.ItemArray[1].ToString();
                dbItem.category = row.ItemArray[2].ToString();
                dbItem.price_per_quantity = Convert.ToDouble(row.ItemArray[3].ToString());
                dbItem.quantity = Convert.ToInt32(row.ItemArray[4].ToString());
                Globals.items.Add(dbItem);
            }
        }

        public static void LoadAllOrders()
        {
            Globals.orders.Clear();
            DataTable ordersTable = getDBList(TableNames.orders.ToString());
            foreach (DataRow row in ordersTable.Rows)
            {
                Order dbOrders = new Order();
                dbOrders.id = Convert.ToInt32(row.ItemArray[0].ToString());
                dbOrders.user_id = Convert.ToInt32(row.ItemArray[1].ToString());
                dbOrders.cilent_id = row.ItemArray[2] != DBNull.Value ? Convert.ToInt32(row.ItemArray[2].ToString()) : 0;
                dbOrders.address = row.ItemArray[3] != DBNull.Value ? row.ItemArray[3].ToString() : string.Empty;
                dbOrders.status = (statusOrderTypes)Convert.ToInt32(row.ItemArray[4]);
                Globals.orders.Add(dbOrders);
            }
        }

        public static void LoadAllStatusOrderTypes()
        {
            Globals.statusOrderTypes.Clear();
            DataTable statusOrderTypesTable = getDBList(TableNames.status_order_types.ToString());
            foreach (DataRow row in statusOrderTypesTable.Rows)
            {
                StatusOrderType dbStatusOrderTypes = new StatusOrderType();
                dbStatusOrderTypes.id = Convert.ToInt32(row.ItemArray[0].ToString());
                dbStatusOrderTypes.name = row.ItemArray[1].ToString();
                Globals.statusOrderTypes.Add(dbStatusOrderTypes);
            }
        }

        public static void LoadAllOrderItems()
        {
            Globals.orderItems.Clear();
            DataTable order_Items = getDBList(TableNames.order_items.ToString());
            foreach (DataRow row in order_Items.Rows)
            {
                OrderItem dborder_Items = new OrderItem();
                dborder_Items.id = Convert.ToInt32(row.ItemArray[0].ToString());
                dborder_Items.order_id = Convert.ToInt32(row.ItemArray[1].ToString());
                dborder_Items.item_id = Convert.ToInt32(row.ItemArray[2].ToString());
                dborder_Items.amount = Convert.ToInt32(row.ItemArray[3].ToString());
                Globals.orderItems.Add(dborder_Items);
            }
        }

        public static void LoadAllUsers()
        {
            Globals.users.Clear();
            DataTable users = getDBList(TableNames.users.ToString());
            foreach (DataRow row in users.Rows)
            {
                User dbUsers = new User();
                dbUsers.id = Convert.ToInt32(row.ItemArray[0].ToString());
                dbUsers.firstname = row.ItemArray[1].ToString();
                dbUsers.lastname = row.ItemArray[2].ToString();
                dbUsers.emailaddress = row.ItemArray[3].ToString();
                dbUsers.username = row.ItemArray[4].ToString();
                dbUsers.password = row.ItemArray[5].ToString();
                dbUsers.type_id = (userTypes)Convert.ToInt32(row.ItemArray[6].ToString());
                Globals.users.Add(dbUsers);
            }
        }

        public static void LoadAllDBData()
        {
            LoadAllUserTypes();
            LoadAllPayments();
            LoadAllItems();
            LoadAllOrders();
            LoadAllOrderItems();
            LoadAllUsers();
            LoadAllStatusOrderTypes();
        }
    }
}
