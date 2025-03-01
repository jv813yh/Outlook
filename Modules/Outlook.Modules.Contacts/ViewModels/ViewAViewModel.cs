using Outlook.Core.ViewModels;
using Prism.Regions;

namespace Outlook.Modules.Contacts.ViewModels
{
    public class ViewAViewModel : ViewModelBase, IRegionMemberLifetime
    {
        public bool KeepAlive 
            => false;
    }
}
