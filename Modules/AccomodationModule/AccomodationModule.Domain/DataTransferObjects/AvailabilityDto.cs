using AccomodationModule.Domain.RequestFeatures;
using ProductsModule.Domain.DataTransferObjects;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the availability data transfer object.
    /// </summary>
    public record AvailabilityDto
    {
        /// <summary>
        /// Gets the ID of the partner room type.
        /// </summary>
        public int PartnerRoomTypeId { get; init; }

        /// <summary>
        /// Gets the date of availability.
        /// </summary>
        public DateTime Date { get; init; }

        /// <summary>
        /// Gets the list of room rates.
        /// </summary>
        public List<double> RoomRates { get; init; } = new();

        /// <summary>
        /// Gets the number of available rooms.
        /// </summary>
        public int NrRooms { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the availability is selected.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Calculates the pricing based on guest totals, markup, commission, child policies, and discount percentage.
        /// </summary>
        /// <param name="guestTotals">The totals of guests.</param>
        /// <param name="markup">The markup percentage.</param>
        /// <param name="commission">The commission percentage.</param>
        /// <param name="childPolicies">The list of child policy rules.</param>
        /// <param name="discountPercentage">The discount percentage.</param>
        /// <returns>The calculated pricing.</returns>
        public PricingDto Pricing(GuestTotals guestTotals, double markup, double commission, List<ChildPolicyRuleDto>? childPolicies = null, double discountPercentage = 0)
        {
            var guestCount = guestTotals.Adults + guestTotals.Kids;
            var price = GetRoomRate(guestCount);

            if (childPolicies is not null)
            {
                if (guestTotals.ChildAges.Any())
                {
                    foreach (var childAge in guestTotals.ChildAges)
                    {
                        foreach (var policy in childPolicies)
                        {
                            if (childAge >= policy.MinAge && childAge <= policy.MaxAge)
                            {
                                if (policy is { Allowed: true })
                                {
                                    switch (policy.Rule)
                                    {
                                        case "N":
                                            break;
                                        case "P":
                                            price = GetRoomRate(guestTotals.Adults) + policy.Ammount; 
                                            break;
                                        case "R":
                                            price = GetRoomRate(guestTotals.Adults) + (guestTotals.Kids * policy.Ammount);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return new PricingDto();
        }

        /// <summary>
        /// Gets the room rate based on the total number of guests.
        /// </summary>
        /// <param name="guestTotal">The total number of guests.</param>
        /// <returns>The room rate.</returns>
        private double GetRoomRate(int guestTotal)
        {
            var rateCount = RoomRates.Count;
            switch (guestTotal)
            {
                case 0:
                    return 0.00;
                case 1:
                    return RoomRates[0];
            }

            return guestTotal >= rateCount ? RoomRates[rateCount - 1] : RoomRates.Last();
        }
    }
}
