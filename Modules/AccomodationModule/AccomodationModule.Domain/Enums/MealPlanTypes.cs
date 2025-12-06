using System.ComponentModel;

namespace AccomodationModule.Domain.Enums
{
    public enum MealPlanTypes
	{
		[Description("None")] None = 0,
        [Description("B&B")] BedAndBreakfast = 1,
        [Description("Self Catering")] SelfCatering = 2,
        [Description("B&B with Dinner")] DinnerBedAndBreakfast = 3,
        [Description("Full Board")] FullBoard = 4,
        [Description("Camp Site")] CampSite = 6,
        [Description("All Inclusive")] AllInclusive = 7
	}
}
