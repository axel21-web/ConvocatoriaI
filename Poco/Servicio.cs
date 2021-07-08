using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvocatoriaI.Poco
{
    public class Servicio
    {
        public decimal Ingresos { get; set; }
        public decimal Inversion { get; set; }
        public decimal Egresos { get; set; }
        public int Plazo { get; set; }
        public double Tasa { get; set; }
        public double Inflacion { get; set; }
        public decimal vs { get; set; }
        public double IR { get; set; }
        public double Interes { get; set; }
    }
}
