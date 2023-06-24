namespace SimpleCalculator.Core.Commands
{
    /// <summary>
    /// The execution result of a command.
    /// </summary>
    /// <typeparam name="TItem">The type of an item as the execution result.</typeparam>
    public class CommandResult<TItem> : ICommandResult<TItem>
    {
        private CommandResult(bool isSucess, string error, TItem item)
        {
            this.IsSuccess = isSucess;
            this.Error = error ?? string.Empty;
            this.Item = item;
        }

        /// <inheritdoc />
        public bool IsFailed => !this.IsSuccess;

        /// <inheritdoc />
        public bool IsSuccess { get; }

        /// <inheritdoc />
        public string Error { get; }

        /// <inheritdoc />
        public TItem Item { get; }

        public static CommandResult<TItem> Succeeded(TItem item)
        {
            return new CommandResult<TItem>(true, string.Empty, item);
        }

        public static CommandResult<TItem> Failed(string error)
        {
            return new CommandResult<TItem>(false, error, default);
        }
    }
}
