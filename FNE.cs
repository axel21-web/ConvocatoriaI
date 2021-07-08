using ConvocatoriaI.Poco;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvocatoriaI
{
    
    public partial class FNE : Form
    {
        public List<Servicio> Servicios { get; set; }

        String[] rows = { "Inversión", "Ingreso", "Egreso", "Depreciación"
                   , "UAI", "IR", "UDI", "Depreciación", "V. S.", "FNE", };

       
       
       
        public FNE()
        {
            InitializeComponent();
            dgvDatos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDatos.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
         
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                double IR = 0.3;
                decimal VPN = 0;
                double Interes = 0;

                Validate(out decimal Ingresos, out decimal Inversion, out int Plazo, out double Taza, out double Inflacion, out decimal vs, out decimal Egresos);

                //Columnas
                dgvDatos.Columns.Add("", "Años");
                for (int i = 0; i <= Plazo; i++)
                {
                    dgvDatos.Columns.Add("", i.ToString());
                }

                //Filas
                for (int i = 0; i < rows.Length; i++)
                {
                    dgvDatos.Rows.Add(rows[i]);
                }

                dgvDatos.Rows[0].Cells[1].Value = Inversion;
                //Ingresos
                for (int i = 2; i < dgvDatos.ColumnCount; i++)
                {
                    if (i == 2)
                    {
                        dgvDatos.Rows[1].Cells[i].Value = Ingresos;
                    }
                    else
                    {
                        Ingresos += (Ingresos * (decimal)Taza);
                        dgvDatos.Rows[1].Cells[i].Value = Ingresos;
                    }
                }

                //Egresos
                for (int i = 2; i < dgvDatos.ColumnCount; i++)
                {
                    if (i == 2)
                    {
                        dgvDatos.Rows[2].Cells[i].Value = Egresos;
                    }
                    else
                    {
                        Egresos += (Egresos * (decimal)Inflacion);
                        dgvDatos.Rows[2].Cells[i].Value = Egresos;
                    }
                }

                dgvDatos.Rows[8].Cells[Plazo + 1].Value = vs;

                calculoDepreciacion(Inversion, Plazo, vs);
                calculoUAI(dgvDatos);
                CalcularIR(IR, dgvDatos);
                CalcularUDI(dgvDatos);
                Depreciacion(dgvDatos);
                calculoFNE(dgvDatos, Inversion, Plazo);
                CalcularVPN(dgvDatos, VPN, Interes);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Se produjo un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }
        private void CalcularVPN(DataGridView dgv, decimal VPN, double Interes)
        {
            Interes = 1 + double.Parse(txtTasa.Text);
            for (int i = 2; i < dgvDatos.ColumnCount; i++)
            {
                for (int j = 1; j < (Int32.Parse(txtPlazo.Text) + 1); j++)
                {
                    VPN += decimal.Parse(dgv.Rows[9].Cells[i].Value.ToString()) * (decimal)Interes;
                }
            }

            VPN += decimal.Parse(dgv.Rows[9].Cells[1].Value.ToString());
            txtVPN.Text = VPN.ToString();
        }

        private void calculoDepreciacion(decimal Inversion, int Plazo, decimal vs)
        {
            for (int i = 2; i < dgvDatos.ColumnCount; i++)
            {
                dgvDatos.Rows[3].Cells[i].Value = (Inversion - vs) / Plazo;
            }
        }

        private void calculoUAI(DataGridView dgv)
        {
            for (int i = 2; i < dgvDatos.ColumnCount; i++)
            {
                float ingresos = float.Parse(dgvDatos.Rows[1].Cells[i].Value.ToString());
                float depreciacion = float.Parse(dgvDatos.Rows[3].Cells[i].Value.ToString());
                float egreso = float.Parse(dgvDatos.Rows[2].Cells[i].Value.ToString());
                dgv.Rows[4].Cells[i].Value = ingresos - egreso - depreciacion;
            }
        }

        private void CalcularIR(double IR, DataGridView dgv)
        {
            for (int i = 2; i < dgvDatos.ColumnCount; i++)
            {
                decimal n = decimal.Parse(dgv.Rows[4].Cells[i].Value.ToString());
                dgv.Rows[5].Cells[i].Value = n * (decimal)IR;
            }
        }

        private void CalcularUDI(DataGridView dgv)
        {
            for (int i = 2; i < dgvDatos.ColumnCount; i++)
            {
                decimal n = decimal.Parse(dgv.Rows[4].Cells[i].Value.ToString());
                decimal m = decimal.Parse(dgv.Rows[5].Cells[i].Value.ToString());

                dgv.Rows[6].Cells[i].Value = n - m;
            }
        }

        private void calculoFNE(DataGridView dgv, decimal Inversion, int Plazo)
        {
            dgv.Rows[9].Cells[1].Value = -Inversion;
            for (int i = 2; i < dgv.ColumnCount - 1; i++)
            {
                decimal UDI = decimal.Parse(dgv.Rows[6].Cells[i].Value.ToString());
                decimal Dep = decimal.Parse(dgv.Rows[7].Cells[i].Value.ToString());
                dgv.Rows[9].Cells[i].Value = UDI + Dep;
            }

            decimal UDI2 = decimal.Parse(dgv.Rows[6].Cells[Plazo + 1].Value.ToString());
            decimal Dep2 = decimal.Parse(dgv.Rows[7].Cells[Plazo + 1].Value.ToString());
            decimal VS = decimal.Parse(dgv.Rows[8].Cells[Plazo + 1].Value.ToString());

            dgv.Rows[9].Cells[Plazo + 1].Value = UDI2 + Dep2 + VS;
        }


        private void Depreciacion(DataGridView dgv)
        {
            for (int i = 2; i < dgvDatos.ColumnCount; i++)
            {
                dgv.Rows[7].Cells[i].Value = dgv.Rows[3].Cells[i].Value;
            }
        }

        private void Validate(out decimal Ingresos, out decimal Inversion, out int Plazo, out double Tasa, out double Inflacion, out decimal VS, out decimal Egresos)
        {
            if (!decimal.TryParse(txtIngresos.Text, out decimal I))
            {
                throw new ArgumentException($"El No. {txtIngresos.Text} es inválido");
            }
            Ingresos = I;
            if (!decimal.TryParse(txtInversion.Text, out decimal In))
            {
                throw new ArgumentException($"El No. {txtInversion.Text} es inválido");
            }
            Inversion = In;
            if (!Int32.TryParse(txtPlazo.Text, out int P))
            {
                throw new ArgumentException($"El No. {txtPlazo.Text} es inválido");
            }
            Plazo = P;
            if (!double.TryParse(txtTasa.Text, out double T))
            {
                throw new ArgumentException($"El No. {txtTasa.Text} es inválido");
            }
            Tasa = T;
            if (!double.TryParse(txtInflacion.Text, out double Inf))
            {
                throw new ArgumentException($"El No. {txtInflacion.Text} es inválido");
            }
            Inflacion = Inf;
            if (!decimal.TryParse(txtVS.Text, out decimal vs))
            {
                throw new ArgumentException($"El No. {txtInflacion.Text} es inválido");
            }
            VS = vs;
            if (!decimal.TryParse(txtEgresos.Text, out decimal E))
            {
                throw new ArgumentException($"El No. {txtEgresos.Text} es inválido");
            }
            Egresos = E;
            if (string.IsNullOrWhiteSpace(txtIngresos.Text))
            {
                throw new ArgumentException("Los ingresos son requeridos");
            }
            if (string.IsNullOrWhiteSpace(txtInversion.Text))
            {
                throw new ArgumentException("La inversión es requerido");
            }
            if (string.IsNullOrWhiteSpace(txtPlazo.Text))
            {
                throw new ArgumentException("Los plazos de la inversión son requeridos");
            }
            if (string.IsNullOrWhiteSpace(txtTasa.Text))
            {
                throw new ArgumentException("La tasa de interés es requerida");
            }
            if (string.IsNullOrWhiteSpace(txtInflacion.Text))
            {
                throw new ArgumentException("La tasa de inflación es requerida");
            }
            if (string.IsNullOrWhiteSpace(txtVS.Text))
            {
                throw new ArgumentException("El valor de salvamento es requerido");
            }
            if (string.IsNullOrWhiteSpace(txtEgresos.Text))
            {
                throw new ArgumentException("Los egresos son requeridos");
            }
            if (Inflacion >= Tasa)
            {
                throw new ArgumentException("La inflación no puede ser mayor o igual que la Tasa de interés");
            }
        }
    }
}
