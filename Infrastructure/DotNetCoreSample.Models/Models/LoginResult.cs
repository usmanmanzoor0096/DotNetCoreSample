using AuthService.Models.DBModels;

namespace AuthService.Models.Models
{
    public class LoginResult
    {
        public LoginResult()
        {
        }
        public OAuthToken Token { get; set; }
        public IError Error { get; set; }
        public bool Succeeded { get; set; }
        public string LocationState { get; set; }
        public string Clientid { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public DateTime ExpiredDate { get; set; }
        public bool HasError
        {
            get
            {
                return Error != null && !string.IsNullOrEmpty(Error.Text);
            }
        }
        public bool FurtherAction { get; set; }
        public bool IsPowerUser { get; set; }
    }

    public enum LoginErrorCodes
    {
        InvalidUserNamePassword = 1,
        LocationExpired = 2,
        UserExpired = 3,
        InvalidClientId = 4,
        InvalidGrantType = 5,
        FailedRefreshToken = 6,
        TrialUserDeactivated = 7,
        Defaulted = 8,
        Canceled = 9,
        InActive = 10,
        TokenNotGenerated = 11,
        TrialUserExpired = 12,
        ClientDeleted = 13,
        LocationDeleted = 14,
        PendingDues = 15,
        V3User = 16,
        UnAuthorized = 17,
        SystemGeneratedError = 18,
        DDSetupPending = 19,
        AccountClosed = 20,
        NoLocationFound = 21
    }

    public interface IError
    {
        int Code { get; }
        string Heading { get; }
        string Text { get; }
    }

    public class CustomeError : IError
    {
        public int Code { get; set; }
        public string Heading { get; set; }
        public string Text { get; set; }
    }

    public class InvalidUserNamePassword : IError
    {
        public InvalidUserNamePassword()
        {
            Code = (int)LoginErrorCodes.InvalidUserNamePassword;
            Heading = "Error!";
            Text = "Invalid email or password";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }
        public string Text { get; private set; }
    }

    public class ClientDefaulted : IError
    {
        public ClientDefaulted()
        {
            Code = (int)LoginErrorCodes.Defaulted;
            Heading = "Error!";
            Text = "You have pending dues. Please contact your organisation supervisor";
        }

        public int Code { get; private set; }
        public string Heading { get; set; }
        public string Text { get; set; }
    }
    public class ClientCanceled : IError
    {
        public ClientCanceled()
        {
            Code = (int)LoginErrorCodes.Canceled;
            Heading = "Error!";
            Text = "You have pending dues. Please contact your organisation supervisor";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }
        public string Text { get; set; }
    }
    public class ClientAccountClosed : IError
    {
        public ClientAccountClosed()
        {
            Code = (int)LoginErrorCodes.AccountClosed;
            Heading = "Error!";
            Text = "The account you are trying to login has been closed";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; set; }
    }
    public class ClientExpired : IError
    {
        public ClientExpired()
        {
            Code = (int)LoginErrorCodes.LocationExpired;
            Heading = "Error!";
            Text = "Client has been expired.";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; private set; }
    }

    public class UserExpired : IError
    {
        public UserExpired()
        {
            Code = (int)LoginErrorCodes.UserExpired;
            Heading = "Error!";
            Text = "User account has been expired.";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; private set; }
    }
    public class TrialUserExpired : IError
    {
        public TrialUserExpired()
        {
            Code = (int)LoginErrorCodes.TrialUserExpired;
            Heading = "Error!";
            Text = "TrialUser account has been expired.";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; private set; }
    }
    public class InvalidClientId : IError
    {
        public InvalidClientId()
        {
            Code = (int)LoginErrorCodes.InvalidClientId;
            Heading = "Error!";
            Text = "Invalid client_id.";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; private set; }
    }

    public class InvalidGrantType : IError
    {
        public InvalidGrantType()
        {
            Code = (int)LoginErrorCodes.InvalidGrantType;
            Heading = "Error!";
            Text = "Invalid grant_type.";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; private set; }
    }

    public class FailedRefreshToken : IError
    {
        public FailedRefreshToken()
        {
            Code = (int)LoginErrorCodes.FailedRefreshToken;
            Heading = "Error!";
            Text = "Failed to refresh token.";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; private set; }
    }

    public class TrialUserDeactivated : IError
    {
        public TrialUserDeactivated()
        {
            Code = (int)LoginErrorCodes.TrialUserDeactivated;
            Heading = "Error!";
            Text = "Your account has been de-activated. Please contact your organisation supervisor.";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; private set; }
    }

    public class InActiveUser : IError
    {
        public InActiveUser()
        {
            Code = (int)LoginErrorCodes.InActive;
            Heading = "Error!";
            Text = "User is not active.";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; private set; }
    }

    public class FailedAccessToken : IError
    {
        public FailedAccessToken()
        {
            Code = (int)LoginErrorCodes.TokenNotGenerated;
            Heading = "Error!";
            Text = "Failed to generate access token.";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; private set; }
    }

    public class ClientDeleted : IError
    {
        public ClientDeleted()
        {
            Code = (int)LoginErrorCodes.ClientDeleted;
            Heading = "Error!";
            Text = "Client has been deleted.";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; private set; }
    }
    public class LocationDeleted : IError
    {
        public LocationDeleted()
        {
            Code = (int)LoginErrorCodes.LocationDeleted;
            Heading = "Error!";
            Text = "Location has been deleted.";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; private set; }
    }
    public class SysteAccessBlocked : IError
    {
        public SysteAccessBlocked()
        {
            Code = (int)LoginErrorCodes.InActive;
            Heading = "Error!";
            Text = "System access has been blocked.";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; private set; }
    }
    public class LocationNotFound : IError
    {
        public LocationNotFound()
        {
            Code = (int)LoginErrorCodes.NoLocationFound;
            Heading = "Error!";
            Text = "No location found.";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; private set; }
    }

    public class V3User : IError
    {
        public V3User()
        {
            Code = (int)LoginErrorCodes.V3User;
            Heading = "Error!";
            Text = "This is a v3 user";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; private set; }
    }

    public class UnAuthorizedUser : IError
    {
        public UnAuthorizedUser()
        {
            Code = (int)LoginErrorCodes.UnAuthorized;
            Heading = "Error!";
            Text = "Un-authorized user to access this application";
        }

        public int Code { get; private set; }
        public string Heading { get; private set; }

        public string Text { get; private set; }
    }

    public class SystemGeneratedError : IError
    {
        public SystemGeneratedError()
        {
            Code = (int)LoginErrorCodes.SystemGeneratedError;
            Heading = "Error!";
            Text = "Error occurred: ";
        }
        public int Code { get; set; }
        public string Heading { get; private set; }

        public string Text { get; set; }
    }

    public class DDSetupPending : IError
    {
        public DDSetupPending()
        {
            Code = (int)LoginErrorCodes.DDSetupPending;
            Heading = "Error!";
            Text = "Step 3 of confirmation is pending";
        }

        public int Code { get; set; }
        public string Heading { get; private set; }

        public string Text { get; set; }
    }
}
