// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.ChatLog
{
    using Eco.Core.Plugins.Interfaces;
    using Eco.Core.Utils;
    using Eco.Core.Utils.Logging;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Systems.Messaging.Chat;
    using Eco.Shared.Localization;
    using System;
    using System.Collections.Concurrent;
    using Eco.Gameplay.Systems.Messaging.Chat.Channels;
    using PathUtils = Eco.Shared.Utils.PathUtils;
    using Eco.Shared.Logging;

    /// <summary> Logs chat and related information (such as login/logout events) to plain text files for server administration purposes. </summary>
    class ChatLogger : IModKitPlugin, IInitializablePlugin
    {
        const string ChatLogNamePrefix = "Chat";

        readonly ConcurrentDictionary<(string, string), ILogWriter> loggers = new();

        public string GetCategory()         => Localizer.DoStr("System");
        public override string ToString()   => Localizer.DoStr("Chat log");
        public string GetStatus()           => string.Empty;

        public void Initialize(TimedTask timer)
        {
            UserManager.NewUserJoinedEvent.Add(user => { this.LogLoginEvent(Localizer.DoStr($"--> {user.Name} joined the server.")); });
            UserManager.OnUserLoggedOut.Add(user => { this.LogLoginEvent(Localizer.DoStr($"<-- {user.Name} logged out.")); });
            UserManager.OnUserLoggedIn.Add(user => { this.LogLoginEvent(Localizer.DoStr($"--> {user.Name} logged in.")); });
            ChatManager.MessageSent.Add(this.OnMessageSent);
        }

        /// <summary>Formats an incoming <seealso cref="ChatMessage"/> and logs it to its relevant log file.</summary>
        /// <param name="msg"><see cref="ChatMessage"/> instance to format and log.</param>
        void OnMessageSent(ChatMessage msg)
        {
            string category;
            string name;
            switch (msg.Receiver)
            {
                case Channel channel:
                    category = "Channel";
                    name     = channel.Name;
                    break;

                case User user:
                    category = "Whisper";

                    var recipientName = user.Name;
                    var senderName    = msg.Sender.Name;

                    // Make sure that the names are always in the same order
                    name = string.Compare(senderName, recipientName, StringComparison.Ordinal) < 0 ? $"{senderName}-{recipientName}" : $"{recipientName}-{senderName}";
                    break;

                default:
                    return;
            }

            // Separate the username from the director character
            this.LogMessage(category, name, $"{msg.Sender}: {msg.Text}");
        }

        /// <summary>Logs a user's login event to the General channel log file and the Login events log file.</summary>
        /// <param name="message">Login event message to process.</param>
        void LogLoginEvent(string message)
        {
            this.LogMessage("Login", string.Empty, message);
            this.LogMessage("Channel", ChannelManager.Obj.Get(SpecialChannel.General).Name, message);
        }

        /// <summary>Logs a message to a specific NLog category to be written to file.</summary>
        /// <param name="category">Category of the log message</param>
        /// <param name="name">Name of the NLog logger used to log the message.</param>
        /// <param name="message">Message to log with NLog.</param>
        void LogMessage(string category, string name, string message)
        {
            var logger = this.loggers.GetOrAdd((category, name), ((string Category, string Name) key) =>
            {
                // Generates log name as combination of category and sanitized name
                var logName = $"{ChatLogNamePrefix}/{key.Category}/{PathUtils.SanitizeFileName(key.Name)}";
                return NLogManager.GetLogWriter(logName);
            });
            logger.Write(message);
        }
    }
}
