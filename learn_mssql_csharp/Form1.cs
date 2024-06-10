using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data;
using System.Linq;

using System.Configuration;
using System.Data.SqlClient;

namespace learn_mssql_csharp
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;

        private List<string[]> rows = null;
        private List<string[]> filteredList = null;

        //т.к таблицы слились, новое подключение не нужно

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString);

            sqlConnection.Open();

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
                "SELECT * FROM Products", 
                sqlConnection);

            DataSet dataSet = new DataSet();

            sqlDataAdapter.Fill(dataSet);

            dataGridView2.DataSource = dataSet.Tables[0];

            //для вкладки LV Filter
            rows = new List<string[]>();

            SqlDataReader sqlDataReader = null;

            string[] row = null;

            try
            {
                SqlCommand sqlCommand = new SqlCommand(
                    "SELECT ProductName, QuantityPerUnit, UnitPrice FROM Products",
                    sqlConnection);

                sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    row = new string[]
                    {
                        Convert.ToString(sqlDataReader["ProductName"]),
                        Convert.ToString(sqlDataReader["QuantityPerUnit"]),
                        Convert.ToString(sqlDataReader["UnitPrice"])
                    };

                    rows.Add(row);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlDataReader != null && !sqlDataReader.IsClosed)
                {
                    sqlDataReader.Close();
                }

            }

            RefreshList(rows);
        }

        private void RefreshList(List<string[]> list)
        {
            listView2.Items.Clear();

            foreach (string[] s in list)
            {
                listView2.Items.Add(new ListViewItem(s));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand(
                "INSERT INTO [Students] (Name, Surname, Birthday, Birthplace, Phone, Email)" +
                "VALUES (@Name, @Surname, @Birthday, @Birthplace, @Phone, @Email)",
                sqlConnection
                );

            DateTime dateTime = DateTime.Parse(textBox3.Text);

            sqlCommand.Parameters.AddWithValue("Name", textBox1.Text);
            sqlCommand.Parameters.AddWithValue("Surname", textBox2.Text);
            sqlCommand.Parameters.AddWithValue("Birthday", $"{dateTime.Month}/{dateTime.Day}/{dateTime.Year}");
            sqlCommand.Parameters.AddWithValue("Birthplace", textBox4.Text);
            sqlCommand.Parameters.AddWithValue("Phone", textBox5.Text);
            sqlCommand.Parameters.AddWithValue("Email", textBox6.Text);

            MessageBox.Show(sqlCommand.ExecuteNonQuery().ToString());


        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //если писать запрос через конкатенацию, а не слитно, но скрипт не работает почему-то
            //SELECT * FROM Products WHERE UnitPrice > 100
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
                textBox7.Text,
                sqlConnection);

            DataSet dataSet = new DataSet();

            sqlDataAdapter.Fill(dataSet);

            dataGridView1.DataSource = dataSet.Tables[0];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            SqlDataReader sqlDataReader = null;

            try
            {
                SqlCommand sqlCommand = new SqlCommand(
                    "SELECT CategoryID, CategoryName " +
                    "FROM Categories",
                    sqlConnection
                    );

                sqlDataReader = sqlCommand.ExecuteReader();

                ListViewItem listViewItem = null;

                while (sqlDataReader.Read())
                {
                    listViewItem = new ListViewItem( new string[]
                    {
                        Convert.ToString(sqlDataReader["CategoryID"]),
                        Convert.ToString(sqlDataReader["CategoryName"])
                    });

                    listView1.Items.Add(listViewItem);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if(sqlDataReader != null && !sqlDataReader.IsClosed)
                {
                    sqlDataReader.Close();
                }
            }

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"ProductName LIKE '%{textBox8.Text}%'";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    {
                        (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "UnitsInStock < 10";
                        break;
                    }
                case 1:
                    {
                        (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "UnitsInStock >= 10 AND UnitxInStock < 50";
                        break;
                    }
                case 2:
                    {
                        (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "UnitsInStock >= 50";
                        break;
                    }
                default:
                    {
                        (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "";
                        break;
                    }

            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            filteredList = rows.Where((x) => 
                x[0].ToLower().Contains(textBox9.Text.ToLower())).ToList();

            RefreshList(filteredList);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    {
                        filteredList = rows.Where((x) =>
                            Double.Parse(x[2]) <= 10).ToList();
                        RefreshList(filteredList);
                        break;
                    }
                case 1:
                    {
                        filteredList = rows.Where((x) =>
                            Double.Parse(x[2]) > 10 && Double.Parse(x[2]) <= 100).ToList();
                        RefreshList(filteredList);
                        break;
                    }
                case 2:
                    {
                        filteredList = rows.Where((x) =>
                            Double.Parse(x[2]) > 100).ToList();
                        RefreshList(filteredList);
                        break;
                    }
                case 3:
                    {
                        RefreshList(rows);
                        break;
                    }

            }
        }
    }
}
