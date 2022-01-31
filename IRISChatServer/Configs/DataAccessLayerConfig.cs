namespace IRISChatServer.Configs
{
    public static class DataAccessLayerConfig
    {
        /// <summary>
        /// Represents the maximum pending connections that are in the queue waiting to be proccessed.
        /// </summary>
        public const int MAXIMUM_PENDING_CONNECTIONS = 100;
        /// <summary>
        /// Represents the message when the initialize method was not called.
        /// </summary>
        public const string NOT_INITIALIZED_EXCEPTION_MESSAGE = "The method 'Initialize' was not called.";
        /// <summary>
        /// Represents the message when the initialize method is called twice.
        /// </summary>
        public const string INITIALIZE_EXCEPTION_MESSAGE = "The method 'Initialize' was already called.";
        /// <summary>
        /// Represents the message when the received message is not valid.
        /// </summary>
        public const string INVALID_MESSAGE_EXCEPTION = "Unknown message type, source can be from attacker or unknown source.";
        /// <summary>
        /// Represents the message when the received bytes is not as expected.
        /// </summary>
        public const string INVALID_BYTES_EXCEPTION = "Invalid bytes received";
        /// <summary>
        /// Represents the message when the recevied message length is less than or equal to zero or exceeds the maximum length.
        /// </summary>
        public const string MESSAGE_SIZE_EXCEPTION = "Message Size cannot be less than zero or exceeds the maximum message size (1MB).";
        /// <summary>
        /// Represents the message when the packet bytes length is not equal to the sent bytes length.
        /// </summary>
        public const string PACKET_NOT_COMPLETE_EXCEPTION = "The number of bytes sent was not equal to the packet length";
        /// <summary>
        /// Represents the message when the serialization returns empty string.
        /// </summary>
        public const string SERIALIZATION_ERROR_EXCEPTION = "Serialization returned empty string.";
        /// <summary>
        /// Represents the Advanced Encryption Standard Key for encrypting/decrypting.
        /// </summary>
        public const string AES_KEY = "x!A%D*G-KaPdSgVkYp3s6v9y$B?E(H+M";
        /// <summary>
        /// Represents the Initialization vector that is used to salt the encryption/decryption
        /// for more complexity.
        /// </summary>
        public const string AES_IV = "15CV1/ZOnVI3rY4wk4INBg==";
        /// <summary>
        /// Represents the ip address that the client will be connected to.
        /// </summary>
        public const string IP = "127.0.0.1";
        /// <summary>
        /// Represents the port that is mapped to the server port.
        /// </summary>
        public const int PORT = 1669;
        /// <summary>
        /// Represents the header size that is 4 bytes.
        /// </summary>
        public const int HEADER_SIZE = 4;
        /// <summary>
        /// Represents the maximum message size that the server can receive
        /// 1024 = 1KB, 1024 * 1024 = 1MB, etc.
        /// </summary>
        public const int MAXIMUM_MESSAGE_SIZE = 1024 * 1024;
        /// <summary>
        /// Represents the maximum response time out which is 30 seconds. 
        /// </summary>
        public const int RESPONSE_TIME_OUT = 30 * 1000;
        /// <summary>
        /// Represents the delay to send all of the pending messages before the client is closed.
        /// </summary>
        public const int LINGER_DELAY_SECONDS = 10;
    }
}