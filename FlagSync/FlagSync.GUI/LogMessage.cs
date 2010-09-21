using System;
using System.Collections.Generic;
using System.Text;

namespace FlagSync.GUI
{
    public class LogMessage
    {
        public enum MessageType
        {
            ErrorMessage,
            StatusMessage,
            SuccessMessage
        }

        private string message;
        private MessageType type;

        public string Message
        {
            get
            {
                return message;
            }
        }

        public MessageType Type
        {
            get 
            { 
                return type; 
            }
        }

        public LogMessage(string message, MessageType type)
        {
            this.message = message;
            this.type = type;
        }
    }
}
