using Calculator.Comun;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Calculator.Cliente
{
    internal class Program
    {


        static void Main(string[] args)
        {
            Console.WriteLine("Consola C#\r");
            Console.WriteLine("------------------------\n");

            Console.WriteLine("Introduce primer operando: ");
            double operando = Double.Parse(Console.ReadLine());

            Console.WriteLine("Introduce segundo operando: ");
            double operador = Double.Parse(Console.ReadLine());

            Console.WriteLine("Introduce operacion a realizar Suma[65] Resta[76] Multiplicacion[87] Division[98]: ");
            String operacion = Console.ReadLine();

            //creamos el objeto DatosOpeacion para guardar la operacion
            DatosOperacion resultOperacion = new DatosOperacion
            {
                operando1 = operando,
                operando2 = operador,
                Operacion = (TipoOperacion)int.Parse(operacion)
            };

            //serializo el objeto, lo envio al servidor y retorno la respuesta de este y lo visualizo
            Console.WriteLine(EnviaMenaje(resultOperacion).ToString());

            Console.Write("Pulsa cualquier tecla para cerrar la calculadora app...");
            Console.ReadKey();
        }

        static DatosOpServer EnviaMenaje<T>( T objeto)
        {
            try
            {
                // Connect to a Remote server
                // Get Host IP Address that is used to establish a connection
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1
                // If a host has multiple addresses, you will get a list of addresses
                
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];


                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 2800);

                // Create a TCP/IP  socket.
                using Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    // Connect to Remote EndPoint
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    Console.WriteLine("Socket redad for {0}",
                        sender.LocalEndPoint.ToString());


                    var cacheEnvio = Serializacion.Serializar(objeto);

                    // Send the data through the socket.
                    int bytesSend = sender.Send(cacheEnvio);

                    // Receive the response from the remote device.
                    byte[] bufferRec = new byte[1024];
                    int bytesRec1 = sender.Receive(bufferRec);

                    //deserializo la operacion final que me ha enviado en servidor
                    var opFinal = Serializacion.Deserializar<DatosOpServer>(bufferRec, 0, bytesRec1);
                    //var resultado = Encoding.UTF8.GetString(bufferRec, 0, bytesRec1);

                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                    return opFinal;

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }
    }
}
