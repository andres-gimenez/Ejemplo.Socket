using Calculator.Comun;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Calculator.Servidor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];

            //IPAddress ipAddress = IPAddress.Parse("ip escucha");

            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 2800);

            try
            {
                // Create a Socket that will use Tcp protocol
                using Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // A Socket must be associated with an endpoint using the Bind method
                listener.Bind(localEndPoint);
                // Specify how many requests a Socket can listen before it gives Server busy response.
                // We will listen 10 requests at a time
                listener.Listen(10);

                Console.WriteLine("Waiting for a connection ..." + listener.LocalEndPoint.ToString());

                while (true)
                {
                    Socket handler = listener.Accept();
 

                    Console.WriteLine("Socket connected to {0}",
                    handler.RemoteEndPoint.ToString()+ " \n");

                    var cacheMenaje = new byte[4096];
                    int bytesMenaje = handler.Receive(cacheMenaje);



                    if (bytesMenaje > 0)
                    {
                        //deserializamos el mensaje que envia el cliente y lo transformamos al objeto original para poder operar
                        var objDes = Serializacion.Deserializar<DatosOperacion>(cacheMenaje, 0, bytesMenaje);
                        //Encoding.UTF8.GetString(cacheMenaje, 0, bytesMenaje);

                        var respuesta = "Ok ";

                        Console.WriteLine("{0} -> {1}", objDes, respuesta);

                        //calculamos la operacion que nos ha enviado el cliente
                        var resultado = realizarOperacon(objDes.operando1, objDes.operando2, objDes.Operacion);


                        //montamos un nuevo objeto con el resultado de la operacion
                        DatosOpServer objFinal = new DatosOpServer
                        {
                            operando1 = objDes.operando1,
                            operando2 = objDes.operando2,
                            Operacion = objDes.Operacion,
                            resultado = resultado
                        };

                        Console.WriteLine("\n Operacion realizada con exito");

                        //serializamos el objeto con la operacion hecha
                        var Opfinal = Serializacion.Serializar(objFinal);

                        //enviamos el objeto serializado al cliente
                        handler.Send(Opfinal);
                        Console.WriteLine("Operacion enviada al cliente con exito.");
                        Thread.Sleep(0);
                        

                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\n Presiona cualquier tecla para continuar...");
            Console.ReadKey();
        }

        public static double realizarOperacon(double operando1, double operando2, TipoOperacion operacion)
        {
            if (operacion == TipoOperacion.suma)
            {
                return operando1 + operando2;
            }
            else if (operacion == TipoOperacion.resta)
            {
                return operando1 - operando2;
            }
            else if (operacion == TipoOperacion.multiplicacion)
            {
                return operando1 * operando2;
            }
            else if (operacion == TipoOperacion.division)
            {
                if (operando2 == 0)
                {
                    throw new Exception("No es posible dividir entre " + operando2+ "\n");
                }
                else
                {
                    return operando1 / operando2;
                }
            }
            else
            {
                throw new Exception("Operacion introducida no válida \n");
            }
        }
    }
}
