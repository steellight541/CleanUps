namespace CleanUps.Shared.DTOs.Flags
{
    /// <summary>
    /// Serves as the base record for all data transfer objects (DTOs) in the CleanUps application.
    /// This class allows using generic constraints on records.
    /// </summary>
    public abstract record RecordFlag;
    //used to tell generics that record classes are in fact a record and not a class, since in c# you cannot tell "where T : record"
    // so you have to make a workaround where you declare an abstract dummy class that other records can inherent from, so that the following works "where T : RecordFlag"

}
