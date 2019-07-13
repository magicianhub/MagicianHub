namespace MagicianHub.Authorization
{
    public enum AuthorizationResponseTypes
    {
        Success,
        NeedVerifyCodeByApp,
        NeedVerifyCodeByPhone,
        WrongCredentials,
        UnexpectedResponse
    }
}
