using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace CoffeeShop
{
    public partial class AddOrderForm : Form
    {
        public AddOrderForm()
        {
            InitializeComponent();
        }

        private void AddOrderForm_Load(object sender, EventArgs e)
        {
            Loaders.LoadAllDBData();
            foreach (Item item in Globals.items)
            {
                cbItems.Items.Add(item.id + "." + item.name);
            }
            cbItems.SelectedIndex = 0;

            if (Globals.currentUser.type_id == userTypes.Admin)
            {
                lblSelectUser.Visible = true;
                cbUsers.Visible = true;
            }
            else if (Globals.currentUser.type_id == userTypes.Client)
            {
                lblAddress.Visible = true;
                txtAddress.Visible = true;
            }

            Loaders.LoadAllUsers();
            foreach (User user in Globals.users)
            {
                cbUsers.Items.Add(user.id + "." + user.username);
            }
            cbUsers.SelectedIndex = 0;
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            //add a new order
            DB db = new DB();
            MySqlCommand command = null;

            User rider = null;

            if (Globals.currentUser.type_id == userTypes.Client)
            {
                List<User> riders = Globals.users.Where(x => x.type_id == userTypes.Curier).ToList();
                if (riders.Count == 0)
                {
                    MessageBox.Show("Introduceti un produs", "Nu se poate crea comanda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    foreach (Order order in Globals.orders.Where(x => !string.IsNullOrWhiteSpace(x.address) && x.status != statusOrderTypes.platita))
                    {
                        rider = riders.Find(x => x.id != order.user_id);
                        if (rider != null)
                        {
                            break;
                        }
                    }
                    if (rider == null)
                    {
                        rider = riders[0];
                    }
                }
            }
            
            if (lbOrders.Items.Count == 0)
            {
                MessageBox.Show("Introduceti un produs", "Nu se poate crea comanda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (txtAddress.Visible == true && !string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Introduceti adresa", "Nu se poate crea comanda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            int userId = Globals.currentUser.id;
            if (Globals.currentUser.type_id == userTypes.Admin || Globals.currentUser.type_id == userTypes.Ospatar)
            {
                command = new MySqlCommand("INSERT INTO `orders`(`user_id`, `status_id`) VALUES (@user_id, @status)", db.getConnection());

                if (Globals.currentUser.type_id == userTypes.Admin)
                {
                    userId = Convert.ToInt32(cbUsers.SelectedItem.ToString().Split('.')[0]);
                }
            }
            else if (Globals.currentUser.type_id == userTypes.Client)
            {
                command = new MySqlCommand("INSERT INTO `orders`(`user_id`, `client_id`, `status_id`, `address`) VALUES (@user_id, @client_id, @status, @address)", db.getConnection());
                command.Parameters.Add("@address", MySqlDbType.VarChar).Value = txtAddress.Text;
                command.Parameters.Add("@client_id", MySqlDbType.Int32).Value = Globals.currentUser.id;
                userId = rider.id;
            }

            command.Parameters.Add("@user_id", MySqlDbType.Int32).Value = userId;
            command.Parameters.Add("@status", MySqlDbType.Int32).Value = (int)statusOrderTypes.asteptare;

            if (command.ExecuteNonQuery() != 1)
            {
                MessageBox.Show("A aparut o eroare la introducerea unei comenzi, incercati din nou", "Nu s-a putut plasa comanda", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            long orderId = command.LastInsertedId;

            for (int i = lbOrders.Items.Count - 1; i >= 0; i--)
            {
                string row = lbOrders.Items[i].ToString();

                if (!row.Contains("|") || row.Split('|').Length < 2)
                {
                    MessageBox.Show("A aparut o eroare la introducerea unei comenzi, incercati din nou", "Comanda nu a putut fi plasata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string[] items = row.Split('|');
                int itemId = Convert.ToInt32(items[0].Split('.')[0]);
                string itemName = items[0].Split('.')[1];
                int amount = Convert.ToInt32(items[1]);

                Item item = Globals.items.Find(x => x.id == itemId);
                if (item == null)
                {
                    MessageBox.Show("Produsul " + itemName + " nu a fost gasit", "Produs inexistent", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (item != null && item.quantity < amount)
                {
                    MessageBox.Show("Cantitatea pentru " + itemName + " este " + item.quantity, "Insuficient", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                command = new MySqlCommand("INSERT INTO `order_items`(`order_id`, `item_id`, `amount`) VALUES (@order_id, @item_id, @amount)", db.getConnection());

                command.Parameters.Add("@order_id", MySqlDbType.Int32).Value = orderId;
                command.Parameters.Add("@item_id", MySqlDbType.Int32).Value = itemId;
                command.Parameters.Add("@amount", MySqlDbType.Int32).Value = amount;

                if (command.ExecuteNonQuery() != 1)
                {
                    MessageBox.Show("A aparut o eroare la introducerea unei comenzi, incercati din nou", "Comanda nu a putut fi plasata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                command = new MySqlCommand("UPDATE `items` set `quantity` = " + (item.quantity - amount) + " where `id` = " + itemId, db.getConnection());

                if (command.ExecuteNonQuery() != 1)
                {
                    MessageBox.Show("A aparut o eroare la actualizarea cantitatii produsului", "Cantitatea nu a putut fi actualizata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                lbOrders.Items.RemoveAt(i);
            }

            MessageBox.Show("Comanda " + orderId + " a fost plasata!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnBack_Click(null, null);
        }
        
        private void btnBack_Click(object sender, EventArgs e)
        {
            switch (Globals.currentUser.type_id)
            {
                case userTypes.Admin:
                    this.Hide();
                    OrdersForm orders = new OrdersForm();
                    orders.Show();
                    break;
                case userTypes.Ospatar:
                    this.Hide();
                    TableOrdersForm tableOrders = new TableOrdersForm();
                    tableOrders.Show();
                    break;
                case userTypes.Client:
                    this.Hide();
                    ClientForm client = new ClientForm();
                    client.Show();
                    break;
            }
        }

        private void btnInsertItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAmount.Text)) return;

            string[] items = lbOrders.Items.Cast<string>().ToArray();
            if (items.Any(item => item.StartsWith(cbItems.SelectedItem.ToString()))) return;
            if (int.TryParse(txtAmount.Text, out int amount))
            {
                lbOrders.Items.Add(Convert.ToString(cbItems.SelectedItem) + "|" + amount);
            }
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            lbOrders.Items.RemoveAt(lbOrders.SelectedIndex);
        }

        private void cbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Globals.currentUser.type_id != userTypes.Admin) return;

            User user = Globals.users.Find(x => x.id == Convert.ToInt32(cbUsers.SelectedItem.ToString().Split('.')[0]));
            if (user == null) return;

            if (user.type_id == userTypes.Curier)
            {
                lblAddress.Visible = true;
                txtAddress.Visible = true;
            }
        }
    }
}
