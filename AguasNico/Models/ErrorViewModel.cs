namespace AguasNico.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; } = "Error desconocido";

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public int? ErrorCode { get; set; }
        public string Message { get; set; } = "Error desconocido";
    }
}
