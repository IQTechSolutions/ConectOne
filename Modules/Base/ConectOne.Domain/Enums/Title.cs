namespace ConectOne.Domain.Enums
{
    /// <summary>
    /// Specifies common personal titles used to address individuals.
    /// </summary>
    /// <remarks>This enumeration includes a selection of honorifics such as 'Mr', 'Mrs', 'Ms', and
    /// professional titles like 'Dr' and 'Prof'. Use these values to represent or request a person's preferred form of
    /// address in user interfaces, forms, or communications.</remarks>
    public enum Title
    {
        Mrs = 0,
        Mr = 1,
        Ms = 2,
        Miss = 3,
        Prof = 4,
        Dr = 5,
        Pastor = 6,
        Me = 7
    }
}