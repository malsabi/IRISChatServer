using System.Net.Sockets;

namespace IRISChatServer.Extensions
{
    public static class SocketExtension
    {
        public static bool IsConnected(this Socket Handler)
        {
            if (Handler != null && Handler.Connected)
            {
                try
                {
                    if (Handler.Poll(0, SelectMode.SelectRead))
                    {
                        if (Handler.Receive(new byte[1], SocketFlags.Peek) == 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}