using ConectOne.Domain.RequestFeatures;

namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a set of parameters used to fetch and filter school events with paging and sorting capabilities.
    /// This DTO extends <see cref="RequestParameters"/> and adds event-specific filters such as category,
    /// parent/learner filtering, and optional date-based queries.
    ///
    /// <para>
    /// These parameters can be supplied to a back-end endpoint to limit the returned events. For example, 
    /// when requesting a paginated list of school events, these parameters help control:
    /// <list type="bullet">
    ///   <item><description>Which page of events to return.</description></item>
    ///   <item><description>How many events per page.</description></item>
    ///   <item><description>Sorting the results based on a field (like event date or name).</description></item>
    ///   <item><description>Filtering events by related categories, parents, learners, or even a start date.</description></item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// Usage scenarios might include:
    /// <list type="bullet">
    ///   <item><description>Requesting only future events: set <c>StartDate</c> to a date, filtering out past events.</description></item>
    ///   <item><description>Fetching events for a specific parent or learner: set <c>ParentId</c> or <c>LearnerId</c>.</description></item>
    ///   <item><description>Searching events by a keyword: provide a <c>SearchText</c> to find matches in event titles or descriptions.</description></item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// The back-end can then read these parameters and build a filtered, sorted, and paged query 
    /// over the events data set.
    /// </para>
    /// </summary>
    public class SchoolEventPageParameters : RequestParameters
    {

        /// <summary>
        /// Indicates whether to include archived (past) events or not. 
        /// If <c>true</c>, the backend might return older, completed events.
        /// If <c>false</c>, it may return only current/future events.
        /// This depends on the backend's interpretation of archived status.
        /// </summary>
        public bool Archived { get; set; } = false;

        /// <summary>
        /// An optional category ID. If provided, only events belonging to this category
        /// should be returned. Useful for showing events by type or grouping.
        /// </summary>
        public string? CategoryId { get; set; }

        /// <summary>
        /// An optional parent ID. If provided, only events relevant to this parent 
        /// or their children should be returned. Useful for parent dashboards 
        /// or personalized event calendars.
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// Email address for user email
        /// </summary>
        public string? UserEmailAddress { get; set; }

        /// <summary>
        /// An optional learner ID. If provided, only events involving this particular
        /// learner should be returned. Useful when building a learner-specific event schedule.
        /// </summary>
        public string? LearnerId { get; set; }

        /// <summary>
        /// An optional teacher ID. If provided, only events involving this particular
        /// learner should be returned. Useful when building a teacher-specific event schedule.
        /// </summary>
        public string? TeacherId { get; set; }

        /// <summary>
        /// An optional <c>StartDate</c> as a string. The backend is expected to parse it
        /// (likely as a date) and use it to filter events. For example, returning only
        /// events starting on or after this date.
        /// </summary>
        public string? StartDate { get; set; }

        /// <summary>
        /// Flag to indicate if this is an active event
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is featured.
        /// </summary>
        public bool? Featured { get; set; }
    }
}
