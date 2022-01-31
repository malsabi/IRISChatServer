using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using IRISChatServer.DataAccessLayer.Messages;

namespace IRISChatServer.DataAccessLayer
{
    public interface IServerBase : IDisposable
    {
        /// <summary>
        /// Expose an event when the server <see cref="IsInitialized"/> property is set to true.
        /// </summary>
        event Action OnServerInitialized;

        /// <summary>
        /// Exposes an event when the server <see cref="IsListening"/> property is changed.
        /// </summary>
        event Action<bool> OnServerStateChanged;

        /// <summary>
        /// Exposes an event when the server listener is started and sets the <see cref="IsListening"/> to true.
        /// </summary>
        event Action OnServerStartedListening;

        /// <summary>
        /// Exposes an event when the server listener is stopped and sets the <see cref="IsListening"/> to false.
        /// </summary>
        event Action OnServerStoppedListening;

        /// <summary>
        /// Exposes an event when there is an exception occured in the server.
        /// </summary>
        event Action<Exception> OnServerException;

        /// <summary>
        /// Expose an event when the server <see cref="IClientBase.IsInitialized"/> property is set to true.
        /// </summary>
        event Action<IClientBase> OnClientInitialized;

        /// <summary>
        /// Exposes an event when the client state such as IsConnected, IsDisconnected, AttemptToReconnect values
        /// are changed.
        /// </summary>
        event Action<IClientBase, bool> OnClientStateChanged;

        /// <summary>
        /// Exposes an event when a client is connected.
        /// </summary>
        event Action<IClientBase> OnClientConnected;

        /// <summary>
        /// Exposes an event when a client is disconnected.
        /// </summary>
        event Action<IClientBase> OnClientDisconnected;

        /// <summary>
        /// Exposes an event when a client received a response message from the server.
        /// </summary>
        event Action<IClientBase, ResponseMessage> OnClientResponseReceived;

        /// <summary>
        /// Exposes an event when a client send a request message to the server.
        /// </summary>
        event Action<IClientBase, RequestMessage> OnClientRequestSent;

        /// <summary>
        /// Exposes an event when an exception occurs in the client.
        /// </summary>
        event Action<IClientBase, Exception> OnClientException;

        /// <summary>
        /// Returns true if the server <see cref="Initialize"/> method is called otherwise false.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Returns true if the server is on the listening state otherwise false.
        /// </summary>
        bool IsListening { get; }

        /// <summary>
        /// Returns the connection end point that is combined by the ip address / host and port number.
        /// </summary>
        IPEndPoint IPEndPoint { get; }

        /// <summary>
        /// Returns a <see cref="ConcurrentDictionary{TKey, TValue}"/> that stores all of the clients.
        /// The <see cref="TKey"/> represents the <see cref="System.Net.IPEndPoint"/>.
        /// The <see cref="TValue"/> represents the <see cref="IClientBase"/>.
        /// </summary>
        ConcurrentDictionary<IPEndPoint, IClientBase> Clients { get; }

        /// <summary>
        /// Returns the Socket.IO that is responsible for listening to new connections and accepting them.
        /// </summary>
        Socket ServerSocket { get; }

        /// <summary>
        /// Initialize the properties and all other instances.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Starts listening on a specific <see cref="IPEndPoint"/>.
        /// </summary>
        /// <returns>Returns a task indicating if the task succeded otherwise failed.</returns>
        Task StartListener();

        /// <summary>
        /// Stops listening and disconnects all connected clients.
        /// </summary>
        /// <returns>Returns a task indicating if the task succeded otherwise failed.</returns>
        Task StopListener();
    }
}