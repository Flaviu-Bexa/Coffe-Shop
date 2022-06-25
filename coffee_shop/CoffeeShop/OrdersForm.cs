using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class OrdersForm : Form
    {
        public OrdersForm()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            switch (Globals.currentUser.type_id)
            {
                case userTypes.Admin:
                    this.Hide();
                    MainForm main = new MainForm();
                    main.Show();
                    break;
                case userTypes.Client:
                    this.Hide();
                    ClientForm client = new ClientForm();
                    client.Show();
                    break;
                default:
                    break;
            }
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddOrderForm addOrder = new AddOrderForm();
            addOrder.Show();
        }

        private void OrdersForm_Load(object sender, EventArgs e)
        {
            if (Globals.currentUser.type_id == userTypes.Client)
            {
                btnAddOrder.Visible = false;
                chkOnlyPaid.Visible = false;    
            }

            Loaders.LoadAllOrders();
            Loaders.LoadAllOrderItems();
            lbOrders.Items.Clear();
            List<Order> userOrders = null;
            if (Globals.currentUser.type_id == userTypes.Admin)
            {
                userOrders = Globals.orders.Where(x => x.status != statusOrderTypes.platita).ToList();
                if (chkOnlyPaid.Checked)
                {
                    userOrders = Globals.orders.Where(x => x.status == statusOrderTypes.platita).ToList();
                }
            }
            else
            {
                userOrders = Globals.orders.Where(x => x.cilent_id == Globals.currentUser.id).ToList();
            }

            List<OrderItem> userOrderItems = Globals.orderItems.Where(x => userOrders.Any(uo => uo.id == x.order_id)).ToList();

            foreach (Order order in userOrders)
            {
                string orderDetails = string.Empty;
                foreach (OrderItem orderItem in userOrderItems)
                {
                    if (orderItem.order_id == order.id)
                    {
                        Item item = Globals.items.Find(x => x.id == orderItem.item_id);
                        orderDetails += item.id + "." + item.name + " x" + orderItem.amount + " | ";
                    }
                }
                if (!string.IsNullOrWhiteSpace(orderDetails))
                {
                    orderDetails += order.status.ToString();
                }

                if (Globals.currentUser.type_id == userTypes.Admin)
                {
                    User user = Globals.users.Find(x => x.id == order.user_id);
                    if (user != null)
                    {
                        orderDetails += " | " + user.username;
                    }
                }

                lbOrders.Items.Add(orderDetails);
            }
        }

        private void chkOnlyPaid_CheckedChanged(object sender, EventArgs e)
        {
            OrdersForm_Load(null, null);
        }
    }
}
