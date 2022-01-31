using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using IRISChatServer.Configs;
using IRISChatServer.DataAccessLayer.Messages;

namespace IRISChatServer.DataAccessLayer
{
    /// <summary>
    /// ADD: StartClientAcceptor Method in IServerBase.
    /// </summary>
    public class ServerBase : IServerBase
    {
        #region "Properties"
        public bool IsInitialized { get; private set; }

        public bool IsListening { get; private set; }

        public IPEndPoint IPEndPoint { get; private set; }

        public ConcurrentDictionary<IPEndPoint, IClientBase> Clients { get; private set; }

        public Socket ServerSocket { get; private set; }
        #endregion

        #region "Events"
        public event Action OnServerInitialized;

        public event Action<bool> OnServerStateChanged;

        public event Action OnServerStartedListening;

        public event Action OnServerStoppedListening;

        public event Action<Exception> OnServerException;

        public event Action<IClientBase> OnClientInitialized;
        public void SetOnClientInitialized(object Sender)
        {
            OnClientInitialized?.Invoke((IClientBase)Sender);
        }

        public event Action<IClientBase, bool> OnClientStateChanged;
        private void SetOnClientStateChanged(object Sender, bool State)
        {
            OnClientStateChanged?.Invoke((IClientBase)Sender, State);
        }

        public event Action<IClientBase> OnClientConnected;
        private void SetOnClientConnected(IClientBase Sender)
        {
            OnClientConnected?.Invoke(Sender);
        }

        public event Action<IClientBase> OnClientDisconnected;
        private void SetOnClientDisconnected(object Sender)
        {
            RemoveClient((IClientBase)Sender);
            OnClientDisconnected?.Invoke((IClientBase)Sender);
        }

        public event Action<IClientBase, ResponseMessage> OnClientResponseReceived;
        private void SetOnClientResponseReceived(object Sender, ResponseMessage Response)
        {
            OnClientResponseReceived?.Invoke((IClientBase)Sender, Response);
        }

        public event Action<IClientBase, RequestMessage> OnClientRequestSent;
        private void SetOnClientRequestSent(object Sender, RequestMessage Request)
        {
            OnClientRequestSent?.Invoke((IClientBase)Sender, Request);
        }

        public event Action<IClientBase, Exception> OnClientException;
        private void SetOnClientException(object Sender, Exception ex)
        {
            OnClientException?.Invoke((IClientBase)Sender, ex);
        }
        #endregion

        #region "Constructors / Destructors"
        public ServerBase(int Port)
        {
            IPEndPoint = new IPEndPoint(IPAddress.Any, Port);
        }

        public ServerBase(string IP, int Port)
        {
            IPEndPoint = new IPEndPoint(IPAddress.Parse(IP), Port);
        }

        public ServerBase(IPAddress IP, int Port)
        {
            IPEndPoint = new IPEndPoint(IP, Port);
        }

        ~ServerBase()
        {
            Dispose();
        }
        #endregion

        #region "Acceptor"
        private void StartAcceptingClients()
        {
            try
            {
                ServerSocket.BeginAccept(new AsyncCallback(AcceptClientCallBack), null);
            }
            catch (Exception ex)
            {
                OnServerException(ex);
            }
        }

        private void AcceptClientCallBack(IAsyncResult AsyncResult)
        {
            try
            {
                Socket ClientSocket = ServerSocket.EndAccept(AsyncResult);
                ClientBase Client = new ClientBase(ClientSocket);
                Client.Initialize();
                AddClient(Client);
                StartAcceptingClients();
            }
            catch (Exception ex)
            {
                OnServerException(ex);
            }
        }

        private void AddClient(IClientBase Client) //improvements.
        {
            SetOnClientConnected(Client);
            Client.OnInitialized += SetOnClientInitialized;
            Client.OnStateChanged += SetOnClientStateChanged;
            Client.OnResponseReceived += SetOnClientResponseReceived;
            Client.OnRequestSent += SetOnClientRequestSent;
            Client.OnDisconnected += SetOnClientDisconnected;
            Client.OnException += SetOnClientException;
            try
            {
                Clients.TryAdd(Client.IPEndPoint, Client);
            }
            catch (Exception ex)
            {
                OnServerException?.Invoke(ex);
            }
        }

        private void RemoveClient(IClientBase Client) //improvements.
        {
            Client.OnInitialized -= SetOnClientInitialized;
            Client.OnStateChanged -= SetOnClientStateChanged;
            Client.OnResponseReceived -= SetOnClientResponseReceived;
            Client.OnRequestSent -= SetOnClientRequestSent;
            Client.OnDisconnected -= SetOnClientDisconnected;
            Client.OnException -= SetOnClientException;
            try
            {
                Clients.TryRemove(IPEndPoint, out IClientBase value);
            }
            catch (Exception ex)
            {
                OnServerException?.Invoke(ex);
            }
        }
        #endregion

        #region "Server"
        public void Initialize()
        {
            if (IsInitialized == false)
            {
                Clients = new ConcurrentDictionary<IPEndPoint, IClientBase>();
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IsInitialized = true;
                OnServerInitialized?.Invoke();
            }
            else
            {
                OnServerException?.Invoke(new Exception(DataAccessLayerConfig.INITIALIZE_EXCEPTION_MESSAGE));
            }
        }
  
        public Task StartListener()
        {
            return Task.Run(() =>
            {
                if (IsInitialized)
                {
                    try
                    {
                        ServerSocket.SetIPProtectionLevel(IPProtectionLevel.Unrestricted);
                        ServerSocket.Bind(IPEndPoint);
                        ServerSocket.Listen(DataAccessLayerConfig.MAXIMUM_PENDING_CONNECTIONS);
                        IsListening = true;
                        OnServerStateChanged?.Invoke(true);
                        OnServerStartedListening?.Invoke();
                        StartAcceptingClients();
                    }
                    catch (Exception ex)
                    {
                        OnServerException?.Invoke(ex);
                    }
                }
                else
                {
                    OnServerException?.Invoke(new Exception(DataAccessLayerConfig.NOT_INITIALIZED_EXCEPTION_MESSAGE));
                }
            });
        }

        public Task StopListener()
        {
            return Task.Run(async () => 
            {
                if (IsListening)
                {
                    IsInitialized = false;
                    IsListening = false;
                    OnServerStateChanged?.Invoke(false);
                    if (Clients.Count > 0)
                    {
                        foreach (KeyValuePair<IPEndPoint, IClientBase> Client in Clients.ToArray())
                        {
                            RemoveClient(Client.Value);
                            await Client.Value.Disconnect();
                        }
                    }
                    Dispose();
                    OnServerStoppedListening?.Invoke();
                }
            });
        }

        public void Dispose()
        {
            if (Clients != null)
            {
                Clients.Clear();
                Clients = null;
            }
            if (ServerSocket != null)
            {
                ServerSocket.Close();
                ServerSocket.Dispose();
                ServerSocket = null;
            }
        }
        #endregion
    }
}