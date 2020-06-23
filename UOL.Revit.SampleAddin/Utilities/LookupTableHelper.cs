using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UOL.Models;
using UOL.SDK.Converters;

namespace UOL.Revit.SampleAddin.Utilities
{
    internal class LookupTableHelper
    {
        private readonly IDataTableConverter dataTableConverter;

        public LookupTableHelper(IDataTableConverter dataTableConverter)
        {
            this.dataTableConverter = dataTableConverter;
        }

        public string CreateLookupTable(string name, IEnumerable<string> headers, IEnumerable<CADProduct> cadProducts)
        {
            var datatable = dataTableConverter.Create(headers, cadProducts, true, true);

            var stream = dataTableConverter.WriteToCsvStream(datatable);

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "temp");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileName = Path.Combine(path, $"{name}.csv");
            using (var fileStream = File.Create(fileName))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }

            return fileName;
        }
    }
}
