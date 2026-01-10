using MudBlazor;

namespace TrackingPage.Layout;

public class Themes
{
    public static MudTheme DefaultTheme => new()
    {
        PaletteLight = new PaletteLight()
        {
            // Основной цвет — фирменный синий Telegram
            Primary = "#0088CC",
            // Вторичный — мягкий оттенок синего для акцентов
            Secondary = "#2B90D9",
            // Информационные элементы — светло-голубые
            Info = "#26A5E4",
            // Верхняя панель делает интерфейс ощущаться как в мессенджере — светлая
            AppbarBackground = "#FFFFFF",
            // Общий фон — очень светлый, близкий к интерфейсу Telegram
            Background = "#F5F7FA",
            // Фон выдвижных панелей — белый, чтобы элементы выглядели "на поверхности"
            DrawerBackground = "#FFFFFF",
            // Нейтральный цвет текста и акцентов
            TextPrimary = "#1F2933",
            ActionDefault = "#2B90D9"
        },
        PaletteDark = new PaletteDark()
        {
            // Сохранить синий как акцент и для кнопок
            Primary = "#26A5E4",
            // Основной "surface" тёмный, но с холодным синим оттенком, похожим на тёмную тему мессенджера
            Surface = "#0F1720",
            // Глубокий фон для комфортного чтения в тёмной теме
            Background = "#32333D",
            AppbarBackground = "#0B1320",
            DrawerBackground = "#0F1720",
            // Текст должен быть светлым и контрастным на тёмном фоне
            TextPrimary = "#E6F4FF",
            // Дефолтное действие — мягкий голубой
            ActionDefault = "#8ABBE6"
        },
        LayoutProperties = new LayoutProperties()
        {
            // Более закруглённые элементы, близкие к "пузырькам" мессенджеров
            DefaultBorderRadius = "12px"
        }
    };
}

