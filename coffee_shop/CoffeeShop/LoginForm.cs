using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class LoginForm : Form
    { 
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            DB db = new DB();

            string username = txtUsername.Text;
            string password = txtPassword.Text;

            Globals.currentUser = new User();

            DataTable userTable = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE  username = @usn and password = @pass", db.getConnection());

            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username;
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = password;

            adapter.SelectCommand = command;

            adapter.Fill(userTable);

            //check if the user exists or not
            if (userTable.Rows.Count > 0)
            {
                Loaders.LoadAllDBData();
                
                Globals.currentUser = new User();
                Globals.currentUser.id = Convert.ToInt32(userTable.Rows[0].ItemArray[0]);
                Globals.currentUser.firstname = userTable.Rows[0].ItemArray[1].ToString();
                Globals.currentUser.lastname = userTable.Rows[0].ItemArray[2].ToString();
                Globals.currentUser.emailaddress = userTable.Rows[0].ItemArray[3].ToString();
                Globals.currentUser.username = userTable.Rows[0].ItemArray[4].ToString();
                Globals.currentUser.type_id = (userTypes)Convert.ToInt32(userTable.Rows[0].ItemArray[6]);

                if (Globals.currentUser.type_id != userTypes.Admin && 
                    cbType.SelectedItem.ToString() != Globals.currentUser.type_id.ToString())
                {
                    MessageBox.Show("Selectati tipul de cont corect", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
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
                        WaiterForm waiter = new WaiterForm();
                        waiter.Show();
                    }
                    else if (Globals.currentUser.type_id == userTypes.Curier)
                    {
                        this.Hide();
                        RiderForm rider = new RiderForm();
                        rider.Show();
                    }
                    else if (Globals.currentUser.type_id == userTypes.Client)
                    {
                        this.Hide();
                        ClientForm client = new ClientForm();
                        client.Show();
                    }
                    else
                    {
                        MessageBox.Show("Selectati un tip de cont", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                if (username.Trim().Equals(""))
                {
                    MessageBox.Show("Introdu numele de utilizator", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (password.Trim().Equals(""))
                {
                    MessageBox.Show("Introdu parola", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Numele de utilizator sau parola sunt incorecte", "Informatii incorecte", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void labelGoToSignUp_LinkClicked(object sender, EventArgs e)
        { 
            this.Hide();
            RegisterForm registerform = new RegisterForm();
            registerform.Show();
        }

        private void User_Load(object sender, EventArgs e)
        {
            DataTable typesTable = Loaders.getDBList(TableNames.user_types.ToString());
            foreach (DataRow type in typesTable.Rows)
            {
                cbType.Items.Add(type.ItemArray[0]);
            }
            cbType.SelectedIndex = 0;
        }

        private void btnCloseApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
