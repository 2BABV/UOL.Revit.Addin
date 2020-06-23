namespace UOL.Revit.SampleAddin.Models
{
    /// <summary>
    /// Defines the parameter type.
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// The Type <see cref="ParameterType"/> is used if no ParameterType is needed.
        /// </summary>
        None = 0,

        /// <summary>
        /// The Type <see cref="ParameterType"/> is used for Type parameters.
        /// </summary>
        Type = 1,

        /// <summary>
        /// The Instance <see cref="ParameterType"/> is used for Instance parameters.
        /// </summary>
        Instance = 2
    }
}
