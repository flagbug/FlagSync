using System;
using System.Collections.Generic;
using System.Text;

namespace FlagSync.GUI
{
    public class LogMessage
    {
        /// <summary>
        /// Type of the log message
        /// </summary>
        public enum MessageType
        {
            /// <summary>
            /// An error message
            /// </summary>
            ErrorMessage,
            /// <summary>
            /// A status message
            /// </summary>
            StatusMessage,
            /// <summary>
            /// A success message
            /// </summary>
            SuccessMessage
        }

        private string message;
        private MessageType type;

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get
            {
                return message;
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public MessageType Type
        {
            get 
            { 
                return type; 
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        public LogMessage(string message, MessageType type)
        {
            this.message = message;
            this.type = type;
        }
    }
}
