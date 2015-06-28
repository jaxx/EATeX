using EA;

namespace EATeX
{
    public interface IEAInterop
    {
        string EA_Connect(Repository repository);
        void EA_Disconnect();
        object EA_GetMenuItems(Repository repository, string menuLocation, string menuName);
        void EA_MenuClick(Repository repository, string menuLocation, string menuName, string itemName);
    }
}