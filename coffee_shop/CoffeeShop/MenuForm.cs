using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CoffeeShop
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            switch (Globals.currentUser.type_id)
            {
                case userTypes.Admin:
                    this.Hide();
                    MainForm main = new MainForm();
                    main.Show();
                    break;
                case userTypes.Ospatar:
                    this.Hide();
                    WaiterForm waiter = new WaiterForm();
                    waiter.Show();
                    break;
                case userTypes.Curier:
                    this.Hide();
                    RiderForm rider = new RiderForm();
                    rider.Show();
                    break;
                case userTypes.Client:
                    this.Hide();
                    ClientForm client = new ClientForm();
                    client.Show();
                    break;
                default:
                    break;
            }
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            tableItems.Columns.Add("name", "Produs");
            tableItems.Columns.Add("price", "Pret");
            tableItems.Columns.Add("quantity", "Cantitate");

            List<string> categories = new List<string>();
            foreach (Item item in Globals.items)
            {
                if (!categories.Contains(item.category))
                {
                    categories.Add(item.category);
                }
            }

            foreach (string category in categories)
            {
                tableItems.Rows.Add();
                tableItems.Rows.Add(category);

                foreach (Item item in Globals.items)
                {
                    if (item.category == category)
                    {
                        string[] row = new string[3];
                        row[0] = item.name;
                        row[1] = item.price_per_quantity.ToString();
                        row[2] = item.quantity.ToString();
                        tableItems.Rows.Add(row);
                    }
                }
            }
        }
    }
}
