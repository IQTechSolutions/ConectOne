using ConectOne.Domain.Entities;
using MessagingModule.Domain.DataTransferObjects;

namespace MessagingModule.Domain.Entities
{
    public class NotificationSubscription : EntityBase<string>
    {
        #region Constructors

        public NotificationSubscription()
        {

        }

        public NotificationSubscription(NotificationSubscriptionDto dto)
        {
            Id = dto.Id;
            UserId = dto.UserId;
            Url = dto.Url;
            P256dh = dto.P256dh;
            Auth = dto.Auth;
        }

        #endregion

        public string? UserId { get; set; }

        public string? Url { get; set; }

        public string? P256dh { get; set; }

        public string? Auth { get; set; }
    }
}
