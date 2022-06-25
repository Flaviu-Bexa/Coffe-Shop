using System;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class RiderForm : Form
    {
        public RiderForm()
        {
            InitializeComponent();
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            Globals.currentOrder = null;
            this.Hide();
            DeliveryForm delivery = new DeliveryForm();
            delivery.Show();
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
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        private void RiderForm_Load(object sender, EventArgs e)
        {
            Globals.currentOrder = null;
            txtEmail.Text = Globals.currentUser.emailaddress;
            txtFullName.Text = Globals.currentUser.firstname + " " + Globals.currentUser.lastname;
            txtUserName.Text = Globals.currentUser.username;
            txtUserType.Text = Globals.currentUser.type_id.ToString();
        }
    }
}
