using System;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class WaiterForm : Form
    {
        public WaiterForm()
        {
            InitializeComponent();
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            Globals.currentOrder = null;
            this.Hide();
            TableOrdersForm table = new TableOrdersForm();
            table.Show();
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
            txtUserType.Text = Globals.currentUser.type_id.ToString();
        }
    }
}
