using System.ComponentModel;

namespace AuthService.Common.Enums
{
    public class APIEnums
    {
        public enum APIStatusCodes
        {
            InternalServerError = 1,
            UserAlreadyRegistered = 2,
            InvalidEmailOrPassword = 3,
            PasswordRequired = 4,
            ExceededLoginAttempts = 5,
            EmailNotVerified = 6,
            InvalidPassword = 7,
            UserNotFound = 8,
            GeneralError = 9,
            UserAlreadyVerified = 10,
            UserRegistrationError = 11,
            RegisterSuccess = 12,
            InvalidCode = 13,
            EmailError = 14,
            EmailAlreadyRegistered = 15,
            InviteUrlExpire = 16,
            UserEmailNotVerified = 17,
            UserPhoneNotVerified = 18,
            UserEmailAlreadyVerified = 19,
            UserDisabledError = 20,
            InvalidRole = 21,
            RoleAlreadyExist = 22,
            NoArtistFound = 23,
            ArtRatingSavedUnsuccessfull = 24,
            ArtRatingSavedSuccessful = 25,
            NoDataFound = 26,
            NoDropAvailable = 27,
            StripePaymentCompleted = 28,
            StripePaymentFailure = 29,
            BlockChainCompleted = 30,
            BlockChainFailure = 31,
            ArtistFanExists = 32,
            AristFansSuccessful = 33,
            BidAuthorizeSuccessful = 34,
            BidAuthorizeFailure = 35,
            HigherBidExist = 36,
            PaymentSucessButErrorInMintingAndOrder = 37,
            PaymentProcessingError = 38,
            DropSupplyScriptError = 39,
            ErrorOnOrderInsertionDB = 40,
            TransactionConfrimation = 41,
            PayPalTokenNotFound = 42,
            PayPalAccessTokenError = 43,
            PayPalPaymentUrlError = 44,
            PaypalRedirectUrlException = 45,
            PayPalPaymentError = 46,
            PayPalPaymentException = 47,
            Success = 48,
            Failure = 49,
            PaypalRefundCompleted = 50,
            PaypalRefundFailure = 51,
            PaypalSaleIdNotFound = 52,
            Error = 53,
            S3BucketAlreadyExist= 54,
            S3BucketNotExist = 55,

        }
        public enum EnumPaymentSource
        {
            Stripe = 1,
            Blocto = 2,
            Crypto = 3,
            Circle = 4,
            MoonPay = 5,
            PayPal = 6
        }
        public enum SocialMediaType
        {
            Google = 1,
            Twitter,
            Facebook,
            Discord,
            Instagram
        }
        public enum Reason
        {
            [Description("duplicate")]
            duplicate,
            [Description("fraudulent")]
            fraudulent,
            [Description("requested_by_customer")]
            requested_by_customer,
            [Description("bank_transaction_error")]
            bank_transaction_error,
            [Description("invalid_account_number")]
            invalid_account_number,
            [Description("insufficient_funds")]
            insufficient_funds,
            [Description("payment_stopped_by_issuer")]
            payment_stopped_by_issuer,
            [Description("payment_returned")]
            payment_returned,
            [Description("bank_account_ineligible")]
            bank_account_ineligible,
            [Description("invalid_ach_rtn")]
            invalid_ach_rtn,
            [Description("unauthorized_transaction")]
            unauthorized_transaction,
            [Description("payment_failed")]
            payment_failed
        }
        public enum ClientType
        {
            CircleHttpClient = 1,
            LicenceNodeAPI,
            DapperAPI,
            AuthServer,
            DotNetCoreSample
        }
        public enum CirclePyamentSourceType
        {
            card,
            ach
        }
        public enum CirclePyamentVerificationType
        {
            cvv,
            three_d_secure
        }
        public enum PaymentType
        {
            USDC,
            CreditCard
        }
        public enum BlockChainScriptEnum
        {
            MintAndTransferScript
        }

        public enum EmailTemplates
        {
            ForgotEmailTemplate,
            VerfiyEmailTemplate,
            ResetPasswordTemplate,
            StoreFrontApprovalEmailTemplate,
            KYBCompletedEmailTemplate,
            ArweaveTokenRunningOutTemplate,
            CongragulationTemplate,
            SucessfullOrderEmailTemplate,
            UnSucessfullOrderEmailTemplate,
            SetPasswordTemplate,
            OrderConfirmationEmailTemplate,
            OrderSuccessFullEmailTemplate
        }
    }
}
