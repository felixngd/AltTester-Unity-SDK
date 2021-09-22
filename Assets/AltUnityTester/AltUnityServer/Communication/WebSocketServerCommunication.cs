using System;
using WebSocketSharp.Server;
namespace Assets.AltUnityTester.AltUnityServer.Communication
{

    public class WebSocketServerCommunication : ICommunication
    {
        WebSocketServer wsServer;
        private readonly int port;
        private readonly string host;

        AltServerWebSocketHandler wsHandler = null;

        public WebSocketServerCommunication(ICommandHandler cmdHandler, string host, int port)
        {
            this.port = port;
            this.host = host;
            Uri uri;
            if (!Uri.TryCreate(string.Format("ws://{0}:{1}/", host, port), UriKind.Absolute, out uri))
            {
                throw new Exception(String.Format("Invalid host or port {0}:{1}", host, port));
            }

            wsServer = new WebSocketServer(uri.ToString());

            wsServer.AddWebSocketService<AltServerWebSocketHandler>("/altws", () =>
            {
                this.wsHandler = new AltServerWebSocketHandler(cmdHandler);

                this.wsHandler.OnErrorHandler += (message, exception) =>
                {
                    if (this.OnError != null) this.OnError.Invoke(message, exception);
                };

                this.wsHandler.OnClientConnected += () =>
                {
                    if (this.OnConnect != null) this.OnConnect.Invoke();
                };

                this.wsHandler.OnClientDisconnected += () =>
                {
                    if (this.OnDisconnect != null)
                    {
                        if (wsServer.WebSocketServices["/altws"].Sessions.Count == 0)
                            this.OnDisconnect();
                    }
                };

                return this.wsHandler;
            });
        }
        public bool IsConnected { get { return wsServer.WebSocketServices.SessionCount > 0; } }
        public bool IsListening { get { return wsServer.IsListening; } }

        public CommunicationHandler OnConnect { get; set; }
        public CommunicationHandler OnDisconnect { get; set; }
        public CommunicationErrorHandler OnError { get; set; }

        public void Start()
        {
            try
            {
                if (!wsServer.IsListening)
                    wsServer.Start();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Only one usage of each socket address"))
                {
                    throw new AddressInUseCommError("Cannot start AltUnity Server. Another process is listening on port " + port);
                }

                throw new UnhandledStartCommError("An error occured while starting AltUnity Server", ex);
            }
        }

        public void Stop()
        {
            wsServer.Stop();
        }
    }
}