using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UOL.Models;

namespace UOL.Revit.SampleAddin
{
    internal class ElementConverter
    {
        /// <summary>
        /// Converts Revit elements to <see cref="CADProduct"/>.
        /// </summary>
        /// <param name="elements">The Revit elements to convert.</param>
        /// <param name="progressBar">The progress bar uesd for showing progress.</param>
        /// <returns>A list with <see cref="CADProduct"/>.</returns>
        public IEnumerable<CADProduct> ConvertToCADProducts(IEnumerable<Element> elements, ProgressBar progressBar)
        {
            var cadProducts = new List<CADProduct>();
            var i = 1;
            progressBar.Maximum = 100;
            double valueStep = 100 / elements.Count();
            progressBar.Value = (int)Math.Round(i * valueStep);
            foreach (var element in elements)
            {
                var cadProduct = ConvertToCADProduct(element);

                if (cadProduct != null)
                {
                    cadProducts.Add(cadProduct);
                }

                progressBar.Value = (int)Math.Round(i * valueStep);
                i++;
                Application.DoEvents();
            }

            return cadProducts;
        }

        /// <summary>
        /// Converts a Revit element to a <see cref="CADProduct"/>.
        /// </summary>
        /// <param name="elements">The Revit elements to convert.</param>
        /// <param name="progressBar">The progress bar uesd for showing progress.</param>
        /// <returns>A <see cref="CADProduct"/>.</returns>
        public CADProduct ConvertToCADProduct(Element element)
        {
            if (element is FamilyInstance familyInstance && !string.IsNullOrEmpty(element.Document.PathName))
            {
                var cadFilterResultString = File.ReadAllText(element.Document.PathName.Replace(".rvt", $"_{familyInstance.Symbol.Id.IntegerValue.ToString()}.met"));
                if (!string.IsNullOrEmpty(cadFilterResultString))
                {
                    var cadFilterResult = JsonConvert.DeserializeObject<CADFilterResult>(cadFilterResultString);

                    var cadProduct = cadFilterResult.CADProducts.FirstOrDefault();

                    UOLAddInUtilities.UpdatePropertiesWithValuesFromFamily(familyInstance.Symbol, cadProduct.Properties, false, true);
                    UOLAddInUtilities.UpdatePropertiesWithValuesFromFamily(familyInstance, cadProduct.Properties, false, true);

                    cadProduct.Id = familyInstance.Id.ToString();
                    cadProduct.ProductId = cadProduct.ProductId ?? 0;
                    return cadProduct;
                }
            }

            return null;
        }
    }
}
