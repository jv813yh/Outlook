using Outlook.Core.Interfaces;
using Prism.Commands;

namespace Outlook.Core.Commands
{
    public class ApplicationCommands : IApplicationCommands
    {
        private CompositeCommand _navigateCommand;

        public ApplicationCommands()
        {
            _navigateCommand = new CompositeCommand();
        }

        public CompositeCommand NavigateCommand 
            => _navigateCommand;
    }
}
