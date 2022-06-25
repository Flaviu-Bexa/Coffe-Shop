using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class PaymentForm : Form
    {
        public PaymentForm()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {

            this.Hide();
            MainForm main = new MainForm();
            main.Show();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddPaymentForm addPayment = new AddPaymentForm();
            addPayment.Show();
        }

        private void Payment_Load(object sender, EventArgs e)
        {
            tablePayments.DataSource = Loaders.getDBList(TableNames.payments.ToString());
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string idLocRemv = tablePayments.SelectedRows[0].Cells[0].Value.ToString();

            DB db = new DB();
            DataTable table = new DataTable();

            db.openConnection();

            MySqlCommand command = new MySqlCommand("DELETE FROM payments WHERE id = " + idLocRemv, db.getConnection());

            MySqlDataReader rdr = command.ExecuteReader();

            table.Load(rdr);

            this.Hide();
            PaymentForm payment = new PaymentForm();
            payment.Show();

            MessageBox.Show("Randul cu ID-ul " + idLocRemv + " a fost sters.", "Rand Sters", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
