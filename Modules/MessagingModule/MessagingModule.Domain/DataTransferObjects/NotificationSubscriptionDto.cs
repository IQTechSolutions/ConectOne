namespace MessagingModule.Domain.DataTransferObjects
{
    public record NotificationSubscriptionDto
    {
        public string Id { get; init; }
        public string? UserId { get; init; }

        public string? Url { get; init; }

        public string? P256dh { get; init; }

        public string? Auth { get; init; }
    }
}
