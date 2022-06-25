using System;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            Globals.currentOrder = null;
            this.Hide();
            AddOrderForm ordersForm = new AddOrderForm();
            ordersForm.Show();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            Globals.currentOrder = null;
            this.Hide();
            MenuForm menu = new MenuForm();
            menu.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Globals.currentOrder = null;
            this.Hide();
            LoginForm user = new LoginForm();
            user.Show();
        }

        private void WaiterForm_Load(object sender, EventArgs e)
        {
            Globals.currentOrder = null;
            txtEmail.Text = Globals.currentUser.emailaddress;
            txtFullName.Text = Globals.currentUser.firstname + " " + Globals.currentUser.lastname;
            txtUserName.Text = Globals.currentUser.username;
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            this.Hide();
            OrdersForm ordersForm = new OrdersForm();
            ordersForm.Show();
        }
    }
}
