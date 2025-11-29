using ConectOne.Domain.CsvGenerator;
using ConectOne.Domain.Entities;

/// <summary>
/// Provides functionality to read CSV files and convert them into a collection of objects.
/// </summary>
/// <typeparam name="T">The type of objects to read from the CSV file. Must inherit from <see cref="CsvableBase"/> and have a parameterless constructor.</typeparam>
public class CsvReader<T> where T : CsvableBase, new()
{
    /// <summary>
    /// Reads the CSV file from the specified file path and converts each row into an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="filePath">The path to the CSV file to read.</param>
    /// <param name="configuration">The configuration settings for reading the CSV file.</param>
    /// <returns>A collection of objects of type <typeparamref name="T"/> read from the CSV file.</returns>
    public IEnumerable<T> Read(string filePath, CsvConfiguration configuration)
    {
        var objects = new List<T>();

        try
        {
            using var sr = new StreamReader(filePath);
            string? line;
            bool headersRead = !configuration.HasHeaderRecord;

            while ((line = sr.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue; // Skip empty lines
                }

                if (headersRead)
                {
                    var obj = new T();
                    var propertyValues = line.Split(configuration.Delimiter);

                    // Ensure the number of fields matches expectations
                    try
                    {
                        obj.FromCsv(propertyValues);
                        objects.Add(obj);
                    }
                    catch (Exception ex)
                    {
                        // Log or handle row-level exceptions
                        Console.WriteLine($"Error parsing row: {line}. Exception: {ex.Message}");
                    }
                }
                else
                {
                    headersRead = true; // Skip the header row
                }
            }
        }
        catch (FileNotFoundException)
        {
            throw new ArgumentException($"The file '{filePath}' was not found.");
        }
        catch (UnauthorizedAccessException)
        {
            throw new InvalidOperationException($"Access to the file '{filePath}' is denied.");
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"An I/O error occurred while reading the file: {ex.Message}");
        }

        return objects;
    }
}
