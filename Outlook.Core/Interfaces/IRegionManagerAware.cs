using Prism.Regions;

namespace Outlook.Core.Interfaces
{
    public interface IRegionManagerAware
    {
        IRegionManager RegionManager { get; set; }
    }
}
