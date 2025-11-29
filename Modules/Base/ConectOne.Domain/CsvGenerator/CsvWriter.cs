using ConectOne.Domain.Entities;

namespace ConectOne.Domain.CsvGenerator
{
    /// <summary>
    /// Provides functionality to write collections of objects to CSV (Comma-Separated Values) format using customizable
    /// output options.
    /// </summary>
    /// <remarks>CsvWriter<T> enables writing objects to CSV streams or files, supporting both full and
    /// partial property selection. The class requires that T implements methods for generating CSV representations and
    /// column headers, as defined by CsvableBase. This class is not thread-safe.</remarks>
    /// <typeparam name="T">The type of objects to write to CSV. Must inherit from CsvableBase to support CSV serialization.</typeparam>
    public class CsvWriter<T> where T : CsvableBase
    {
        /// <summary>
        /// Writes the specified collection of objects to the provided memory stream in CSV format.
        /// </summary>
        /// <remarks>The method writes column headers based on the first object's schema, followed by the
        /// CSV representation of each object. The caller is responsible for managing the lifetime of the provided
        /// memory stream. No data is written if the collection is empty.</remarks>
        /// <param name="objects">The collection of objects to write to the stream. Each object must support conversion to CSV format.</param>
        /// <param name="memoryStream">The memory stream to which the CSV data will be written. Must be writable and remain open for the duration
        /// of the operation.</param>
        public void Write(IEnumerable<T> objects, MemoryStream memoryStream)
        {
            try
            {
                var objs = objects as List<T> ?? objects.ToList();
                if (objs.Any())
                {
                    var sw = new StreamWriter(memoryStream);
                    var headersAdded = false;

                    foreach (var obj in objs)
                    {
                        if (!headersAdded)
                        {
                            sw.WriteLine(obj.GetColumnHeaders());
                            headersAdded = true;
                        }

                        sw.WriteLine(obj.ToCsv());
                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// Writes the specified collection of objects to a CSV file at the given destination, including only the
        /// specified properties.
        /// </summary>
        /// <remarks>The method writes one line per object to the specified file, using the object's ToCsv
        /// method to format the output. No data is written if the collection is empty.</remarks>
        /// <param name="objects">The collection of objects to write to the CSV file. Only objects in this collection will be processed.</param>
        /// <param name="destination">The file path where the CSV output will be written. If the file exists, it will be overwritten.</param>
        /// <param name="propertyNames">An array of property names to include in the CSV output. Only these properties will be written for each
        /// object.</param>
        /// <param name="isIgnore">A value indicating whether to ignore properties that are not found on the object. If set to <see
        /// langword="true"/>, missing properties will be ignored; otherwise, an error may occur.</param>
        public void Write(IEnumerable<T> objects, string destination, string[] propertyNames, bool isIgnore)
        {
            var objs = objects as List<T> ?? objects.ToList();
            if (objs.Any())
            {
                using (var sw = new StreamWriter(destination))
                {
                    foreach (var obj in objs)
                    {
                        sw.WriteLine(obj.ToCsv(propertyNames, isIgnore));
                    }
                }
            }
        }

        /// <summary>
        /// Writes the specified collection of objects to a CSV file at the given destination, including only the
        /// selected properties and optionally ignoring empty values.
        /// </summary>
        /// <remarks>The method writes one line per object to the specified file, using the object's ToCsv
        /// method to generate each line. The file is created or overwritten at the specified destination.</remarks>
        /// <param name="objects">The collection of objects to write to the CSV file. Each object must support conversion to CSV format via
        /// the ToCsv method.</param>
        /// <param name="destination">The file path where the CSV output will be written. If the file exists, it will be overwritten.</param>
        /// <param name="propertyIndexes">An array of property indexes indicating which properties of each object to include in the CSV output. The
        /// order of indexes determines the column order in the file.</param>
        /// <param name="isIgnore">A value indicating whether to ignore empty or null property values when writing to the CSV file. If set to
        /// <see langword="true"/>, such values will be omitted.</param>
        public void Write(IEnumerable<T> objects, string destination, int[] propertyIndexes, bool isIgnore)
        {
            var objs = objects as List<T> ?? objects.ToList();
            if (objs.Any())
            {
                using (var sw = new StreamWriter(destination))
                {
                    foreach (var obj in objs)
                    {
                        sw.WriteLine(obj.ToCsv(propertyIndexes, isIgnore));
                    }
                }
            }
        }
    }
}
