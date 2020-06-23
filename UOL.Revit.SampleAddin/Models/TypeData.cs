using System.Collections.Generic;

namespace UOL.Revit.SampleAddin.Models
{
    /// <summary>
    /// Provides a <see cref="TypeData"/> class.
    /// </summary>
    public class TypeData
    {
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>Type: <see cref="string"/><br/>
        /// The path.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the type name.
        /// </summary>
        /// <value>Type: <see cref="string"/><br/>
        /// The type name.
        /// </value>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets parameter info dictionary.
        /// </summary>
        /// <value>Type: <see cref="Dictionary{string, ParameterInfo}"/><br/>
        /// The parameters.
        /// </value>
        public Dictionary<string, ParameterInfo> Parameters { get; set; }
    }
}
