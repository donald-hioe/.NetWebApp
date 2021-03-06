﻿using MySql.Data.MySqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using practicum_webshop;



namespace practicum_webshop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Constants c = new Constants();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = new MySqlConnection(c.dbConnection);

            con.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT count(*) from customer where username=@username and password =@password;", con);
            cmd.Parameters.AddWithValue("@username", UsernameInput.Text);
            cmd.Parameters.AddWithValue("@password", PasswordInput.Text);
            int count = Convert.ToInt32(cmd.ExecuteScalar());

            if (count == 1)
            {
                Window2 win2 = new Window2(UsernameInput.Text);
                win2.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong password or username!");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Window1 win1 = new Window1();
            win1.Show();
            this.Close();
        }
        
    }
}
