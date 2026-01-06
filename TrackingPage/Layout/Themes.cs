using MudBlazor;

namespace TrackingPage.Layout;

public class Themes
{
    public static MudTheme DefaultTheme => new()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#2ecc71",
            Secondary = "#e74c3c",
            Info = "#3498db",
            AppbarBackground = "#2c3e50",
            Background = "#f8f9fa",
            DrawerBackground = "#ffffff"
        },
        PaletteDark = new PaletteDark()
        {
            Primary = "#2ecc71",
            Surface = "#1e1e2d",
            Background = "#1a1a27",
            AppbarBackground = "#1a1a27",
            DrawerBackground = "#1e1e2d",
            TextPrimary = "#ffffff",
            ActionDefault = "#adadb1"
        },
        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "8px" // Можно заодно настроить скругления
        }
    };
}

