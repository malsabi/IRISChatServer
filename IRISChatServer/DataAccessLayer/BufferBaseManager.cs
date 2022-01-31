using System;
using System.Collections.Generic;
using IRISChatServer.Enums;
using IRISChatServer.DataAccessLayer.Messages;
using IRISChatServer.Configs;

namespace IRISChatServer.DataAccessLayer
{
    public class BufferBaseManager : IDisposable
    {
        #region "Properties"
        public byte[] HeaderBuffer { get; private set; }

        public byte[] MessageBuffer { get; private set; }

        public int MessageSize { get; set; }

        public BufferState State { get; set; }

        public List<ResponseMessage> Responses { get; set; }
        #endregion

        #region "Constructors / Destructors"
        public BufferBaseManager()
        {
            HeaderBuffer = new byte[DataAccessLayerConfig.HEADER_SIZE];
            MessageBuffer = null;
            MessageSize = -1;
            State = BufferState.Header;
            Responses = new List<ResponseMessage>();
        }

        ~BufferBaseManager()
        {
            Dispose();
        }
        #endregion

        #region "Buffer"
        public void ReAllocate()
        {
            if (MessageSize > 0)
            {
                MessageBuffer = new byte[MessageSize];
            }
        }

        public void DeAllocate()
        {
            if (MessageBuffer != null)
            {
                MessageBuffer = null;
                GC.Collect();
            }
        }

        public void Dispose()
        {
            if (Responses != null)
            {
                Responses.Clear();
                Responses = null;
            }
            HeaderBuffer = null;
            MessageBuffer = null;
        }
        #endregion
    }
}