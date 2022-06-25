using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class TableOrdersForm : Form
    {
        public TableOrdersForm()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            WaiterForm waiter = new WaiterForm();
            waiter.Show();
        }

        private void TableForm_Load(object sender, EventArgs e)
        {
            Loaders.LoadAllItems();
            Loaders.LoadAllOrderItems();
            Loaders.LoadAllOrders();

            tableOrders.Columns.Add("orderId", "Id");
            tableOrders.Columns.Add("itemNames", "Descriere");

            List<Order> userOrders = Globals.orders.Where(x => x.user_id == Globals.currentUser.id && x.status != statusOrderTypes.platita).ToList();
            List<OrderItem> userOrderItems = Globals.orderItems.Where(x => userOrders.Any(uo => uo.id == x.order_id)).ToList();

            foreach (Order order in userOrders)
            {
                object[] orderDetails = new object[2];
                orderDetails[0] = order.id;
                foreach (OrderItem orderItem in userOrderItems)
                {
                    if (orderItem.order_id == order.id)
                    {
                        Item item = Globals.items.Find(x => x.id == orderItem.item_id);
                        orderDetails[1] += item.id + "." + item.name + " x" + orderItem.amount + " |";
                    }
                }
                tableOrders.Rows.Add(orderDetails);
            }
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddOrderForm addOrder = new AddOrderForm();
            addOrder.Show();
        }

        private void btnShowOrder_Click(object sender, EventArgs e)
        {
            if (this.tableOrders.SelectedRows.Count == 0) return;

            DataGridViewRow row = this.tableOrders.SelectedRows[0];
            if (row.Cells[0].Value == null || row.Cells[1].Value == null) return;

            Globals.currentOrder = new CurrentOrder();
            Globals.currentOrder.id = Convert.ToInt32(row.Cells[0].Value);
            Globals.currentOrder.user_id = Globals.currentUser.id;
            Globals.currentOrder.Items = new List<CurrentOrder.LocalItem>();

            string[] orderItems = row.Cells[1].Value.ToString().Split('|');

            foreach (string item in orderItems)
            {
                string[] orderDetails = item.Split(' ');
                if (orderDetails.Length < 2) break;
                string[] itemDetails = orderItems[0].Split('.');
                CurrentOrder.LocalItem Item = new CurrentOrder.LocalItem();
                Item.item_id = Convert.ToInt32(itemDetails[0]);
                Item.amount = Convert.ToInt32(orderDetails[1].Trim('x'));
                Item dbItem = Globals.items.Find(x => x.id == Item.item_id);
                Item.item_name = dbItem.name;
                Item.price = dbItem.price_per_quantity;
                Globals.currentOrder.Items.Add(Item);
            }

            this.Hide();
            OrderForm orderForm = new OrderForm();
            orderForm.Show();
        }
    }
}
