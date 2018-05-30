using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;


namespace practicum_webshop
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        Constants c = new Constants();
        public Window1()
        {
            InitializeComponent();
            
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            char[] chararray = UsernameInput.Text.ToCharArray();
            Array.Reverse(chararray);
            string reverse = "";
            for (int i = 0; i <= chararray.Length - 1; i++)
            {
                reverse += chararray.GetValue(i);
            }
            password.Content = reverse;
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {

            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection(c.dbConnection);           

            con.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT count(*) from customer where username='" + UsernameInput.Text +"';", con);
            int count = Convert.ToInt32(cmd.ExecuteScalar());


            if(count == 0)
            {
                string query2 = "INSERT INTO customer (username, password, cash) VALUES('" + UsernameInput.Text + "','" + password.Content + "', 10.00);";
                MySqlCommand cmd2 = new MySqlCommand(query2, con);
                cmd2.ExecuteNonQuery();
                MessageBox.Show("Account with name " + UsernameInput.Text + " has been created!");
            }
            else
            {
                MessageBox.Show("This username already exists! Choose another one.");
            }
        }
    }
}
