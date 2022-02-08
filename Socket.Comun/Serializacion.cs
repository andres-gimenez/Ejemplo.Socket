using Calculator.Comun;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Calculator.Comun
{
    public class Serializacion
    {
        public static T Deserializar<T>(byte[] bytes, int index, int count)
        {
            var json = Encoding.UTF8.GetString(bytes, index, count);
            var objeto = JsonSerializer.Deserialize<T>(json);

            return objeto;
        }

        public static byte[] Serializar<T>(T objeto)
        {
            var json = JsonSerializer.Serialize<T>(objeto);
            var arraydebyte = Encoding.UTF8.GetBytes(json);

            return arraydebyte;
        }
    }
}
