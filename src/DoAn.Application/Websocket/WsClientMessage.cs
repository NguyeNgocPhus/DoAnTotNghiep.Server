namespace DoAn.Application.Websocket
{
    public class WsClientMessage
    {
        public string command { get; set; }
        public string identifier { get; set; }
        public object data { get; set; }

    }
}
