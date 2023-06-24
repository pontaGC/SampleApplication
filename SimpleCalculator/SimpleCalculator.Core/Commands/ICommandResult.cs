using System.Diagnostics.CodeAnalysis;

namespace SimpleCalculator.Core.Commands
{
    /// <summary>
    /// The execution result of a command.
    /// </summary>
    /// <typeparam name="TItem">The type of an item as the execution result.</typeparam>
    public interface ICommandResult<TItem>
    {
        /// <summary>
        /// Gets a value indcating whether or not a command is executed unsuccessfully.
        /// </summary>
        bool IsFailed { get; }

        /// <summary>
        /// Gets a value indcating whether or not a command is executed successfully.
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Gets an error reason.
        /// </summary>
        [NotNull]
        public string Error { get; }

        /// <summary>
        /// Gets an item obtrained as the execution result.
        /// </summary>
        TItem Item { get; }
    }
}
