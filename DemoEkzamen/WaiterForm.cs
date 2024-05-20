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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DemoEkzamen
{
    public partial class WaiterForm : Form
    {
        public WaiterForm()
        {
            InitializeComponent();
        }

        int selectedId = 0;
        string selectedStatus = "";

        string connectionString = "server=localhost;port=3306;user=root;password=;database=demoekzamen";

        MySqlConnection connection;


        public void GetOrders()
        {

            try
            {
                string sql = "SELECT order_id, order_status, description as order_description, cook.fio as order_cook, waiter.fio as order_waiter, order_date\r\nFROM orders\r\nINNER JOIN users as cook ON orders.cook_id = cook.user_id\r\nINNER JOIN users as waiter ON orders.waiter_id = waiter.user_id\r\nORDER BY order_date DESC;";

                DataTable dataTable = new DataTable();

                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;

            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Ошибка при получении заказов! {ex.Message}");
            }
            finally { connection.Close(); }
        }


        public void AddOrder(string cook, string descr)
        {
           try
            {
                string sql = $"INSERT INTO orders (cook_id, waiter_id, description, order_status) VALUES ({cook}, {Auth.user_id}, '{descr}', '{GlobalVars.ORDER_ACCEPET}');";

                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                int result = cmd.ExecuteNonQuery();
                connection.Close();

                if (result > 0)
                {
                    MessageBox.Show("Новый заказ создан успешно!");
                    GetOrders();

                }
            } catch
            {
                connection.Close();
                MessageBox.Show("Произошла ошибка при создании заказа!");
            }
        }

        public void GetCookers()
        {
            try
            {

                string sql = $"SELECT user_id, fio as '{GlobalVars.COOK_ROLE}' FROM users WHERE post = '{GlobalVars.COOK_ROLE}';";

                DataTable dataTable = new DataTable();

                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataTable);

                comboBox1.DisplayMember = GlobalVars.COOK_ROLE;
                comboBox1.ValueMember = "user_id";
                comboBox1.DataSource = dataTable;

            }
            finally { connection.Close(); }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void WaiterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();

        }

        private void WaiterForm_Load(object sender, EventArgs e)
        {
            connection = new MySqlConnection(connectionString);
        
            GetCookers();
            GetOrders();

        }

     

        public void updateOrderStatus(string status, int orderId)
        {

            if (status == GlobalVars.ORDER_ACCEPET || status == GlobalVars.ORDER_PAID)
            {
                try
                {
                    string sql = $"UPDATE orders SET order_status = '{status}' WHERE order_id = {orderId}";

                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    int result = cmd.ExecuteNonQuery();
                    connection.Close();

                    if (result > 0)
                    {
                        MessageBox.Show("Статус изменен успешно!");
                        GetOrders();

                    }

                }
                catch
                {
                    connection.Close();
                    MessageBox.Show("Произошла ошибка при обновлении статуса!");

                }

            }
            else
            {
                MessageBox.Show("Передан некорректный статус!");
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        public bool CheckPostBox()
        {
            if (comboBox1.SelectedValue != null)
            {
                return true;
            }
            return false;
        }

        public bool CheckTextBoxes()
        {
            if (textBox1.Text.Length > 4)
            {
                return true;
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           if (CheckPostBox() && CheckTextBoxes())
            {
                AddOrder(comboBox1.SelectedValue.ToString(), textBox1.Text);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                selectedId = id;
                label17.Text = $"ID заказа: {id}";

            }
            catch
            {
                MessageBox.Show("Ошибка при конвертации OrderId");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (selectedId != 0 && selectedStatus != "")
            {
                updateOrderStatus(selectedStatus, selectedId);
            }
            else
            {
                MessageBox.Show("Выберите статус или заказ!");
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedItem != null)
            {
                string status = comboBox4.SelectedItem.ToString();
                selectedStatus = status;
            }
        }
    }
}
