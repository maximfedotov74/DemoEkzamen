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
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
        }

        int selectedId = 0;
        string selectedStatus = "";

        string connectionString = "server=localhost;port=3306;user=root;password=;database=demoekzamen";

        MySqlConnection connection;

        public void GetUsers()
        {

            try
            {
                string sql = $"SELECT user_id, post as user_post, fio as user_fio, status as user_status FROM users WHERE post = '{GlobalVars.COOK_ROLE}' OR post = '{GlobalVars.WAITER_ROLE}' ORDER BY user_id DESC;";

                DataTable dataTable = new DataTable();
         

                connection.Open();
               

                MySqlCommand cmd = new MySqlCommand(sql, connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
                


            }
            catch 
            {

                MessageBox.Show("Ошибка при получении пользователей!");
            }
            finally {
                connection.Close();

            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            connection = new MySqlConnection(connectionString);
            GetUsers();
            GetCookers();
            GetWaiters();
            GetOrders();
            GetWaitersShift();
            GetCookersShift();
        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
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
                dataGridView2.DataSource = dataTable;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении заказов! {ex.Message}");
            }
            finally { connection.Close(); }
        }

        public void GetCookers()
        {
            try {

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

        public void GetWaiters()
        {
            try
            {

                string sql = $"SELECT user_id, fio as '{GlobalVars.WAITER_ROLE}' FROM users WHERE post = '{GlobalVars.WAITER_ROLE}';";

                DataTable dataTable = new DataTable();

                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataTable);

                comboBox2.DisplayMember = GlobalVars.WAITER_ROLE;
                comboBox2.ValueMember = "user_id";
                comboBox2.DataSource = dataTable;

            }
            finally { connection.Close(); }
        }

        public void GetCookersShift()
        {
            try
            {
                string sql = $"SELECT shift.shift_id as shift_cook_id, cook.fio as cook, shift.start_date as shift_cook_start_date, shift.end_date as shift_cook_end_date FROM shift INNER JOIN users as cook ON shift.user_id = cook.user_id WHERE cook.post = '{GlobalVars.COOK_ROLE}' ORDER BY shift.shift_id DESC;";

                DataTable dataTable = new DataTable();

                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataTable);
                dataGridView3.DataSource = dataTable;
            } finally { connection.Close(); }
        }

        public void GetWaitersShift()
        {
            try
            {
                string sql = $"SELECT shift.shift_id as shift_waiter_id, waiter.fio as waiter, shift.start_date as shift_waiter_start_date, shift.end_date as shift_waiter_end_date FROM shift INNER JOIN users as waiter ON shift.user_id = waiter.user_id WHERE waiter.post = '{GlobalVars.WAITER_ROLE}' ORDER BY shift.shift_id DESC;";

                DataTable dataTable = new DataTable();

                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataTable);
                dataGridView4.DataSource = dataTable;
            } finally { connection.Close(); }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
          
        }

        public void addNewShift(string user, string start, string end)
        {
            try
            {
                string sql = $"INSERT INTO shift (user_id, start_date, end_date) VALUES ({user}, '{start}', '{end}');";

                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                int result = cmd.ExecuteNonQuery();
                connection.Close();

                if (result > 0)
                {
                    MessageBox.Show("Новая смена создана успешно!");
                    GetWaitersShift();
                    GetCookersShift();
                }

            } catch
            {
                connection.Close();
            }
        }

        public void addNewUser(string post, string fio, string password, string login)
        {
           try
            {

                string hash = HashPassword.Hash(password);

                string sql = $"INSERT INTO users (login, password_hash, fio, post, status) VALUES ('{login}', '{hash}', '{fio}', '{post}', '{GlobalVars.WORK_STATUS}');";

                connection.Open();

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                int result = cmd.ExecuteNonQuery();
                connection.Close();

                if (result > 0)
                {
                    MessageBox.Show("Новый пользователь создан успешно!");
                    GetUsers();
                    GetCookers();
                    GetWaiters();


                }

            } catch 
            {
                connection.Close();
                MessageBox.Show("Произошла ошибка при создании пользователя!");
            }
        }

        public void updateUserStatus(string status, int userId) { 

            if (status == GlobalVars.WORK_STATUS || status == GlobalVars.DISMISSED_STATUS)
            {
               try
                {
                    string sql = $"UPDATE users SET status = '{status}' WHERE user_id = {userId}";

                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    int result = cmd.ExecuteNonQuery();
                    connection.Close();

                    if (result > 0)
                    {
                        MessageBox.Show("Статус изменен успешно!");
                        GetUsers();
                    } 

                } catch {
                    connection.Close();
                    MessageBox.Show("Произошла ошибка при обновлении статуса!");

                } 

            } else
            {
                MessageBox.Show("Передан некорректный статус!");
            }  
        }

        public void Combobox_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                ComboBox comboBox = sender as ComboBox;

                int userId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString().Trim());
                updateUserStatus(comboBox.SelectedItem.ToString(), userId);
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при конвертации данных: User ID");
            }



        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

           
            if (CheckTextBoxes() && CheckPostBox())
            {
                addNewUser(comboBox3.SelectedItem.ToString(), textBox1.Text, textBox3.Text, textBox2.Text);
            }
            
        }

        public bool CheckPostBox()
        {
            if (comboBox3.SelectedItem != null)
            {
                string post = comboBox3.SelectedItem.ToString();
                if (post == GlobalVars.ADMIN_ROLE || post == GlobalVars.COOK_ROLE || post == GlobalVars.WAITER_ROLE)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckTextBoxes()
        {
            if (textBox2.Text.Length > 4 && textBox1.Text.Length > 8 && textBox3.Text.Length > 4)
            {
                return true;
            }
            return false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
           
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            if  (comboBox4.SelectedItem != null)
            {
                string status = comboBox4.SelectedItem.ToString();
                selectedStatus = status;
            }
            

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (selectedId != 0 && selectedStatus != "")
            {
                updateUserStatus(selectedStatus, selectedId);
            } else
            {
                MessageBox.Show("Выберите статус или пользователя!");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                selectedId = id;
                label17.Text = $"ID пользователя: {id}";

            }
            catch
            {
                MessageBox.Show("Ошибка при конвертации UserId");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (comboBox1.SelectedValue != null)
            {
                string start = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string end = dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss");

                addNewShift(comboBox1.SelectedValue.ToString(), start, end);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedValue != null)
            {
                string start = dateTimePicker4.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string end = dateTimePicker3.Value.ToString("yyyy-MM-dd HH:mm:ss");

                addNewShift(comboBox2.SelectedValue.ToString(), start, end);
            }
        }
    }
}
