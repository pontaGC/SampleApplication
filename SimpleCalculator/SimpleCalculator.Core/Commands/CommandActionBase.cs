using System.Windows.Input;

namespace EditorAppFramework.Presentation.Core.Commands
{
    /// <summary>
    /// Abstract <see cref="ICommand"/> class.
    /// </summary>
    public abstract class CommandActionBase : ICommand
    {
        #region ICommand

        /// <inheritdoc />
        event EventHandler? ICommand.CanExecuteChanged
        {
            add { this.canExecuteChangedInternal += value; }
            remove { this.canExecuteChangedInternal -= value; }
        }

        private EventHandler? canExecuteChangedInternal;

        /// <inheritdoc />
        bool ICommand.CanExecute(object? parameter)
        {
            return this.CanExecute(parameter);
        }

        /// <inheritdoc />
        void ICommand.Execute(object? parameter)
        {
            this.Execute(parameter);
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method called by <see cref="ICommand"/>.<c>CanExecute(object)</c>;
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns><c>true</c> if this command can be executed; otherwise, <c>false</c>. Typically, a command source calls the <c>CanExecute</c> method when the <c>CanExecuteChanged</c> event is raised.</returns>
        protected abstract bool CanExecute(object? parameter);

        /// <summary>
        /// This method called by <see cref="ICommand"/>.<c>Execute(object)</c>; 
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        protected abstract void Execute(object? parameter);

        /// <summary>
        /// Raised a <c>CanExecuteChanged</c> event.
        /// </summary>
        public virtual void RaiseCanExecuteChanged()
        {
            this.canExecuteChangedInternal?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
