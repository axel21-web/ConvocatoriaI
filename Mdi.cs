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
    public partial class Mdi : Form
    {
        List<Servicio> servicios;
        public Mdi()
        {
            InitializeComponent();
            servicios = new List<Servicio>();
        }

        private void SalirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FNE frmfne = new FNE();
            frmfne.Servicios = servicios;
            frmfne.MdiParent = this;
            frmfne.Show();
        }

        private void GuardarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void EliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvDatos.Rows.Count == 0)
            {
                return;
            }
            int index = dgvDatos.CurrentCell.RowIndex;
            Productos.RemoveAt(index);

            dgvCatalogo.DataSource = null;
            dgvCatalogo.DataSource = Productos;
        }
    }
}
