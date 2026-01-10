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

    public bool IsTg { get; set; }
    public long TgUserId { get; set; }
    public string TgUserName { get; set; } = string.Empty;
    public string TgChatId { get; set; } = string.Empty;
    public class TgInitData { public TgUser User { get; set; } }
    public class TgUser { public long Id { get; set; } public string Username { get; set; } }

}
