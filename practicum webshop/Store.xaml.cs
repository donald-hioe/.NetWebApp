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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            int amount = 0;
            double price = 0.0;
            double currentCash = 0.0;
            //string value = null;
            string conString = "Server=localhost;Database=webshop;Uid=donald;Pwd=Niek02102004;";
            MySqlConnection con = new MySqlConnection(conString);

            con.Open();
            MySqlCommand cmd = new MySqlCommand("select amount, price from product where name='" + Cart.Items[0] + "';", con);
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                amount = (reader.GetInt32(0));
                amount -= 1;
                price = (reader.GetDouble(1));
            }
            Console.WriteLine(amount);
            Console.WriteLine(price);
            reader.Close();

            MySqlCommand cmd2 = new MySqlCommand("SELECT cash from customer where username='" + ValueFromLogin + "';", con);
            cmd2.ExecuteNonQuery();
            MySqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                currentCash = (reader2.GetDouble(0));
                currentCash -= price;
                string x = currentCash.ToString();
            }
            Console.WriteLine(currentCash);
            
            reader2.Close();


            MySqlCommand cmd3 = new MySqlCommand("update webshop.product set amount ="+ amount+" where name = '"+ Cart.Items[0] + "'; ", con);
            cmd3.ExecuteNonQuery();
            MySqlCommand cmd4 = new MySqlCommand("update webshop.customer set cash =" + currentCash + " where username = '"+ValueFromLogin+"'; ", con);                       
            cmd4.ExecuteNonQuery();
            Cash.Content = "€"+currentCash;


            //var amount = cmd.ExecuteScalar();
            //Cash.Content = amount;


        }

            private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Object obj in store.SelectedItems)
                if (!Cart.Items.Contains(obj)){
                    Cart.Items.Add(obj);
                }
               
            
        }
        private void Refresh(object sender, RoutedEventArgs e)
        {
            string conString = "Server=localhost;Database=webshop;Uid=donald;Pwd=Niek02102004;";
            MySqlConnection con = new MySqlConnection(conString);

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
            string conString = "Server=localhost;Database=webshop;Uid=donald;Pwd=Niek02102004;";
            MySqlConnection con = new MySqlConnection(conString);

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

            MySqlCommand cmd2 = new MySqlCommand("SELECT * from customer where username='"+ ValueFromLogin+"';", con);
            cmd2.ExecuteNonQuery();
            MySqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                double cash = reader2.GetDouble(2);
                Cash.Content = "€"+cash;
                
            }
            reader2.Close();

        }
    }
}
