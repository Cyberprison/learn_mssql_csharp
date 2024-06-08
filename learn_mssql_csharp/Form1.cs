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
using System.Data;
using System.Data.SqlClient;

namespace learn_mssql_csharp
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString);

            sqlConnection.Open();

            if(sqlConnection.State == ConnectionState.Open)
            {
                MessageBox.Show("Server connected"); 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand(
                //"INSERT INTO [Students] (Name, Surname, Birthday, Birthplace, Phone, Email)" +
                //"VALUES (N'Алекс', N'Алексеев', '02/02/2002', N'Москва', N'1', N'а')",

                //"INSERT INTO [Students] (Name, Surname, Birthday, Birthplace, Phone, Email)" +
                //$"VALUES (N'{textBox1.Text}', N'{textBox2.Text}', '{textBox3.Text}', N'{textBox4.Text}', N'{textBox5.Text}', N'{textBox6.Text}')",

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
    }
}
