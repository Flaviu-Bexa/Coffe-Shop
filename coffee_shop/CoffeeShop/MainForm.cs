using System;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            txtEmail.Text = Globals.currentUser.emailaddress;
            txtFullName.Text = Globals.currentUser.firstname + " " + Globals.currentUser.lastname;
            txtUserName.Text = Globals.currentUser.username;
            txtUserType.Text = Globals.currentUser.type_id.ToString();
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            Globals.currentOrder = null;
            this.Hide();
            InventoryForm inventory = new InventoryForm();
            inventory.Show();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            Globals.currentOrder = null;
            this.Hide();
            OrdersForm orders = new OrdersForm();
            orders.Show();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            Globals.currentOrder = null;
            this.Hide();
            UsersTable users = new UsersTable();
            users.Show();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            Globals.currentOrder = null;
            this.Hide();
            PaymentForm payment = new PaymentForm();
            payment.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Globals.currentOrder = null;
            this.Hide();
            LoginForm user = new LoginForm();
            user.Show();
        }
    }
}
