using Autodesk.Revit.DB;

namespace UOL.Revit.SampleAddin.Utilities
{
    /// <summary>
    /// Provides a family load option.
    /// </summary>
    internal class FamilyLoadOption : IFamilyLoadOptions
    {
        /// <summary>
        /// Returns <c>true</c> when family is found.
        /// </summary>
        /// <param name="familyInUse">Specifies whether family is in use.</param>
        /// <param name="overwriteParameterValues">Specifies output parameter which has always value <c>true</c>.</param>
        /// <returns><c>true</c>.</returns>
        public bool OnFamilyFound(
          bool familyInUse,
          out bool overwriteParameterValues)
        {
            overwriteParameterValues = true;
            return true;
        }

        /// <summary>
        /// Returns <c>true</c> when shared family is found.
        /// </summary>
        /// <param name="sharedFamily">Specifies a shared family.</param>
        /// <param name="familyInUse">Specifies whether shared family is in use.</param>
        /// <param name="source">Specifies output parameter indicating the family source.</param>
        /// <param name="overwriteParameterValues">Specifies output parameter indicating whether parameter values are overwritten.</param>
        /// <returns><c>true</c>.</returns>
        public bool OnSharedFamilyFound(
          Family sharedFamily,
          bool familyInUse,
          out FamilySource source,
          out bool overwriteParameterValues)
        {
            source = FamilySource.Family;
            overwriteParameterValues = true;
            return true;
        }
    }
}
