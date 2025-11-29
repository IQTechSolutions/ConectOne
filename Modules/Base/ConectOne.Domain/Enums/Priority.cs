namespace ConectOne.Domain.Enums
{
    /// <summary>
    /// Specifies the relative importance or urgency of an item or operation.
    /// </summary>
    /// <remarks>Use this enumeration to indicate how tasks, requests, or items should be prioritized. The
    /// values range from highest to lowest priority, with 'Optional' representing items that are not
    /// required.</remarks>
    public enum Priority
    {
        High = 0,
        Medium = 1,
        Low = 2,
        Optional = 3
    }
}
