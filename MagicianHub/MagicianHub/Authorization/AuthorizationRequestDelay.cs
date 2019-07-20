namespace MagicianHub.Authorization
{
    public static class AuthorizationRequestDelay
    {
        public static int DelayMsModifier;

        public static int CalculateDelay()
        {
            return DelayMsModifier;
        }

        public static void RecalculateRequests()
        {
            if (DelayMsModifier == 0)
            {
                DelayMsModifier += 100;
                return;
            }
            if (DelayMsModifier >= 1000)
            {
                DelayMsModifier += DelayMsModifier / 2;
                return;
            }
            if (DelayMsModifier >= 10000)
            {
                DelayMsModifier += DelayMsModifier / 4;
                return;
            }
            DelayMsModifier += DelayMsModifier;
        }
    }
}
