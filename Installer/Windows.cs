using System;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Install
{
    public static class Windows
    {
        //public static DisclaimerWindow DisclaimerWindow = new DisclaimerWindow();
        public static LoginWindow LoginWindow = new LoginWindow();
        public static ProgressWindow ProgressWindow = new ProgressWindow();
        public static SteamGuardWindow SteamGuardWindow = new SteamGuardWindow();
        public static OptionsWindow OptionsWindow = new OptionsWindow();
        public static CompleteWindow CompleteWindow = new CompleteWindow();

        public static void WindowClose(Window windowToHide, Window nextWindow = null)
        {
            // Hide the specified window
            windowToHide.Hide();

            // If a next window is provided, show the next window
            nextWindow?.Show();

            // Check if all windows in the Windows class are hidden
            bool allHidden = true;
            foreach (FieldInfo field in typeof(Windows).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.GetValue(null) is Window window && window.IsVisible)
                {
                    allHidden = false;
                    break;
                }
            }

            // Exit the application if all windows are hidden
            if (allHidden)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
