using System;
using System.Net;
using System.Linq;

namespace Correo_Argentino_Tracking
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.Write('\n');
            Console.WriteLine("╔═════════════════════════════════════════════════════════════════╗");
            Console.Write("║            ");
            WriteColor(ConsoleColor.Yellow, "Correo Argentino Tracking Internacional");
            Console.WriteLine("              ║");
            Console.WriteLine("╚═════════════════════════════════════════════════════════════════╝");
            Console.Write('\n');

            string tracking = ReadTracking();
            RetrieveTracking(tracking);

            Console.WriteLine("Presiona ENTER para buscar otro tracking, en caso contrario, cualquier otra tecla...");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
                Main(args);
        }

        static void RetrieveTracking(string tracking)
        {
            using (WebClient wc = new WebClient())
            {
                string url = "https://api.correoargentino.com.ar/backendappcorreo/api/api/shipping-tracking-int-nac?id_shipping=" + tracking;
                Console.WriteLine("Obteniendo informacion del tracking...");
                string json = wc.DownloadString(url);

                TrackingIntNac intNac = TrackingIntNac.FromJson(json);
                if (intNac.Rta == "OK")
                {
                    PrintTracking(intNac);
                }
                else
                {
                    WriteLineColor(ConsoleColor.Red, "Error: " + json);
                }
            }
        }
        static void PrintTracking(TrackingIntNac tracking)
        {
            var pais = tracking.Data.ShippingNac.Eventos.Where(x => !string.IsNullOrEmpty(x.NombrePais)).FirstOrDefault();
            WriteLineColor(ConsoleColor.White, $"\nOrigen: {pais.NombrePais}");
            WriteLineColor(ConsoleColor.White, $"Tracking inter: {tracking.Data.Id}");
            WriteLineColor(ConsoleColor.White, $"Tracking local: {tracking.Data.ItemIdLocal}");
            WriteLineColor(ConsoleColor.White, $"\nGestion Correo Argentino: {tracking.Data.GestionCorreo}");
            WriteLineColor(ConsoleColor.White, $"Gestion AFIP: {tracking.Data.ProcesoAfip}");

            var entrega = tracking.Data.ShippingNac.Eventos.Where(x => x.EstadoEntrega == "ENTREGADO").FirstOrDefault();
            if (entrega != null)
            {
                string entregaStr = entrega.EstadoEntrega + " - " + entrega.MotivoNoEntrega;
                WriteLineColor(ConsoleColor.Green, $"Estado: {entregaStr}");
            }

            WriteLineColor(ConsoleColor.Gray, "\nEstado internacional:");
            foreach (var evento in tracking.Data.Eventos)
            {
                WriteColor(ConsoleColor.DarkGray, $"{evento.HoraEventoAr,-16}\t");
                WriteColor(ConsoleColor.White, $"{evento.EventoAr,-40}\t");
                WriteColor(ConsoleColor.Blue, $"{evento.OficinaAr,-40}\n");
            }

            if (tracking.Data.ShippingNac?.Cantidad > 0)
            {
                WriteLineColor(ConsoleColor.Gray, "\nEstado nacional:");
                foreach (var evento in tracking.Data.ShippingNac.Eventos)
                {
                    WriteColor(ConsoleColor.DarkGray, $"{evento.FechaEvento,-16}\t");
                    WriteColor(ConsoleColor.White, $"{evento.CodigoEvento,-40}\t");
                    WriteColor(ConsoleColor.Blue, $"{evento.Planta,-40}\n");
                }
            }
        }

        static string ReadTracking()
        {
            WriteColor(ConsoleColor.Cyan, "Ingrese tracking: ");

            string tracking = Console.ReadLine().Trim().ToUpperInvariant();

            if (string.IsNullOrEmpty(tracking))
            {
                WriteLineColor(ConsoleColor.Red, "Debe ingresar algun tracking!");
                ReadTracking();
            }

            if (tracking.Length != 13)
            {
                WriteLineColor(ConsoleColor.Red, "El tracking ingresado es incorrecto!");
                ReadTracking();
            }

            if (tracking.Length == 13 && CheckTrackingValid(tracking))
            {
                return tracking;
            }
            return string.Empty;
        }

        static bool CheckTrackingValid(string tracking)
        {
            char first = tracking[0];
            char second = tracking[1];
            char third = tracking[2];

            if ((first > 'A' && first < 'Z') && (second > 'A' && second < 'Z') && (third > '0' && third < '9'))
            {
                int last = tracking[11];
                int secondLast = tracking[12];
                if ((last > 'A' && last < 'Z') && (secondLast > 'A' && secondLast < 'Z'))
                {
                    return true;
                }
            }
            return false;
        }

        static void WriteLineColor(ConsoleColor color, string value)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void WriteColor(ConsoleColor color, string value)
        {
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
