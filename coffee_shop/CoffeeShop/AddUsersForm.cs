using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class AddUsersForm : Form
    {
        public AddUsersForm()
        {
            InitializeComponent();
        }

        private void AddUsers_Load(object sender, EventArgs e)
        {
            DataTable typesTable = Loaders.getDBList(TableNames.user_types.ToString());
            foreach (DataRow type in typesTable.Rows)
            {
                cbType.Items.Add(type.ItemArray[0]);
            }
            cbType.SelectedIndex = 0;
        }

        private void txtFirstName_Enter(object sender, EventArgs e)
        {
            string fname = txtFirstName.Text;
            if (fname.ToLower().Trim().Equals("Prenume"))
            {
                txtFirstName.Text = "";
                txtFirstName.ForeColor = Color.Black;
            }
        }

        private void txtFirstName_Leave(object sender, EventArgs e)
        {
            string fname = txtFirstName.Text;
            if (fname.ToLower().Trim().Equals("Prenume") || fname.Trim().Equals(""))
            {
                txtFirstName.Text = "Prenume";
                txtFirstName.ForeColor = Color.Gray;
            }
        }

        private void txtLastname_Enter(object sender, EventArgs e)
        {
            string lname = txtLastname.Text;
            if (lname.ToLower().Trim().Equals("Nume"))
            {
                txtLastname.Text = "";
                txtLastname.ForeColor = Color.Black;
            }
        }

        private void txtLastname_Leave(object sender, EventArgs e)
        {
            string lname = txtLastname.Text;
            if (lname.ToLower().Trim().Equals("Nume") || lname.Trim().Equals(""))
            {
                txtLastname.Text = "Nume";
                txtLastname.ForeColor = Color.Gray;
            }
        }

        private void txtEmail_Enter(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            if (email.ToLower().Trim().Equals("Adresa de email"))
            {
                txtEmail.Text = "";
                txtEmail.ForeColor = Color.Black;
            }
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            if (email.ToLower().Trim().Equals("Adresa de email") || email.Trim().Equals(""))
            {
                txtEmail.Text = "Adresa de email";
                txtEmail.ForeColor = Color.Gray;
            }
        }

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            if (username.ToLower().Trim().Equals("Nume de utilizator"))
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black;
            }
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            if (username.ToLower().Trim().Equals("Nume de utilizator") || username.Trim().Equals(""))
            {
                txtUsername.Text = "Nume de utilizator";
                txtUsername.ForeColor = Color.Gray;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            string password = txtPassword.Text;
            if (password.ToLower().Trim().Equals("Parola"))
            {
                txtPassword.Text = "";
                txtPassword.UseSystemPasswordChar = true;
                txtPassword.ForeColor = Color.Black;
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            string password = txtPassword.Text;
            if (password.ToLower().Trim().Equals("Parola") || password.Trim().Equals(""))
            {
                txtPassword.Text = "Parola";
                txtPassword.UseSystemPasswordChar = true;
                txtPassword.ForeColor = Color.Gray;
            }
        }

        private void txtPasswordConfirm_Enter(object sender, EventArgs e)
        {
            string cpassword = txtPasswordConfirm.Text;
            if (cpassword.ToLower().Trim().Equals("Confirma Parola"))
            {
                txtPasswordConfirm.Text = "";
                txtPasswordConfirm.UseSystemPasswordChar = true;
                txtPasswordConfirm.ForeColor = Color.Black;
            }
        }

        private void txtPasswordConfirm_Leave(object sender, EventArgs e)
        {
            string cpassword = txtPasswordConfirm.Text;
            if (cpassword.ToLower().Trim().Equals("Confirma Parola") ||
                cpassword.ToLower().Trim().Equals("Parola") ||
                cpassword.Trim().Equals(""))
            {
                txtPasswordConfirm.Text = "Confirma Parola";
                txtPasswordConfirm.UseSystemPasswordChar = true;
                txtPasswordConfirm.ForeColor = Color.Gray;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //add a new user

            DB db = new DB();
            MySqlCommand command = new MySqlCommand("INSERT INTO `users`(`firstname`, `lastname`, `emailaddress`, `username`, `password`, `type_id`) VALUES (@fn, @ln, @email, @usn, @pass, @typeId)", db.getConnection());

            command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = txtFirstName.Text;
            command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = txtLastname.Text;
            command.Parameters.Add("@email", MySqlDbType.VarChar).Value = txtEmail.Text;
            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = txtUsername.Text;
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = txtPassword.Text;
            command.Parameters.Add("@typeId", MySqlDbType.Int32).Value = cbType.SelectedIndex;

            //open connection
            db.openConnection();

            if (!checkTextBoxesValues())
            {
                //check if password matched the confirm password
                if (txtPassword.Text.Equals(txtPasswordConfirm.Text))
                {
                    //CHECK if username already exist
                    if (CheckUsername())
                    {
                        MessageBox.Show("Numele de utilizator exista deja", "Nume de utilizator existent", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        //execute the query
                        if (command.ExecuteNonQuery() == 1)
                        {
                            this.Hide();
                            UsersTable users = new UsersTable();
                            users.Show();

                            MessageBox.Show("Contul a fost creat cu success", "Cont Creat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            string message = "Eroare";
                            string title = "Titlu";
                            MessageBox.Show(message, title);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Parola este incorecta", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Introdu informatiile personale", "Fara Informatii", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //close connection
            db.closeConnection();
        }
        //check if the username already exist

        public Boolean CheckUsername()
        {
            DB db = new DB();
            string username = txtUsername.Text;
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE  username = @usn", db.getConnection());

            command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username;

            adapter.SelectCommand = command;

            adapter.Fill(table);

            //check if this username already exists int the database
            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //check if the textboxes contains the default values
        public Boolean checkTextBoxesValues()
        {
            string fname = txtFirstName.Text;
            string lname = txtLastname.Text;
            string email = txtEmail.Text;
            string uname = txtUsername.Text;
            string pass = txtPassword.Text;

            if (fname.Equals("Prenume") || lname.Equals("Nume") || email.Equals("Adresa de email")
                || uname.Equals("Nume de utilizator") || pass.Equals("Parola"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            UsersTable users = new UsersTable();
            users.Show();
        }
    }
}
