namespace Trabajo_Final.DTO.Response.Server_HTTP_Response
{
    public class ResponseBodyDTO
    {
        public string message { get; set; }
        public string type { get; set; }
        public string path { get; set; }

        public ResponseBodyDTO() { }

        public ResponseBodyDTO(string message, string type, string path)
        {
            this.message = message;
            this.type = type;
            this.path = path;
        }
    }
}
