using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoEkzamen
{
    public partial class AuthForm : Form
    {
        public AuthForm()
        {
            InitializeComponent();
        }


        string connectionString = "server=localhost;port=3306;user=root;password=;database=demoekzamen";

        MySqlConnection connection;



        public void Authorization(string login, string password)
        {
            try
            {

                string hash = HashPassword.Hash(password);

                string sql = $"SELECT user_id, login ,fio, post, status FROM users WHERE login = '{login}' AND password_hash = '{hash}';";

                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    MessageBox.Show("Неверный логин или пароль!");
                    return;
                }

                while (reader.Read()) {
                    Auth.auth = true;
                    Auth.user_id = Convert.ToInt32(reader[0]);
                    Auth.login = reader[1].ToString();
                    Auth.fio = reader[2].ToString();
                    Auth.post = reader[3].ToString();
                    Auth.status = reader[4].ToString(); 
                }

                if (Auth.auth)
                {
                    if (Auth.status == GlobalVars.DISMISSED_STATUS)
                    {
                        MessageBox.Show("Доступ запрещен!");
                        return;
                    }

                    if (Auth.post == GlobalVars.ADMIN_ROLE)
                    {
                        ClearFields();
                        this.Hide();
                        AdminForm admin_form = new AdminForm();
                        admin_form.Show();
                        return;
                    } else if (Auth.post == GlobalVars.COOK_ROLE)
                    {
                        ClearFields();
                        this.Hide();
                        CookForm cook_form = new CookForm();
                        cook_form.Show();
                        return;
                    } else if  (Auth.post == GlobalVars.WAITER_ROLE)
                    {
                        ClearFields();
                        this.Hide();
                        WaiterForm waiter_form = new WaiterForm();
                        waiter_form.Show();
                        return;
                    } else
                    {
                        ClearFields();
                        MessageBox.Show("Доступ запрещен!");
                        return;
                    }
                  
                }


            }
            catch
            {
                MessageBox.Show("Произошла ошибка при авторизации, попробуйте позже!");
                return;
            }

            finally { connection.Close(); } 
        }

        private void AuthForm_Load(object sender, EventArgs e)
        {
            connection = new MySqlConnection(connectionString);

        }

        public void ClearFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 0) {

                Authorization(textBox1.Text, textBox2.Text);
            
            }
        }
    }
}
