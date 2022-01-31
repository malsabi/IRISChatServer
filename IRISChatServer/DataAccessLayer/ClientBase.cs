using System;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using IRISChatServer.Enums;
using IRISChatServer.Configs;
using IRISChatServer.Helpers;
using IRISChatServer.DataAccessLayer.Messages;
using IRISChatServer.DataAccessLayer.Encryption;

namespace IRISChatServer.DataAccessLayer
{
    public class ClientBase : IClientBase
    {
        #region "Fields"
        private BufferBaseManager BufferBase;
        #endregion

        #region "Properties"
        public bool IsInitialized { get; private set; }

        public bool IsConnected { get; private set; }

        public Socket ClientSocket { get; private set; }

        public IPEndPoint IPEndPoint { get; private set; }
        #endregion

        #region "Events"
        public event Action<object> OnInitialized;

        public event Action<object, bool> OnStateChanged;

        public event Action<object> OnDisconnected;

        public event Action<object, ResponseMessage> OnResponseReceived;

        public event Action<object, RequestMessage> OnRequestSent;

        public event Action<object, Exception> OnException;
        #endregion

        #region "Constructors / Destructors"
        public ClientBase(Socket ClientSocket)
        {
            this.ClientSocket = ClientSocket;
        }

        ~ClientBase()
        {
            Dispose();
        }
        #endregion

        #region "Receiver"
        private void StartReceivingMessages()
        {
            try
            {
                switch (BufferBase.State)
                {
                    case BufferState.Header:
                        {
                            ClientSocket.BeginReceive(BufferBase.HeaderBuffer, 0, BufferBase.HeaderBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessageCallback), null);
                        }
                        break;
                    case BufferState.Message:
                        {
                            ClientSocket.BeginReceive(BufferBase.MessageBuffer, 0, BufferBase.MessageBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessageCallback), null);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, ex);
                Disconnect();
            }
        }

        private void ReceiveMessageCallback(IAsyncResult AsyncResult)
        {
            try
            {
                int BytesReceived = ClientSocket.EndReceive(AsyncResult);
                switch (BufferBase.State)
                {
                    case BufferState.Header:
                        {
                            if (BytesReceived == DataAccessLayerConfig.HEADER_SIZE)
                            {
                                BufferBase.MessageSize = BitConverter.ToInt32(BufferBase.HeaderBuffer, 0);

                                if (BufferBase.MessageSize <= 0 || BufferBase.MessageSize >= DataAccessLayerConfig.MAXIMUM_MESSAGE_SIZE)
                                {
                                    throw new Exception(DataAccessLayerConfig.MESSAGE_SIZE_EXCEPTION);
                                }
                                else
                                {
                                    BufferBase.State = BufferState.Message;
                                    BufferBase.ReAllocate();
                                }
                            }
                            else
                            {
                                throw new Exception(DataAccessLayerConfig.INVALID_BYTES_EXCEPTION);
                            }
                        }
                        break;
                    case BufferState.Message:
                        {
                            ResponseMessage Response = ProcessMessage.Process(BufferBase.MessageBuffer);
                            if (Response == null)
                            {
                                throw new Exception(DataAccessLayerConfig.INVALID_MESSAGE_EXCEPTION);
                            }
                            else
                            {
                                BufferBase.DeAllocate();
                                BufferBase.MessageSize = -1;
                                BufferBase.State = BufferState.Header;
                                if (BufferBase.Responses.Count > 0)
                                {
                                    foreach (ResponseMessage PendingRespone in BufferBase.Responses.ToArray())
                                    {
                                        if (PendingRespone.Id.SequenceEqual(Response.Id) && PendingRespone.ExpectedMessageType.Equals(Response.MessageType))
                                        {
                                            PendingRespone.Message = Response.Message;
                                            PendingRespone.MessageType = Response.MessageType;
                                            PendingRespone.Handler.Set();
                                        }
                                    }
                                }
                                OnResponseReceived(this, Response);
                            }
                        }
                        break;
                }
                StartReceivingMessages();
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, ex);
                Disconnect();
            }
        }
        #endregion

        #region "Client"
        public void Initialize()
        {
            if (IsInitialized == false)
            {
                IPEndPoint = (IPEndPoint)ClientSocket.RemoteEndPoint;
                BufferBase = new BufferBaseManager();
                IsInitialized = true;
                OnInitialized?.Invoke(this);
                IsConnected = true;
                OnStateChanged?.Invoke(this, true);
                StartReceivingMessages();
            }
            else
            {
                OnException?.Invoke(this, new Exception(DataAccessLayerConfig.INITIALIZE_EXCEPTION_MESSAGE));
            }
        }

        public Task Disconnect()
        {
            return Task.Run(() => 
            {
                IsConnected = false;
                OnStateChanged?.Invoke(this, false);
                Dispose();
                OnDisconnected?.Invoke(this);
            });
        }

        public Task SendMessage(IMessage Message)
        {
            return Task.Run(() => 
            {
                if (IsConnected)
                {
                    RequestMessage Request = new RequestMessage()
                    {
                        Message = Message,
                        MessageType = Message.GetType().Name,
                    };
                    string SerializedRequest = Serialization.Serialize(Request);
                    if (string.IsNullOrEmpty(SerializedRequest))
                    {
                        OnException?.Invoke(this, new Exception(DataAccessLayerConfig.SERIALIZATION_ERROR_EXCEPTION));
                    }
                    else
                    {
                        byte[] Packet = SocketHelper.AppendHeader(AES256.Encrypt(SerializedRequest));
                        try
                        {
                            int BytesSent = ClientSocket.Send(Packet);
                            if (BytesSent == Packet.Length)
                            {
                                OnRequestSent(this, Request);
                            }
                            else
                            {
                                OnException?.Invoke(this, new Exception(DataAccessLayerConfig.PACKET_NOT_COMPLETE_EXCEPTION));
                            }
                        }
                        catch (Exception ex)
                        {
                            OnException?.Invoke(this, ex);
                            Disconnect();
                        }
                    }
                }
            });
        }

        public Task<ResponseMessage> PostMessage(IMessage Message, string ExpectedMessageType)
        {
            return Task.Run(async () =>
            {
                ResponseMessage ExpectedResponse = new ResponseMessage(DateTime.Now, ExpectedMessageType);
                if (IsConnected)
                {
                    RequestMessage Request = new RequestMessage(DateTime.Now, ExpectedMessageType)
                    {
                        Message = Message,
                        MessageType = Message.GetType().Name,
                        ExpectedMessageType = ExpectedMessageType
                    };
                    ExpectedResponse.Id = Request.Id;
                    BufferBase.Responses.Add(ExpectedResponse);

                    string SerializedRequest = Serialization.Serialize(Request);
                    if (string.IsNullOrEmpty(SerializedRequest))
                    {
                        OnException?.Invoke(this, new Exception(DataAccessLayerConfig.SERIALIZATION_ERROR_EXCEPTION));
                    }
                    else
                    {
                        byte[] Packet = SocketHelper.AppendHeader(AES256.Encrypt(SerializedRequest));
                        try
                        {
                            int BytesSent = ClientSocket.Send(Packet);
                            if (BytesSent == Packet.Length)
                            {
                                OnRequestSent(this, Request);
                                ExpectedResponse.Handler.WaitOne(DataAccessLayerConfig.RESPONSE_TIME_OUT);
                                if (ExpectedResponse.Message == null)
                                {
                                    ExpectedResponse.IsTimeout = true;
                                }
                                BufferBase.Responses.Remove(ExpectedResponse);
                            }
                            else
                            {
                                OnException?.Invoke(this, new Exception(DataAccessLayerConfig.PACKET_NOT_COMPLETE_EXCEPTION));
                            }
                        }
                        catch (Exception ex)
                        {
                            OnException?.Invoke(this, ex);
                            await Disconnect();
                        }
                    }
                }
                return ExpectedResponse;
            });
        }

        public void Dispose()
        {
            if (BufferBase != null)
            {
                BufferBase.Dispose();
            }
            if (ClientSocket != null)
            {
                try
                {
                    ClientSocket.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                }
                ClientSocket.Close();
                ClientSocket.Dispose();
                ClientSocket = null;
            }
        }
        #endregion
    }
}