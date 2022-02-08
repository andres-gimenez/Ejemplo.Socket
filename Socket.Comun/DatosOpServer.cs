using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Comun
{
    public class DatosOpServer
    {
        public double operando1 { get; set; }
        public double operando2 { get; set; }
        public TipoOperacion Operacion { get; set; }
        public double resultado { get; set; }

        public override string ToString()
        {
            return "El resultado de "+ Operacion + " "+ operando1 + " y " + operando2 + " = "+ resultado;
        }
    }
}
