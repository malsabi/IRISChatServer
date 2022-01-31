using System;
using IRISChatServer.Configs;

namespace IRISChatServer.Helpers
{
    public class SocketHelper
    {
        public static byte[] AppendHeader(byte[] Packet)
        {
            byte[] Message = new byte[Packet.Length + DataAccessLayerConfig.HEADER_SIZE];
            Buffer.BlockCopy(BitConverter.GetBytes(Packet.Length), 0, Message, 0, DataAccessLayerConfig.HEADER_SIZE);
            Buffer.BlockCopy(Packet, 0, Message, DataAccessLayerConfig.HEADER_SIZE, Packet.Length);
            return Message;
        }
    }
}