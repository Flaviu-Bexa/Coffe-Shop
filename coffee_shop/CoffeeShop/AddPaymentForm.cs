using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class AddPaymentForm : Form
    {
        public AddPaymentForm()
        {
            InitializeComponent();
        }

        private void btnAddPayment_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("INSERT INTO `payments`(`method`, `total_amount`, `time`) VALUES (@method, @total, @time)", db.getConnection());

            command.Parameters.Add("@method", MySqlDbType.VarChar).Value = cbComboBox.Text;
            command.Parameters.Add("@total", MySqlDbType.VarChar).Value = txtTotal.Text;
            command.Parameters.Add("@time", MySqlDbType.VarChar).Value = txtTimestamp.Text;

            command.ExecuteNonQuery();

            command = new MySqlCommand("UPDATE `orders` set `status_id` = 4 where `id` = " + Globals.currentOrder.id, db.getConnection());
            command.ExecuteNonQuery();

            MessageBox.Show("Plata a fost adaugata cu success", "Plata Adaugata", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Globals.currentOrder = null;

            btnBack_Click(null, null);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (Globals.currentUser.type_id == userTypes.Admin)
            {
                this.Hide();
                PaymentForm payment = new PaymentForm();
                payment.Show();
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
                RiderForm clientForm = new RiderForm();
                clientForm.Show();
            }
        }

        private void AddPayment_Load(object sender, EventArgs e)
        {
            this.txtTimestamp.Text = DateTime.Now.ToString();
            cbComboBox.SelectedIndex = 0;
            if (Globals.currentOrder == null) return;

            double amount = 0;
            foreach (CurrentOrder.LocalItem item in Globals.currentOrder.Items)
            {
                amount += item.amount * item.price;
            }
            txtTotal.Text = amount.ToString();
        }
    }
}
