using System;
using System.Linq;
using System.Collections.Generic;
using IRISChatServer.DataAccessLayer.Encryption;

namespace IRISChatServer.DataAccessLayer.Messages
{
    public static class ProcessMessage
    {
        #region "Private Static Methods"
        private static IMessage DeserializeToMessage(string MessageType, object Message)
        {
            if (MessageType.Equals(nameof(RecoverUserPasswordResultMessage)))
            {
                return Serialization.Deserialize<RecoverUserPasswordResultMessage>(Convert.ToString(Message));
            }
            else if (MessageType.Equals(nameof(RegisterUserResultMessage)))
            {
                return Serialization.Deserialize<RegisterUserResultMessage>(Convert.ToString(Message));
            }
            else if (MessageType.Equals(nameof(SignInUserResultMessage)))
            {
                return Serialization.Deserialize<SignInUserResultMessage>(Convert.ToString(Message));
            }
            else if (MessageType.Equals(nameof(SignOutUserResultMessage)))
            {
                return Serialization.Deserialize<SignOutUserResultMessage>(Convert.ToString(Message));
            }
            else if (MessageType.Equals(nameof(UpdateUserProfileResultMessage)))
            {
                return Serialization.Deserialize<UpdateUserProfileResultMessage>(Convert.ToString(Message));
            }
            else if (MessageType.Equals(nameof(DeleteUserAccountResultMessage)))
            {
                return Serialization.Deserialize<DeleteUserAccountResultMessage>(Convert.ToString(Message));
            }
            else
            {
                return null;
            }
        }

        private static IEnumerable<Type> GetRegisteredMessages()
        {
            Type Messagetype = typeof(IMessage);
            IEnumerable<Type> RegisteredTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => Messagetype.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);
            return RegisteredTypes;
        }
        #endregion

        #region "Public Static Methods"
        public static ResponseMessage Process(byte[] EncryptedPacket)
        {
            try
            {
                //1. Decrypt the AES from the Encrypted Packet, if an exception occured we will return null.
                string DecryptedJsonString = AES256.BytesToString(AES256.Decrypt(EncryptedPacket));
                //2. Deserialize the DecryptedJsonString into MessageWrapper, if an exception occured we will return null.
                ResponseMessage Response = Serialization.Deserialize<ResponseMessage>(DecryptedJsonString);
                //3. Validate the Message and check if the MessageType are registered.
                bool IsMessageTypeRegistered = false;
                foreach (Type RegisteredType in GetRegisteredMessages())
                {
                    if (RegisteredType.Name.Equals(Response.MessageType))
                    {
                        IsMessageTypeRegistered = true;
                        break;
                    }
                }
                //4. If Message type is not found then return null, source can be from an attacker or unknown source.
                if (IsMessageTypeRegistered == false)
                {
                    return null;
                }
                else
                {
                    //5. Dserialize the message by using the MessageType and return the result.
                    Response.Message = DeserializeToMessage(Response.MessageType, Response.Message);
                    //6. Return the response to the client.
                    return Response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion
    }
}