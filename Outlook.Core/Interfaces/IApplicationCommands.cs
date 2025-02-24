using Prism.Commands;

namespace Outlook.Core.Interfaces
{
    public interface IApplicationCommands
    {
        CompositeCommand NavigateCommand { get; }
    }
}
