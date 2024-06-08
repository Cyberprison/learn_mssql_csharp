using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Configuration;
using System.Data.SqlClient;

namespace learn_mssql_csharp
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;

        //т.к таблицы слились, новое подключение не нужно

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString);

            sqlConnection.Open();
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
    }
}
