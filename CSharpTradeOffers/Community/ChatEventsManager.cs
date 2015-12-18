using System;
using System.Threading;

namespace CSharpTradeOffers.Community
{
    public class ChatEventsManager
    {
        private bool _searching;

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

        public event OnMessageReceived MessageReceived;
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

        public event OnPoll PollReceived;
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

        public event OnChatMessage ChatMessageReceived;
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

        public event OnTyping OnUserTyping;
        #endregion

        private readonly SteamChatHandler _chatHandler;

        public ChatEventsManager(SteamChatHandler chatHandler)
        {
            _chatHandler = chatHandler;
            BeginMessageLoop(TimeSpan.FromSeconds(1));
        }

        public ChatEventsManager(SteamChatHandler chatHandler, TimeSpan waitAfterPoll)
        {
            _chatHandler = chatHandler;
            BeginMessageLoop(waitAfterPoll);
        }

        /// <summary>
        /// Automatically polls, required to subscribe to chat-related events
        /// </summary>
        /// <param name="waitAfterPoll">Time to wait wait before each poll</param>
        private void BeginMessageLoop(TimeSpan waitAfterPoll)
        {
            if (_searching) return;
            var messageThread = new Thread(() => MessageLoop(waitAfterPoll));
            messageThread.Start();
        }

        /// <summary>
        /// Starts a message loop that allows events to be subsribed to.
        /// </summary>
        /// <param name="waitAfterPoll">Wait before each poll.</param>
        private void MessageLoop(TimeSpan waitAfterPoll)
        {
            _searching = true;
            while (_searching)
            {
                Thread.Sleep(waitAfterPoll);
                PollResponse response = _chatHandler.Poll();
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
}
