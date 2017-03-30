using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parsercbr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //ru.cbr.www.DailyInfo di = new ru.cbr.www.DailyInfo();

        //вывести список доступных валют
        private void button1_Click(object sender, EventArgs e)
        {
            ru.cbr.www.DailyInfo di = new ru.cbr.www.DailyInfo();
            DataSet ds = new DataSet();
            if (checkBox1.Checked) ds = di.EnumValutes(false); //False - перечень ежедневных валют
            else ds = di.EnumValutes(true); //True - перечень ежемесячных валют
            dataGridView1.DataSource = ds.Tables["EnumValutes"];
            dataGridView1.Columns[0].HeaderText = "Внутренний код валюты";
            dataGridView1.Columns[1].HeaderText = "Название валюты";
            dataGridView1.Columns[2].HeaderText = "Англ. название валюты";
            dataGridView1.Columns[3].HeaderText = "Номинал";
            dataGridView1.Columns[4].HeaderText = "Внутренний код валюты, являющейся 'базовой'";
            dataGridView1.Columns[5].HeaderText = "цифровой код ISO";
            dataGridView1.Columns[6].HeaderText = "3х буквенный код ISO";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false) checkBox2.Checked = true;
            else checkBox2.Checked = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == false) checkBox1.Checked = true;
            else checkBox1.Checked = false;
        }

        //Вывести динамику курса выбранной валюты на график
        private void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView1.DataSource==null)
            {
                MessageBox.Show("Сначала получите перечень валют!", "Сообщение!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ru.cbr.www.DailyInfo di = new ru.cbr.www.DailyInfo();
            System.DateTime DateFrom, DateTo;
            DateFrom = dateTimePicker1.Value;
            DateTo = dateTimePicker2.Value;

            //Вызываем GetCursDynamic для получения таблицы с курсами заданной валютой
            DataSet Ds = (System.Data.DataSet)di.GetCursDynamic(DateFrom, DateTo, dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString());
            Ds.Tables[0].Columns[0].ColumnName = "Дата";
            Ds.Tables[0].Columns[1].ColumnName = "Вн.код валюты";
            Ds.Tables[0].Columns[2].ColumnName = "Номинал";
            Ds.Tables[0].Columns[3].ColumnName = "Курс";
                        
            chart1.DataSource = Ds;
            chart1.Series[0].XValueMember = (Ds.Tables[0].Columns[0]).ToString();
            chart1.Series[0].YValueMembers = (Ds.Tables[0].Columns[3]).ToString();
            chart1.Series[0].LegendText = dataGridView1[6, dataGridView1.CurrentRow.Index].Value.ToString();
            chart1.DataBind();
        }
    }
}
