using System;
using System.Text;
using System.Text.Json;

namespace Calculator.Comun
{
    public class DatosOperacion
    {
        public double operando1 { get; set;}
        public double operando2 { get; set; }
        public TipoOperacion Operacion { get; set; }

        public override string ToString()
        {
            return "primer operando: "+operando1+", segundo operando: "+operando2+", tipo operacion: "+Operacion;
        }

        static void Main(string[] args)
        {}

        }

}
