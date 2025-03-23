namespace CleanUps.BusinessDomain.Models.Flags
{
    /// <summary>
    /// Serves as the base flag for all model classes in the CleanUps application.
    /// All domain models that represent database entities should inherit from this class.
    /// </summary>
    public abstract class ModelFlag;
    //Used to tell generics that this is in fact a model class from the db e.g. "Where T : ModelFlag",
    //used to insure only valid types are passed to the relevant generic interfaces in the BusinessLogic.Services
}
