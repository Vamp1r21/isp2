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

namespace pop2
{
    public partial class Form1 : Form
    {
        string str = "server=localhost; user=root; password= 1234; database= chernovik; port= 3306";
        int s = 0;
        int f = 0;
        string sql = "";
        public Form1()
        {
            InitializeComponent();
            label1.Hide();
            label2.Hide();
            comboBox1.Hide();
            comboBox1.Items.Clear();
            MySqlConnection connection = new MySqlConnection(str);
            connection.Open();
            sql = "Select * from type_material";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
            DataTable ds = new DataTable();
            adapter.Fill(ds);
            for(int i=0;i<ds.Rows.Count; i++)
            {
                comboBox1.Items.Add(ds.Rows[i]["Name"]);
            }
        }

        public void tabl(string sql)
        {
            MySqlConnection connection = new MySqlConnection(str);
            connection.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Hide();
            s = 0;
            MySqlConnection connection = new MySqlConnection(str);
            sql = $"Select * from materials";
            comboBox1.Show();
            tabl(sql + $"  Limit { s},{ s + 15}");
            sql = "Select Count(ID) from materials";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            f = Int32.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
            if (f > 15) label2.Show();
            sql = $"Select * from materials";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dmeriwmew
            s = 0;
            label1.Hide();
            MySqlConnection connection = new MySqlConnection(str);
            sql = "Select count(materials.ID) from materials inner join type_material on materials.idmateria=type_material.ID and " +
                $"type_material.Name='{comboBox1.Text}'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            f = Int32.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
            if (f < 15) label2.Hide();
            else label2.Show();
            sql = $"Select * from materials inner join type_material on materials.idmateria=type_material.ID and " +
                $"type_material.Name='{comboBox1.Text}'";
            tabl(sql+ $"  Limit { s},{ s + 15}");
        }

        private void label2_Click(object sender, EventArgs e)
        {
            s += 15;
            label1.Show();
            tabl(sql + $"  Limit { s},{ s + 15}");
            if (f < s + 15) label2.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            s -= 15;
            label2.Show();
            tabl(sql + $"  Limit { s},{ s + 15}");
            if (s==0) label1.Hide();
        }
    }
}
