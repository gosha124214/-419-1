using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_4
{
    public partial class Form2 : Form
    {

        DataBase class1 = new DataBase();


        public Form2()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            this.Hide();
            form3.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            class1.openConnection();
            var dataTime = dateTimePicker1.Value;
            var IdTech= textBox1.Text;;
            var problemDescryption = textBox2.Text;;

            var addQuery = $"insert into [dbo].[Requests] (startDate, IdTech, problemDescryption, IdClient) values('{dataTime}','{IdTech}','{problemDescryption}','{ClientInfo.IdRole}')";

            var command = new SqlCommand(addQuery, class1.getConnection());

            command.ExecuteNonQuery();

            class1.closeConection();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
