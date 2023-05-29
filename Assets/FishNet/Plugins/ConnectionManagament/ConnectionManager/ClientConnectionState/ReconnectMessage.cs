namespace FishNet.ConnectionManagement
{
    public readonly struct ReconnectMessage
    {
        public readonly int CurrentAttempt;
        public readonly int MaxAttempt;

        public ReconnectMessage(int currentAttempt, int maxAttempt)
        {
            CurrentAttempt = currentAttempt;
            MaxAttempt = maxAttempt;
        }
    }
}