using System.Data;
using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CoffeeShop
{
    public partial class UsersTable : Form
    {
        public UsersTable()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {

            this.Hide();
            MainForm main = new MainForm();
            main.Show();
        }

        private void Users_Load(object sender, EventArgs e)
        {
            Loaders.LoadAllUsers();

            tableUser.Columns.Add("userId", "Id");
            tableUser.Columns.Add("firstName", "Prenume");
            tableUser.Columns.Add("lastName", "Nume");
            tableUser.Columns.Add("emailAddress", "Email");
            tableUser.Columns.Add("userName", "Nume utilizator");
            tableUser.Columns.Add("userType", "Tip");

            foreach (User user in Globals.users)
            {
                object[] userDetails = new object[6];
                userDetails[0] = user.id;
                userDetails[1] = user.firstname;
                userDetails[2] = user.lastname;
                userDetails[3] = user.emailaddress;
                userDetails[4] = user.username;
                userDetails[5] = user.type_id.ToString();

                tableUser.Rows.Add(userDetails);
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddUsersForm addUsers = new AddUsersForm();
            addUsers.Show();
        }

        private void btnRemoveUser_Click(object sender, EventArgs e)
        {
            if (this.tableUser.SelectedRows.Count == 0) return;
            if (this.tableUser.SelectedRows[0].Cells[0].Value == null) return;

            int userId = Convert.ToInt32(tableUser.SelectedRows[0].Cells[0].Value.ToString());
            if (Globals.currentUser.id == userId)
            {
                MessageBox.Show("Nu te poti sterge pe tine", "Nu se poate sterge!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DB db = new DB();
            DataTable table = new DataTable();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("DELETE FROM users WHERE id = " + userId, db.getConnection());

            MySqlDataReader rdr = command.ExecuteReader();

            table.Load(rdr);

            this.Hide();
            UsersTable users = new UsersTable();
            users.Show();

            MessageBox.Show("Randul cu ID-ul " + userId + " a fost sters.", "Rand Sters", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
