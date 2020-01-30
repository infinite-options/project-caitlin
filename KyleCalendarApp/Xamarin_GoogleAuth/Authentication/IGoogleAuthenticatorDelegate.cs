using System;

namespace Xamarin_GoogleAuth.Authentication
{
    public interface IGoogleAuthenticationDelegate
    {
        void OnAuthenticationCompleted();
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCanceled();
        //void OnGetCalendarsCompleted(GoogleOAuthToken token);
    }
}
