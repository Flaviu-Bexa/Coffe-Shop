using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class AddItemsForm : Form
    {
        public AddItemsForm()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            InventoryForm inventory = new InventoryForm();
            inventory.Show();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("INSERT INTO `items`(`name`, `category`, `price_per_quantity`, `quantity`) VALUES (@n, @c, @pr, @q)", db.getConnection());

            command.Parameters.Add("@n", MySqlDbType.VarChar).Value = txtName.Text;
            command.Parameters.Add("@c", MySqlDbType.VarChar).Value = txtCategory.Text;
            command.Parameters.Add("@pr", MySqlDbType.VarChar).Value = txtPrice.Text;
            command.Parameters.Add("@q", MySqlDbType.VarChar).Value = txtQuantity.Text;

            command.ExecuteNonQuery();

            this.Hide();
            InventoryForm inventory = new InventoryForm();
            inventory.Show();

            MessageBox.Show("Produs Adaugat cu Success", "Produs Adugat", MessageBoxButtons.OK, MessageBoxIcon.Information);



            //open connection
          
        }
    }
}
