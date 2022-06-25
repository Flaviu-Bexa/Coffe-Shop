using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class OrderForm : Form
    {
        public OrderForm()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (Globals.currentUser.type_id == userTypes.Admin)
            {
                this.Hide();
                MainForm main = new MainForm();
                main.Show();
            }
            else if (Globals.currentUser.type_id == userTypes.Ospatar)
            {
                this.Hide();
                TableOrdersForm tableOrdersForm = new TableOrdersForm();
                tableOrdersForm.Show();
            }
            else if (Globals.currentUser.type_id == userTypes.Curier)
            {
                this.Hide();
                DeliveryForm deliveryForm = new DeliveryForm();
                deliveryForm.Show();
            }
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            if (Globals.currentOrder == null) return;
            if (Globals.currentUser.type_id == userTypes.Curier)
            {
                if (Globals.currentOrder.status == statusOrderTypes.asteptare ||
                    Globals.currentOrder.status == statusOrderTypes.realizare)
                {
                    btnDeliverOrder.Visible = true;
                    btnPayment.Visible = false;
                }
                else if (Globals.currentOrder.status == statusOrderTypes.curs_de_livrare)
                {
                    btnDeliverOrder.Visible = false;
                    btnPayment.Visible = true;
                }
            }
            else if (Globals.currentUser.type_id == userTypes.Admin)
            {
                if (!string.IsNullOrWhiteSpace(Globals.currentOrder.address))
                {
                    if (Globals.currentOrder.status == statusOrderTypes.asteptare ||
                        Globals.currentOrder.status == statusOrderTypes.realizare)
                    {
                        btnDeliverOrder.Visible = true;
                        btnPayment.Visible = false;
                    }
                    else if (Globals.currentOrder.status == statusOrderTypes.curs_de_livrare)
                    {
                        btnDeliverOrder.Visible = false;
                        btnPayment.Visible = true;
                    }
                }
                else
                {
                    btnDeliverOrder.Visible = false;
                    btnPayment.Visible = true;
                }
            }
            else if (Globals.currentUser.type_id == userTypes.Ospatar)
            {
                btnDeliverOrder.Visible = false;
                btnPayment.Visible = true;
            }

            lbOrders.Items.Clear();
            if (!string.IsNullOrWhiteSpace(Globals.currentOrder.address))
            {
                lbOrders.Items.Add("Adresa: " + Globals.currentOrder.address);
                lbOrders.Items.Add("Status: " + Globals.currentOrder.status.ToString());
            }
            lbOrders.Items.Add("Produse: ");
            foreach (var item in Globals.currentOrder.Items)
            {
                lbOrders.Items.Add("\t" + item.item_name + " x" + item.amount);
            }
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddPaymentForm addPaymentForm = new AddPaymentForm();
            addPaymentForm.Show();
        }

        private void btnDeliverOrder_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            Globals.currentOrder.status = statusOrderTypes.curs_de_livrare;
            MySqlCommand command = new MySqlCommand("UPDATE `orders` set `status_id` = 3 where `id` = " + Globals.currentOrder.id, db.getConnection());
            command.ExecuteNonQuery();
            OrderForm_Load(null, null);
        }
    }
}
