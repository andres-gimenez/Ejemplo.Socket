using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Calculator.Comun
{
    public enum TipoOperacion : int
    {
        [EnumMember(Value = "suma")]
        suma = 65,
        [EnumMember(Value = "resta")]
        resta = 76,
        [EnumMember(Value = "multiplicacion")]
        multiplicacion = 87,
        [EnumMember(Value = "division")]
        division = 98
    }

}
