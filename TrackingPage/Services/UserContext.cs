namespace TrackingPage.Services;

public class UserContext
{
    public event Action? OnChange;

    private bool _isMobile;
    private bool _isDarkMode;
    private bool _isTg;

    public bool IsMobile
    {
        get => _isMobile;
        set
        {
            if (_isMobile != value)
            {
                _isMobile = value;
                NotifyChanged();
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
                NotifyChanged();
            }
        }
    }

    public bool IsTg
    {
        get => _isTg;
        set
        {
            if (_isTg != value)
            {
                _isTg = value;
                NotifyChanged();
            }
        }
    }
    public TgUser TgUser { get; set; } = new();

    private void NotifyChanged() => OnChange?.Invoke();

}

public class TgInitData
{
    public TgUser? User { get; set; }
    public string QueryId { get; set; } = string.Empty;
}

public class TgUser
{
    public long Id { get; set; }
    public string? Username { get; set; } = string.Empty;
    public string? FirstName { get; set; } = string.Empty;
}
