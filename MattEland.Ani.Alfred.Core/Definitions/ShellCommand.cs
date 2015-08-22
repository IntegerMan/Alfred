using System;

using JetBrains.Annotations;

using MattEland.Common;

namespace MattEland.Ani.Alfred.Core.Definitions
{
    /// <summary>
    /// Represents a command to execute against the application shell
    /// </summary>
    public struct ShellCommand : IEquatable<ShellCommand>
    {
        /// <summary>
        /// Gets the target of the command
        /// </summary>
        /// <value>The target of the command</value>
        [NotNull]
        public string Target { get; }

        /// <summary>
        ///     Gets the name of the command to execute.
        /// </summary>
        /// <value>The name of the command.</value>
        [NotNull]
        public string Name { get; }

        /// <summary>
        ///     Gets any extra data or parameters needed to execute the command.
        /// </summary>
        /// <value>The extra data.</value>
        [NotNull]
        public string Data { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellCommand" /> struct.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <param name="target">The target.</param>
        /// <param name="data">The data.</param>
        public ShellCommand([CanBeNull] string name, [CanBeNull] string target,
                            [CanBeNull] string data)
        {
            Target = target.NonNull();
            Name = name.NonNull();
            Data = data.NonNull();
        }

        /// <summary>
        /// Gets a string representation of this struct
        /// </summary>
        /// <returns>A string representation of this struct</returns>
        public override string ToString()
        {
            return $"Target: {Target}, Name: {Name}, Data: {Data}";
        }

        /// <summary>
        ///     Determines whether the specified <see cref="ShellCommand" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns><c>true</c> if the specified <see cref="ShellCommand" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(ShellCommand other)
        {
            return string.Equals(Target, other.Target) && string.Equals(Name, other.Name) && string.Equals(Data, other.Data);
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
                var hashCode = Target.GetHashCode();
                hashCode = (hashCode * 397) ^ Name.GetHashCode();
                hashCode = (hashCode * 397) ^ Data.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        ///     Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ShellCommand left, ShellCommand right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ShellCommand left, ShellCommand right)
        {
            return !left.Equals(right);
        }
    }
}