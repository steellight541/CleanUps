namespace CleanUps.Shared.DTOs.Enums
{
    /// <summary>
    /// Represents the current status of an Event.
    /// Used to track the status of an event from the perspective of the user.
    /// </summary>
    public enum StatusDTO
    {
        /// <summary>
        /// The event is scheduled but has not yet started.
        /// </summary>
        Upcoming = 1,
        
        /// <summary>
        /// The event is currently taking place.
        /// </summary>
        Ongoing = 2,
        
        /// <summary>
        /// The event has finished successfully.
        /// </summary>
        Completed = 4,
        
        /// <summary>
        /// The event has been cancelled and will not take place.
        /// </summary>
        Canceled = 5,
    }
}
