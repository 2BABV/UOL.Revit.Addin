namespace UOL.Revit.SampleAddin.Models
{
    /// <summary>
    /// Represents a parameter info class.
    /// </summary>
    public class ParameterInfo
    {
        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>Type: <see cref="string"/><br/>
        /// The parameter name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parameter type.
        /// </summary>
        /// <value>Type: <see cref="ParameterType"/><br/>
        /// The parameter type.
        /// </value>
        public ParameterType Type { get; set; }

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        /// <value>Type: <see cref="object"/><br/>
        /// The parameter value.
        /// </value>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the parameter value type of the parameter.
        /// </summary>
        /// <value>Type: <see cref="ParameterValueType"/><br/>
        /// The parameter value type.
        /// </value>
        public ParameterValueType ValueType { get; set; }
    }
}
