using System.ComponentModel;

namespace AuthService.Models.Enum
{
    public enum EmailTemplate : short
    {
        [Description("Welcome Activation Email")]
        WelcomeActivationEmail = 1,

        [Description("SignUp Success Email")]
        SignUpSuccessEmail = 2,

        [Description("Invite Email")]
        InviteEmail = 3,

        [Description("Forgot Password Email")]
        ForgotPasswordEmail = 4,

        [Description("CardHolder Approve Email")]
        CardHolderApproveEmail = 5,

        [Description("Two FA Code Email")]
        TwoFACode = 6,

        [Description("Set Password Email Template")]
        SetPasswordTemplate = 7,

        [Description("Reset Password Email Template")]
        ResetPasswordTemplate = 8,

        [Description("Client created successfully Template")]
        CongragulationTemplate = 9
    }
}
