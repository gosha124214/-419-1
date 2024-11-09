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
    public partial class Form3 : Form
    {
        DataBase class1 = new DataBase();

        enum RowState
        {
            Existed,
            New,
            Modified,
            ModifiedNew,
            Deleted
        }

        int selectedRow;

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("IdClient", "ID клиента");
            dataGridView1.Columns.Add("startDate", "Дата");
            dataGridView1.Columns.Add("IdTech", "Номер техники");
            dataGridView1.Columns.Add("problemDescryption", "Проблема");
            dataGridView1.Columns.Add("completionData", "дата окончания");
            dataGridView1.Columns.Add("IdMaster", "ID мастера");

        }



        private void ReadSingleRow(DataGridView dgv, IDataRecord record)
        {



            string startDate = (record.GetValue(0) == DBNull.Value) ? string.Empty : record.GetString(0);
            int IdTech = (record.GetValue(1) == DBNull.Value) ? 0 : (int)record.GetValue(1);

            string problemDescryption = (record.GetValue(2) == DBNull.Value) ? string.Empty : record.GetString(2);
            string completionData = (record.GetValue(3) == DBNull.Value) ? string.Empty : record.GetString(3);

            string repairParts = (record.GetValue(4) == DBNull.Value) ? string.Empty : record.GetString(4);
            int IdMaster = (record.GetValue(5) == DBNull.Value) ? 0 : (int)record.GetValue(5);

            int IdClient = (record.GetValue(6) == DBNull.Value) ? 0 : (int)record.GetValue(6);



            dgv.Rows.Add(IdClient, startDate,
               IdTech, problemDescryption,
               completionData, IdMaster,  
               RowState.ModifiedNew);

        }


        private void RefreshDataGridClient(DataGridView dgw)
        {

            dgw.Rows.Clear();

            string queryString = $"select startDate, IdTech, problemDescryption, completionDate, repairParts, IdMaster, IdClient from [dbo].[Requests] where IdClient = {ClientInfo.IdRole}";

            SqlCommand command = new SqlCommand(queryString, class1.getConnection());

            class1.openConnection();


            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }


        public Form3()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            this.Hide();
            form4.ShowDialog();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            this.Hide();
            form2.ShowDialog();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGridClient(dataGridView1);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            RefreshDataGridClient(dataGridView1);
            Search(dataGridView1);
            
        }

        private void Search(DataGridView dvg)
        {


            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();

            SqlDataAdapter adapter2 = new SqlDataAdapter();
            DataTable dt2 = new DataTable();

            dvg.Rows.Clear();


            string queryString = $"select startDate, IdTech, problemDescryption, completionDate, repairParts, IdMaster, IdClient from [Likhoy429191-12].[dbo].[Requests] where IdClient = {ClientInfo.IdRole}";

            SqlCommand sqlcommand = new SqlCommand(queryString, class1.getConnection());

            adapter2.SelectCommand = sqlcommand;
            adapter2.Fill(dt2);

            string searchSQL = $"select startDate, IdTech, problemDescryption, completionDate, repairParts, IdMaster, IdClient from [Likhoy429191-12].[dbo].[Requests] where IdClient = {ClientInfo.IdRole} and concat (startDate, IdTech, problemDescryption, completionDate, repairParts, IdMaster, IdClient) like '%" + textBox1.Text + "%'";
            SqlCommand command = new SqlCommand(searchSQL,class1.getConnection());


            adapter.SelectCommand = command;
            adapter.Fill(dt);

            label1.Text =$"Дано: {dt.Rows.Count.ToString()} из {dt2.Rows.Count.ToString()}";

            class1.openConnection();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(dvg, reader);
            
            }
            reader.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Change();
            RefreshDataGridClient(dataGridView1);

        }

        private void Change()
        {
            class1.openConnection();
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var dataTime = textBox4.Text;
            var namderTehnic = textBox2.Text;
            var whatProblem = textBox3.Text;


            var changeQuery = $"update Requests set IdTech = '{namderTehnic}', problemDescryption = '{whatProblem}' where startDate = '{dataTime}' ";

            var command = new SqlCommand(changeQuery, class1.getConnection());
            command.ExecuteNonQuery();


            class1.closeConection();


        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                textBox4.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                textBox3.Text = row.Cells[3].Value.ToString();
                
            }
        }
    }
}
