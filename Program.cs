using System;
using System.IO;
using Newtonsoft.Json.Linq;

class Program
{
    static void Main(string[] args)
    {
        string rootDirectory = @"C:\Projects"; // Change to your root directory

        // Find all appsettings.json files
        string[] jsonFiles = Directory.GetFiles(rootDirectory, "appsettings*.json", SearchOption.AllDirectories);

        foreach (string filePath in jsonFiles)
        {
            Console.WriteLine($"Processing: {filePath}");
            ClearJsonFile(filePath);
        }

        Console.WriteLine("All appsettings.json files have been processed.");
    }

    static void ClearJsonFile(string filePath)
    {
        try
        {
            // Read the JSON file
            string jsonContent = File.ReadAllText(filePath);

            // Parse the JSON into a JObject
            JObject jsonObject = JObject.Parse(jsonContent);

            // Clear all values
            ClearValues(jsonObject);

            // Write the updated JSON back to the file
            File.WriteAllText(filePath, jsonObject.ToString(Newtonsoft.Json.Formatting.Indented));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
        }
    }

    static void ClearValues(JToken token)
    {
        if (token is JObject obj)
        {
            foreach (var property in obj.Properties())
            {
                ClearValues(property.Value);
            }
        }
        else if (token is JArray array)
        {
            foreach (var item in array)
            {
                ClearValues(item);
            }
        }
        else if (token is JValue value)
        {
            if (value.Type == JTokenType.String)
            {
                value.Value = ""; // Set value to an empty string
            }
            else if (value.Type == JTokenType.Integer || value.Type == JTokenType.Float)
            {
                value.Value = 0; // Set value to 0
            }
            else if (value.Type == JTokenType.Boolean)
            {
                value.Value = false; // Set value to false
            }
            else if (value.Type == JTokenType.Date)
            {
                value.Value = DateTime.MinValue; // Set value to the minimum date value
            }
            else if (value.Type == JTokenType.Null)
            {
                // Do nothing, leave it as null
            }
        }
    }
}
