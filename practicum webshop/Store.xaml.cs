using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace practicum_webshop
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        Constants c = new Constants();
        string ValueFromLogin;

        public Window2()
        {
            InitializeComponent();
        }

        public Window2(string username)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

            InitializeComponent();
            ValueFromLogin = username;          
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            int amount = 0;
            double price = 0.0;
            double currentCash = 0.0;

            MySqlConnection con = new MySqlConnection(c.dbConnection);
            con.Open();

            MySqlCommand cmd = new MySqlCommand("select amount, price from product where name='" + Cart.Items[0] + "';", con);
            cmd.ExecuteNonQuery();

            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                amount = (reader.GetInt32(0));
                price = (reader.GetDouble(1));
            }
            reader.Close();

            MySqlCommand cmd2 = new MySqlCommand("select cash from customer where username='" + ValueFromLogin + "';", con);
            cmd2.ExecuteNonQuery();
            MySqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                currentCash = (reader2.GetDouble(0));
                currentCash -= price;
                string x = currentCash.ToString();
            }
            reader2.Close();

            if (currentCash < 0)
            {
                MessageBox.Show("You cannot finalize your purchases, you do not have enough money left");
                //return to skip the database updates and not make the purchase
                return;
            }

            //Finalize purchase by updating database
            MySqlCommand cmd3 = new MySqlCommand("update webshop.product set amount =" + (amount - 1) + " where name = '"+ Cart.Items[0] + "'; ", con);
            cmd3.ExecuteNonQuery();
            MySqlCommand cmd4 = new MySqlCommand("update webshop.customer set cash =" + currentCash + " where username = '" + ValueFromLogin + "'; ", con);                       
            cmd4.ExecuteNonQuery();
            MySqlCommand cmd5 = new MySqlCommand("insert into webshop.purchases (customer, product) values ('" + ValueFromLogin + "', '" + Cart.Items[0] + "'); ", con);
            cmd5.ExecuteNonQuery();

            //Update GUI
            Cash.Content = "€" + currentCash;
            MessageBox.Show("You succesfully made a purchase");
        }

        private void Button_Buy(object sender, RoutedEventArgs e)
        {
            var selectedItems = store.SelectedItems;
            if (store.SelectedIndex != -1)
            {
                for (int i = selectedItems.Count - 1; i >= 0; i--)
                    Cart.Items.Add(selectedItems[i]);
            }
            else
                MessageBox.Show("Select the item you want to add to your cart first");
        }
        

        private void Button_Remove(object sender, RoutedEventArgs e)
        {
            var selectedItems = Cart.SelectedItems;
            if (Cart.SelectedIndex != -1)
            {
                for (int i = selectedItems.Count - 1; i >= 0; i--)
                    Cart.Items.Remove(selectedItems[i]);
            }
            else
                MessageBox.Show("Select the item you want to remove from your cart first");
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection(c.dbConnection);

            con.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * from product where amount >= 0;", con);
            cmd.ExecuteNonQuery();
            store.Items.Clear();
            MySqlDataReader reader = cmd.ExecuteReader();           
            while (reader.Read())
            {
                string name = reader.GetString(0);
                int amount = reader.GetInt32(1);
                double price = reader.GetDouble(2);
                store.Items.Add(name);
            }
            reader.Close();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Welkom.Content = "Welkom: " + ValueFromLogin;
            MySqlConnection con = new MySqlConnection(c.dbConnection);

            con.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * from product where amount > 0;", con);
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                string name = reader.GetString(0);
                int amount = reader.GetInt32(1);
                double price = reader.GetDouble(2);
                
                store.Items.Add(name); 
            }
            reader.Close();

            MySqlCommand cmd2 = new MySqlCommand("SELECT * from customer where username='" + ValueFromLogin + "';", con);
            cmd2.ExecuteNonQuery();
            MySqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                double cash = reader2.GetDouble(2);
                //Update GUI
                Cash.Content = "€" + cash;
            }
            reader2.Close();
        }
    }
}
