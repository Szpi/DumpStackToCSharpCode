using System;
using Microsoft.Internal.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace RuntimeTestDataCollector.ThemeColor
{
    public static class VsThemeColorExtension
    {
        private static byte[] GetThemedColorRgba(IVsUIShell5 vsUIShell, ThemeResourceKey themeResourceKey)
        {
            Guid category = themeResourceKey.Category;
            __THEMEDCOLORTYPE colorType = __THEMEDCOLORTYPE.TCT_Foreground;
            if (themeResourceKey.KeyType == ThemeResourceKeyType.BackgroundColor || themeResourceKey.KeyType == ThemeResourceKeyType.BackgroundBrush)
            {
                colorType = __THEMEDCOLORTYPE.TCT_Background;
            }

            // This call will throw an exception if the color is not found
            uint rgbaColor = vsUIShell.GetThemedColor(ref category, themeResourceKey.Name, (uint)colorType);
            return BitConverter.GetBytes(rgbaColor);
        }

        public static System.Windows.Media.Color GetThemedWPFColor(this IVsUIShell5 vsUIShell, ThemeResourceKey themeResourceKey)
        {
            Validate.IsNotNull(vsUIShell, "vsUIShell");
            Validate.IsNotNull(themeResourceKey, "themeResourceKey");

            byte[] colorComponents = GetThemedColorRgba(vsUIShell, themeResourceKey);

            return System.Windows.Media.Color.FromArgb(colorComponents[3], colorComponents[0], colorComponents[1], colorComponents[2]);
        }
    }
}