using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class InventoryForm : Form
    {
        public InventoryForm()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {

            this.Hide();
            MainForm main = new MainForm();
            main.Show();
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            string idLocRemv = tableItems.SelectedRows[0].Cells[0].Value.ToString();

            DB db = new DB();
            DataTable table = new DataTable();

            db.openConnection();

            MySqlCommand command = new MySqlCommand("DELETE FROM items WHERE id = " + idLocRemv, db.getConnection());

            MySqlDataReader rdr = command.ExecuteReader();

            table.Load(rdr);

            this.Hide();
            InventoryForm inventory = new InventoryForm();
            inventory.Show();

            MessageBox.Show("Randul cu ID-ul " + idLocRemv + " a fost sters.", "Rand Sters", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddItemsForm addItems = new AddItemsForm();
            addItems.Show();
        }

        private void Inventory_Load(object sender, EventArgs e)
        {
            tableItems.DataSource = Loaders.getDBList(TableNames.items.ToString());
        }
    }
}
