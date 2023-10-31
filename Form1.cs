using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Net.Configuration;

namespace codigo_1._2
{
    public partial class Form1 : Form
    {
        SqlConnection conexion;
        SqlCommand comando;
        
        public Form1()
        {
            InitializeComponent();
            conexion = new SqlConnection("Data Source=DESKTOP-U982URJ;Initial Catalog=\"Estudiantes 1\";Integrated Security=True");
            comando = new SqlCommand("Select * from Alumno ", conexion);


            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(conexion.State == ConnectionState.Closed)
            {
                button1.Text = "Conectado";
                conexion.Open();
                button1.BackColor = Color.Green;
                Leer();

                

            }
            else
            {
                button1.Text = "Desconectado";
                conexion.Close();
                button1.BackColor= Color.Red;
                dataGridView1.Rows.Clear(); 


            }
        }
   
        private void Leer(string PdeTabla = "Select * from Alumno",DataGridView Pdgv=null)
        {
            comando.CommandText = PdeTabla;
            SqlDataReader Dr = comando.ExecuteReader();
            if (Pdgv == null) { Pdgv = dataGridView1; }
            Pdgv.Rows.Clear();
            while (Dr.Read())
            {

                Pdgv.Rows.Add(new object[] { Dr[0], Dr[1], Dr[2] });


            }
            Dr.Close();




        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void button2_Click(object sender, EventArgs e)
        {


            int Legajo = int.Parse(Interaction.InputBox("Ingrese el Legajo:"));
            string Nombre = Interaction.InputBox("Ingrese el Nombre:");
            string Apellido = Interaction.InputBox("Ingrese el  Apellido");
            comando.CommandText = $"Insert into Alumno (Legajo,Nombre,Apellido) Values ({Legajo},'{Nombre}','{Apellido}')";
            comando.ExecuteNonQuery();
            Leer();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count < 0) throw new Exception("No hay nada para eliminar!!!");
                {

                    comando.CommandText = $"Delete from Alumno Where Legajo = {dataGridView1.SelectedRows[0].Cells[0].Value}";
                    comando.ExecuteNonQuery();
                    Leer();




                }




            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            try
            {

                if (dataGridView1.Rows.Count < 0) throw new Exception("No hay nada que modificar!!!");

                string Nombre = Interaction.InputBox("Ingrese el Nombre:");
                string Apellido = Interaction.InputBox("Ingrese el  Apellido");
                comando.CommandText = $"Update Alumno set Nombre = '{Nombre}', Apellido= '{Apellido}' where Legajo ={dataGridView1.SelectedRows[0].Cells[0].Value}";
                comando.ExecuteNonQuery();
                Leer();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text == "") throw new Exception("Debe ingresar un filtro de busqueda!!!");
                if (dataGridView1.Rows.Count == 0) throw new Exception("No hay alumnnos para Consultar!!!");
                var ingresoConsulta = Interaction.InputBox($"Ingrese el {comboBox1.SelectedItem}: ");
                if (comboBox1.SelectedIndex == 0 && !Information.IsNumeric(ingresoConsulta)) throw new Exception("El Legajo debe ser numérico !!!");
                var comilla = comboBox1.SelectedIndex == 0 ? "" : "'";
                Leer($"select * from Alumno where {comboBox1.SelectedItem}={comilla}{ingresoConsulta}{comilla}", dataGridView2);

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
