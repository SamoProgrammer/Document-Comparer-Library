using System.Text.Json;
using System.Xml.Serialization;
using System.Text;

public static class ResponseGenerator
{
    // Method to generate the JSON response.
    public static string GenerateJsonResponse(List<Difference> differences)
    {
        return JsonSerializer.Serialize(differences, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    // Method to generate the XML response.
    public static string GenerateXmlResponse(List<Difference> differences)
    {
        var serializer = new XmlSerializer(typeof(List<Difference>));
        using (var stringWriter = new StringWriter())
        {
            serializer.Serialize(stringWriter, differences);
            return stringWriter.ToString();
        }
    }

    // Method to generate the HTML response styled like GitHub's diff view.
    public static string GenerateHtmlResponse(List<Difference> differences)
    {
        StringBuilder html = new StringBuilder();

        // Start of the HTML
        html.AppendLine("<html><head><style>");
        html.AppendLine("body { font-family: Arial, sans-serif; }");
        html.AppendLine(".line { padding: 5px; }");
        html.AppendLine(".addition { background-color: #d4fcbc; color: #096900; }");
        html.AppendLine(".deletion { background-color: #fbb6b6; color: #9b1c1c; }");
        html.AppendLine(".modification { background-color: #eaf5ff; color: #0366d6; }");
        html.AppendLine("</style></head><body>");

        html.AppendLine("<h2>Document Differences</h2>");

        // Iterate through the differences and build HTML for each.
        foreach (var diff in differences)
        {
            string cssClass = diff.ChangeType.ToLower() switch
            {
                "addition" => "addition",
                "deletion" => "deletion",
                _ => "modification"
            };

            html.AppendLine($"<div class='line {cssClass}'>");
            html.AppendLine($"<strong>Line {diff.LineNumber} ({diff.ChangeType})</strong><br>");

            if (diff.ChangeType == "Addition")
            {
                html.AppendLine($"<span><strong>Added:</strong> {EscapeHtml(diff.ContentInDoc2)}</span><br>");
            }
            else if (diff.ChangeType == "Deletion")
            {
                html.AppendLine($"<span><strong>Deleted:</strong> {EscapeHtml(diff.ContentInDoc1)}</span><br>");
            }
            else // Modification
            {
                html.AppendLine($"<span><strong>Old:</strong> {EscapeHtml(diff.ContentInDoc1)}</span><br>");
                html.AppendLine($"<span><strong>New:</strong> {EscapeHtml(diff.ContentInDoc2)}</span><br>");
            }

            html.AppendLine("</div>");
        }

        // End of the HTML
        html.AppendLine("</body></html>");

        return html.ToString();
    }

    // Helper method to escape HTML special characters.
    public static string EscapeHtml(string input)
    {
        return input.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    }
}