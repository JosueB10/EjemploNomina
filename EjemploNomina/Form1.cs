using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.Office.Interop.Excel;
using static System.Net.WebRequestMethods;

using objExcel = Microsoft.Office.Interop.Excel;


namespace EjemploNomina
{
    public partial class Form1 : Form
    {


        public void Clean()
        {


            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
            textBox6.Text = string.Empty;   
            comboBox1.Text = string.Empty;
            comboBox2.Text = string.Empty;
            comboBox3.Text = string.Empty;
            

            textBox3.Focus();

        }
        public Form1()
        {
            InitializeComponent();
        }
        string ruta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void BtnCalcular_Click(object sender, EventArgs e)
        {

            string nombre = textBox3.Text;
            double salario = double.Parse(textBox1.Text);
            string cedula = textBox4.Text;
            int codigo = int.Parse(textBox5.Text);
            string sexo = comboBox1.Text;
            string CargoDes = textBox6.Text;
            string profesion = comboBox3.Text;
            string estado = comboBox2.Text;
            double horasExtras = double.Parse(textBox2.Text);
            double TotalHorasExtras = ((salario / 240) * 2) * horasExtras;
            double totalDevengado = salario + TotalHorasExtras;
            double deduccionINSS = (totalDevengado * 6.5) / 100;
            double salarioIr = (salario - deduccionINSS) * 12;

            double tasair1;
            double tasaIR = 0;
            double irx12 = 0;
            if (salarioIr == 0 || salarioIr <= 100000)
            {


                tasaIR = 0;
            }
            else if (salarioIr == 100000.01 || salarioIr <= 200000)
            {

                tasair1 = ((salarioIr - 100000) * 15) / 100;
                tasaIR = (tasair1 + 0) / 12;

            }
            else if (salarioIr == 200000.01 || salarioIr <= 350000)
            {
                tasair1 = ((salarioIr - 200000) * 20) / 100;
                tasaIR = (tasair1 + 15000) / 12;
            }
            else if (salarioIr == 350000.01 || salarioIr <= 500000)
            {
                tasair1 = ((salarioIr - 350000) * 25) / 100;
                tasaIR = (tasair1 + 45000) / 12;
            }
            else if (salarioIr == 500000.01 || salarioIr > 500000.01)
            {
                tasair1 = ((salarioIr - 500000) * 30) / 100;
                tasaIR = (tasair1 + 82500) / 12;
            }

            // double INSSPatronal = salarioTotal * 0.1267;
            double salarioNeto = totalDevengado - deduccionINSS - tasaIR;
            dgvNomina.Rows.Add(nombre, cedula, codigo, sexo, CargoDes, profesion, salario, horasExtras, TotalHorasExtras, totalDevengado, deduccionINSS, tasaIR, salarioNeto, estado);
            Clean();





            /*for (int i = 0; i < dgvNomina.Rows.Count; i++)
            {
                for (int j = i+1; j < dgvNomina.Rows.Count; j++)
                {
                    if (dgvNomina.Rows[i].Cells[0].Value.ToString()== dgvNomina.Rows[j].Cells[0].Value.ToString())
                    {
                        MessageBox.Show("El valor "+dgvNomina.Rows[i].Cells[0].Value.ToString()+" ya existe en la fila " +(j+1)+", no se puede repetir.");
                        return;
                    }
                }
            }

            */

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvNomina.AllowUserToAddRows = false;
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            int NumRowseSelect;//Variable conmtadora
            NumRowseSelect = dgvNomina.CurrentRow.Index;
            dgvNomina.Rows.RemoveAt(NumRowseSelect);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "^[0-9]*$"))
            {
                MessageBox.Show("Por favor solo ingrese numeros.");
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
            }

            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(textBox2.Text, "^[0-9]*$"))
            {
                MessageBox.Show("Por favor solo ingrese numeros.");
                textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 1);
            }

            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox3.Text, "[^a-zA-Z]"))
            {
                MessageBox.Show("Por favor solo ingrese letras.");
                textBox3.Text = textBox3.Text.Remove(textBox3.Text.Length - 1);
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow Row in dgvNomina.Rows)
            {
                String strFila = Row.Index.ToString();
                string Valor = Convert.ToString(Row.Cells["Cedula"].Value);

                if (Valor == this.textBox7.Text)
                {
                    dgvNomina.Rows[Convert.ToInt32(strFila)].DefaultCellStyle.BackColor = Color.Green;
                }
            }

            

            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            objExcel.Application objAplicacion = new objExcel.Application();
            Workbook objLibro = objAplicacion.Workbooks.Add(XlSheetType.xlWorksheet);
            Worksheet objHoja = (Worksheet)objAplicacion.ActiveSheet;

            objAplicacion.Visible = false;



            foreach (DataGridViewColumn columna in dgvNomina.Columns)
            {
                objHoja.Cells[1, columna.Index + 1] = columna.HeaderText;
                foreach (DataGridViewRow fila in dgvNomina.Rows)
                {
                    objHoja.Cells[fila.Index + 2, columna.Index + 1] = fila.Cells[columna.Index].Value;
                }
            }

            objLibro.SaveAs(ruta + "\\NominaHospital.xlsx");
            objLibro.Close();

        }

        private void dgvNomina_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void dgvNomina_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
           


        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            dgvNomina.CurrentCell = null;   
            if (textBox7.Text!= "")
            {

                foreach (DataGridViewRow r in dgvNomina.Rows)
                {
                    r.Visible = false;
                }
                foreach (DataGridViewRow r in dgvNomina.Rows)
                {
                    foreach (DataGridViewCell c in r.Cells) {


                        if ((c.Value.ToString().ToUpper()).IndexOf(textBox7.Text.ToUpper())==0)
                        {
                              r.Visible=true;
                            break;
                        }
                    
                    
                    }
                }




            }
            else { 
            
            
            
            }





        }
    }
}
