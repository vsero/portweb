using T = TrackingPage.Services;
using U = UTrack;

namespace TrackingPage.Extentions;

public static class UserContextExtentions
{

    public static U.TgUser? ToApiModel(this T.TgUser tguser)
    {
        if (tguser == null) return null;

        return new U.TgUser
        {
            Id = tguser.Id,
            Username = tguser.Username,
            FirstName = tguser.FirstName
        };
    }

}
