using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Lab_4
{
    public partial class Form1 : Form
    {
        DataBase class1 = new DataBase();

        int numberGo = 4;
        int TimerBan =3;
        int i;

        string[] fn = Directory.GetFiles(Application.StartupPath + "\\Capcha", "*.jpg");

        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            NamberCAPCHA();
        }


        public int NamberCAPCHA()
        {

            Random random = new Random();
            i = random.Next(0, 11);

            pictureBox1.Image = Image.FromFile(fn[i]);

            return i;
        }

        enum TypeRole : byte { Client =1, Operator, Master, Menager }


        private void button1_Click(object sender, EventArgs e)
        {

            var Login = textBox1.Text;
            var Password = textBox2.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();

            string sql = $"select [Log], Pas, TypeAndRole.IdType, Users.IdUser, IdRole, FIO, Type from [dbo].[Users] JOIN TypeAndRole ON Pas = '{Password}' and [Log] = '{Login}'  JOIN [dbo].[Type]  ON Users.IdUser = TypeAndRole.IdUser and TypeAndRole.IdType =[Type].[IdType]";

            SqlCommand sqlCommand = new SqlCommand(sql, class1.getConnection());

            adapter.SelectCommand = sqlCommand;
            adapter.Fill(dt);


            var CAPCHA = textBox3.Text;


            if ((dt.Rows.Count == 1) && (numberGo == 4 || CAPCHA == Path.GetFileNameWithoutExtension(fn[i])))
            {
                ClientInfo.IdType = (int)dt.Rows[0][2];
                ClientInfo.IdUser = (int)dt.Rows[0][3];
                ClientInfo.IdRole = (int)dt.Rows[0][4];

                ClientInfo.FIO= (string)dt.Rows[0][5];
                ClientInfo.Role = (string)dt.Rows[0][6];

                numberGo = 4;
                MessageBox.Show("Успешный вход", "Успешный вход", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (ClientInfo.IdType == (int)TypeRole.Client)
                {

                    Form2 form2 = new Form2();

                    this.Hide();
                    form2.ShowDialog();
                }
                else if (ClientInfo.IdType == (int)TypeRole.Operator)
                {
                    Form7 form7 = new Form7();

                    this.Hide();
                    form7.ShowDialog();

                }

                else if (ClientInfo.IdType == (int)TypeRole.Master)
                {

                    Form8 form8 = new Form8();

                    this.Hide();
                    form8.ShowDialog();
                }

                else if (ClientInfo.IdType == (int)TypeRole.Menager)
                {
                    Form2 form2 = new Form2();

                    this.Hide();
                    form2.ShowDialog();
                }
               

            }

            else if ((dt.Rows.Count == 1) && (CAPCHA != Path.GetFileNameWithoutExtension(fn[i])))
            {

                MessageBox.Show("Вы не прошли CAPCHA.\nПопробуйте ещё раз.", "CAPCHA не верна", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else
            {

                numberGo--;

                switch (numberGo)
                {

                    case 3:

                        pictureBox1.Visible = true;

                        label4.Visible = true;

                        textBox3.Visible = true;

                        button2.Visible = true;

                        MessageBox.Show($"Ваше оставшееся колечество попыток равно: {numberGo}", "Неуспешный вход", MessageBoxButtons.OK, MessageBoxIcon.Warning);



                        break;

                    case 2:

                        button1.Enabled = false;
                        timer1.Enabled = true;

                        MessageBox.Show($"Ваше оставшееся колечество попыток равно: {numberGo}.\nВ связи с этим Вам отказанно в доступе на {30} секунд", "Вы забанены", MessageBoxButtons.OK, MessageBoxIcon.Warning);



                        break;

                    case 1:

                        button1.Enabled = false;
                        MessageBox.Show("Вы были заблокированы до перезагрузки программы", "Вы заблокированны", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        break;

                }
                NamberCAPCHA();
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar= '*';
            //textBox3.UseSystemPasswordChar= false;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = $"До разбана осталось: {TimerBan.ToString()} секунд";

            TimerBan--;

            if(TimerBan<-1) 
            { 
                button1.Enabled = true;
                timer1.Enabled = false;
                label3.Text = "Вводите";
            }
        }


    }
}
