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
    public partial class CookForm : Form
    {
        public CookForm()
        {
            InitializeComponent();
        }


        int selectedId = 0;
        string selectedStatus = "";

        string connectionString = "server=localhost;port=3306;user=root;password=;database=demoekzamen";

        MySqlConnection connection;

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void CookForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();

        }

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

        public void updateOrderStatus(string status, int orderId)
        {

            if (status == GlobalVars.ORDER_PREPARING || status == GlobalVars.ORDER_COMPLETED)
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


        private void CookForm_Load(object sender, EventArgs e)
        {
            connection = new MySqlConnection(connectionString);
            GetOrders();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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

        private void button4_Click_1(object sender, EventArgs e)
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
