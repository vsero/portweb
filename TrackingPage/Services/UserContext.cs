namespace TrackingPage.Services;

public class UserContext
{
    public event Action? OnContextChanged;

    private bool _isMobile;
    private bool _isDarkMode;

    public bool IsMobile
    {
        get => _isMobile;
        set
        {
            if (_isMobile != value)
            {
                _isMobile = value;
                OnContextChanged?.Invoke();
            }
        }
    }

    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            if (_isDarkMode != value)
            {
                _isDarkMode = value;
                OnContextChanged?.Invoke();
            }
        }
    }

}
