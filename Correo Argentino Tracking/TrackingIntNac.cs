using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Correo_Argentino_Tracking
{
    public partial class TrackingIntNac
    {
        [JsonPropertyName("rta")]
        public string Rta { get; set; }

        [JsonPropertyName("code")]
        public long Code { get; set; }

        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonPropertyName("eventos")]
        public List<DataEvento> Eventos { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("itemIdLocal")]
        public string ItemIdLocal { get; set; }

        [JsonPropertyName("cantidad")]
        public long Cantidad { get; set; }

        [JsonPropertyName("ProcesoAfip")]
        public string ProcesoAfip { get; set; }

        [JsonPropertyName("GestionCorreo")]
        public string GestionCorreo { get; set; }

        [JsonPropertyName("shippingNac")]
        public ShippingNac ShippingNac { get; set; }
    }

    public partial class DataEvento
    {
        [JsonPropertyName("oficinaAR")]
        public string OficinaAr { get; set; }

        [JsonPropertyName("destino")]
        public string Destino { get; set; }

        [JsonPropertyName("eventoAR")]
        public string EventoAr { get; set; }

        [JsonPropertyName("recibio")]
        public string Recibio { get; set; }

        [JsonPropertyName("horaEventoAR")]
        public string HoraEventoAr { get; set; }

        [JsonPropertyName("salidaDespacho")]
        public string SalidaDespacho { get; set; }
    }

    public partial class ShippingNac
    {
        [JsonPropertyName("eventos")]
        public List<ShippingNacEvento> Eventos { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("codigoProducto")]
        public string CodigoProducto { get; set; }

        [JsonPropertyName("codigoPais")]
        public string CodigoPais { get; set; }

        [JsonPropertyName("cantidad")]
        public long Cantidad { get; set; }

        [JsonPropertyName("leyenda")]
        public string Leyenda { get; set; }
    }

    public partial class ShippingNacEvento
    {
        [JsonPropertyName("codigoEvento")]
        public string CodigoEvento { get; set; }

        [JsonPropertyName("fechaEvento")]
        public string FechaEvento { get; set; }

        [JsonPropertyName("planta")]
        public string Planta { get; set; }

        [JsonPropertyName("estadoEntrega")]
        public string EstadoEntrega { get; set; }

        [JsonPropertyName("motivoNoEntrega")]
        public string MotivoNoEntrega { get; set; }

        [JsonPropertyName("nombrePais")]
        public string NombrePais { get; set; }
    }

    public partial class TrackingIntNac
    {
        public static TrackingIntNac FromJson(string json) => JsonSerializer.Deserialize<TrackingIntNac>(json);
    }
}
