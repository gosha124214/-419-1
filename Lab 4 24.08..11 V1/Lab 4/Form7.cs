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
    public partial class Form7 : Form
    {
        DataBase class1 = new DataBase();

        int selectedRow;
        enum RowState
        {
            Existed,
            New,
            Modified,
            ModifiedNew,
            Deleted
        }
        public Form7()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            class1.openConnection();

            var IdRequests = textBox2.Text;

            var IdMaster = textBox3.Text;
            var dataTime = textBox4.Text;

            var addQuery = $" update Requests set IdRequest = '{IdRequests}', IdMaster = '{IdMaster}' where startDate = '{dataTime}'";

            var command = new SqlCommand(addQuery, class1.getConnection());

            command.ExecuteNonQuery();

            class1.closeConection();

            RefreshDataGridOperator(dataGridView1);

        }

        private void RefreshDataGridOperator(DataGridView dgw)
        {

            dgw.Rows.Clear();

            string queryString = $"select * from [dbo].[Requests]";

            SqlCommand command = new SqlCommand(queryString, class1.getConnection());

            class1.openConnection();


            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }
        private void Search(DataGridView dvg)
        {


            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();

            SqlDataAdapter adapter2 = new SqlDataAdapter();
            DataTable dt2 = new DataTable();

            dvg.Rows.Clear();


            string queryString = $"select * from [Likhoy429191-12].[dbo].[Requests]";

            SqlCommand sqlcommand = new SqlCommand(queryString, class1.getConnection());

            adapter2.SelectCommand = sqlcommand;
            adapter2.Fill(dt2);

            string searchSQL = $"select * from [Likhoy429191-12].[dbo].[Requests] where concat (startDate, IdTech, problemDescryption, completionDate, repairParts, IdMaster, IdClient) like '%" + textBox1.Text + "%'";
            SqlCommand command = new SqlCommand(searchSQL, class1.getConnection());


            adapter.SelectCommand = command;
            adapter.Fill(dt);

            label1.Text = $"Дано: {dt.Rows.Count.ToString()} из {dt2.Rows.Count.ToString()}";

            class1.openConnection();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(dvg, reader);

            }
            reader.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            RefreshDataGridOperator(dataGridView1);
            Search(dataGridView1);
        }

        private void ReadSingleRow(DataGridView dgv, IDataRecord record)
        {
            int IdRrequest = (record.GetValue(8) == DBNull.Value) ? 0 : (int)record.GetValue(8);
            string startDate = (record.GetValue(0) == DBNull.Value) ? string.Empty : record.GetString(0);
            int IdTech = (record.GetValue(1) == DBNull.Value) ? 0 : (int)record.GetValue(1);

            string problemDescryption = (record.GetValue(2) == DBNull.Value) ? string.Empty : record.GetString(2);
            string completionData = (record.GetValue(3) == DBNull.Value) ? string.Empty : record.GetString(3);

            string repairParts = (record.GetValue(4) == DBNull.Value) ? string.Empty : record.GetString(4);
            int IdMaster = (record.GetValue(6) == DBNull.Value) ? 0 : (int)record.GetValue(6);

            int IdClient = (record.GetValue(5) == DBNull.Value) ? 0 : (int)record.GetValue(5);



            dgv.Rows.Add(IdRrequest,IdClient, startDate,
               IdTech, problemDescryption,
               completionData, IdMaster,
               RowState.ModifiedNew);

        }

        private void Form7_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGridOperator(dataGridView1);
        }


        private void CreateColumns()
        {
            dataGridView1.Columns.Add("IdRrequest", "ID Заявки");
            dataGridView1.Columns.Add("IdClient", "ID клиента");
            dataGridView1.Columns.Add("startDate", "Дата");
            dataGridView1.Columns.Add("IdTech", "Номер техники");
            dataGridView1.Columns.Add("problemDescryption", "Проблема");
            dataGridView1.Columns.Add("completionData", "дата окончания");
            dataGridView1.Columns.Add("IdMaster", "ID мастера");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                textBox4.Text = row.Cells[2].Value.ToString();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            this.Hide();
            form5.ShowDialog();
        }
    }
}
