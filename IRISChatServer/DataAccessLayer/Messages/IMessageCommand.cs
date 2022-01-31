namespace IRISChatServer.DataAccessLayer.Messages
{
    public interface IMessageCommand
    {
        /// <summary>
        /// Decides whether this message processor can execute the specified message.
        /// </summary>
        /// <param name="Message">The message to execute.</param>
        /// <returns>Returns true if the message can be executed in the message command.</returns>
        bool CanExecute(IMessage Message);

        /// <summary>
        /// Decides whether this message command can execute messages received from the sender.
        /// </summary>
        /// <param name="Sender">The sender of a message.</param>
        /// <returns>Returns true if the message command can execute messages received from the sender otherwise false.</returns>
        bool CanExecuteFrom(object Sender);

        /// <summary>
        /// Executes the received message.
        /// </summary>
        /// <param name="Sender">The sender of this message.</param>
        /// <param name="Message">The received message.</param>
        void Execute(object Sender, IMessage Message);
    }
}