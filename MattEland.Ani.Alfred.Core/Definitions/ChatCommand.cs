// ---------------------------------------------------------
// ChatCommand.cs
// 
// Created on:      08/11/2015 at 3:59 PM
// Last Modified:   08/11/2015 at 10:18 PM
// Original author: Matt Eland
// ---------------------------------------------------------

using System;
using System.Globalization;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    ///     Represents a command that the chat system is requesting Alfred execute.
    ///     This command will be routed to a subsystem for processing
    /// </summary>
    public struct ChatCommand : IEquatable<ChatCommand>
    {
        /// <summary>
        ///     Gets the empty command for null-ish comparison.
        /// </summary>
        public static readonly ChatCommand Empty = new ChatCommand();

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChatCommand" /> struct.
        /// </summary>
        /// <param name="command">The command.</param>
        public ChatCommand(string command) : this(null, command, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChatCommand" /> struct.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        /// <param name="command">The command.</param>
        public ChatCommand(string subsystem, string command) : this(subsystem, command, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChatCommand" /> struct.
        /// </summary>
        /// <param name="subsystem">The subsystem.</param>
        /// <param name="command">The command.</param>
        /// <param name="data">The data.</param>
        public ChatCommand(string subsystem, string command, string data) : this()
        {
            Subsystem = subsystem;
            Command = command;
            Data = data;
        }

        /// <summary>
        ///     Gets the subsystem the command is intended for.
        /// </summary>
        /// <value>The subsystem.</value>
        public string Subsystem { get; }

        /// <summary>
        ///     Gets the name of the command to execute. This will be interpreted by the subsystem.
        /// </summary>
        /// <value>The name of the command.</value>
        /// <remarks>
        /// TODO: This might be better off as Name as Command leads to syntax like: command.Command
        /// </remarks>
        public string Command { get; }

        /// <summary>
        ///     Gets any extra data or parameters needed to execute the command.
        /// </summary>
        /// <value>The extra data.</value>
        public string Data { get; }

        /// <summary>
        ///     Determines if this instance is equal to another instance
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns><c>true</c> if the instances are equal, <c>false</c> otherwise.</returns>
        public bool Equals(ChatCommand other)
        {
            return string.Equals(Subsystem, other.Subsystem) && string.Equals(Command, other.Command) &&
                   string.Equals(Data, other.Data);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            return string.Format(CultureInfo.CurrentCulture, Resources.ChatCommandToString, Subsystem, Command, Data);
        }


        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is ChatCommand && Equals((ChatCommand)obj);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Subsystem?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (Command?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Data?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        /// <summary>
        ///     Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ChatCommand left, ChatCommand right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ChatCommand left, ChatCommand right)
        {
            return !left.Equals(right);
        }
    }
}