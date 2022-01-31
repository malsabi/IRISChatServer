using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using IRISChatServer.DataAccessLayer.Messages;

namespace IRISChatServer.DataAccessLayer
{
    public interface IClientBase : IDisposable
    {
        /// <summary>
        /// Expose an event when the server <see cref="IsInitialized"/> property is set to true.
        /// </summary>
        event Action<object> OnInitialized;

        /// <summary>
        /// Exposes an event when the client state such as IsConnected, IsDisconnected, AttemptToReconnect values
        /// are changed.
        /// </summary>
        event Action<object, bool> OnStateChanged;

        /// <summary>
        /// Exposes an event when a client is disconnected.
        /// </summary>
        event Action<object> OnDisconnected;

        /// <summary>
        /// Exposes an event when a client received a response message from the server.
        /// </summary>
        event Action<object, ResponseMessage> OnResponseReceived;

        /// <summary>
        /// Exposes an event when a client send a request message to the server.
        /// </summary>
        event Action<object, RequestMessage> OnRequestSent;

        /// <summary>
        /// Exposes an event when an exception occurs in the client.
        /// </summary>
        event Action<object, Exception> OnException;

        /// <summary>
        /// Returns true if the <see cref="Initialize"/> method was called otherwise false.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Returns true if the client is connected to the server.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Returns the Socket.IO that is responsible for connecting and communication to/with the server.
        /// </summary>
        Socket ClientSocket { get; }

        /// <summary>
        /// Returns the connection end point that is combined by the ip address / host and port number.
        /// </summary>
        IPEndPoint IPEndPoint { get; }

        /// <summary>
        /// Initialize the properties and all other instances.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Disconnects from the server if the client is connected to the server.
        /// </summary>
        Task Disconnect();

        /// <summary>
        /// Sends a message to the server.
        /// </summary>
        /// <param name="Message">Represents the message to be sent.</param>
        Task SendMessage(IMessage Message);

        /// <summary>
        /// Sends a message to the server and returns a response from the server.
        /// </summary>
        /// <param name="Message">Represents the meessage to be sent.</param>
        /// <param name="ExpectedMessageType">Represents the expected message response from the server.</param>
        /// <returns>Returns a task of <see cref="ResponseMessage"/> that contains the response from the server.</returns>
        Task<ResponseMessage> PostMessage(IMessage Message, string ExpectedMessageType);
    }
}