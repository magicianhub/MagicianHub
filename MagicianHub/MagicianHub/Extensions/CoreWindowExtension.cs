using Windows.UI.Core;

namespace MagicianHub.Extensions
{
    public static class CoreWindowExtension
    {
        private static bool _isActive = true;

        public static void InitExtension(this CoreWindow coreWindow)
        {
            coreWindow.Activated += CoreWindowOnActivated;
        }

        private static void CoreWindowOnActivated(
            CoreWindow sender,
            WindowActivatedEventArgs args
        )
        {
            _isActive = args.WindowActivationState !=
                        CoreWindowActivationState.Deactivated;
        }
        
        public static bool IsActive(this CoreWindow coreWindow) => _isActive;
    }
}
