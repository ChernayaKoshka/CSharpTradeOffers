using System;
using System.Threading;

namespace CSharpTradeOffers.Community
{
    public class ChatEventsManager
    {
        public event OnTyping OnUserTyping;

        public event OnChatMessage ChatMessageReceived;

        public event OnPoll PollReceived;

        public event OnMessageReceived MessageReceived;

        private bool _searching;

        private readonly SteamChatHandler _chatHandler;

        public ChatEventsManager(SteamChatHandler chatHandler)
        {
            _chatHandler = chatHandler;
            BeginMessageLoop(TimeSpan.FromSeconds(1));
        }

        public ChatEventsManager(SteamChatHandler chatHandler, TimeSpan waitAfterPoll, int secTimeOut = 0)
        {
            _chatHandler = chatHandler;
            BeginMessageLoop(waitAfterPoll, secTimeOut);
        }

        /// <summary>
        /// Automatically polls, required to subscribe to chat-related events
        /// </summary>
        /// <param name="waitAfterPoll">Time to wait wait before each poll</param>
        private void BeginMessageLoop(TimeSpan waitAfterPoll, int secTimeOut = 0)
        {
            if (_searching) return;
            var messageThread = new Thread(() => MessageLoop(waitAfterPoll,secTimeOut));
            messageThread.Start();
        }

        /// <summary>
        /// Starts a message loop that allows events to be subsribed to.
        /// </summary>
        /// <param name="waitAfterPoll">Wait before each poll.</param>
        private void MessageLoop(TimeSpan waitAfterPoll, int secTimeOut = 0)
        {
            _searching = true;
            while (_searching)
            {
                Thread.Sleep(waitAfterPoll);
                PollResponse response = _chatHandler.Poll(secTimeOut);
                PollReceived?.Invoke(this, new PollArgs(response));

                if (response.Messages == null) continue;
                foreach (Message message in response.Messages)
                {
                    MessageReceived?.Invoke(this, new MessageArgs(message));
                    switch (message.Type.ToLower())
                    {
                        case "saytext":
                            ChatMessageReceived?.Invoke(this, new ChatMessageArgs(message));
                            break;
                        case "typing":
                            OnUserTyping?.Invoke(this, new TypingArgs(message));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Stops the message loop
        /// </summary>
        public void EndMessageLoop()
        {
            if (_searching) _searching = false;
        }
    }

    #region OnMessageReceived
    public class MessageArgs : EventArgs
    {
        public MessageArgs(Message message)
        {
            Message = message;
        }

        public Message Message;
    }

    public delegate void OnMessageReceived(object sender, MessageArgs e);
    #endregion

    #region OnPoll
    public class PollArgs : EventArgs
    {
        public PollArgs(PollResponse response)
        {
            Response = response;
        }

        public PollResponse Response;
    }

    public delegate void OnPoll(object sender, PollArgs e);
    #endregion

    #region OnChatMessage
    public class ChatMessageArgs : EventArgs
    {
        public ChatMessageArgs(Message chatMessage)
        {
            ChatMessage = chatMessage;
        }

        public Message ChatMessage;
    }

    public delegate void OnChatMessage(object sender, ChatMessageArgs e);
    #endregion

    #region OnUserTyping
    public class TypingArgs : EventArgs
    {
        public TypingArgs(Message typingMessage)
        {
            TypingMessage = typingMessage;
        }

        public Message TypingMessage;
    }

    public delegate void OnTyping(object sender, EventArgs e);
    #endregion
}
