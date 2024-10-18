using DocumentFormat.OpenXml.Packaging;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Text.Json;
using System.Xml.Serialization;

namespace DocumentComparisonLib;

public class DocumentComparer
{
    // Main method to compare two documents and return structured output (JSON or XML).
    public string CompareDocuments(string filePath1, string filePath2, string format = "json")
    {
        string[] doc1Lines = ExtractTextFromFile(filePath1);
        string[] doc2Lines = ExtractTextFromFile(filePath2);

        var differences = CompareDocuments(doc1Lines, doc2Lines);

        return format.ToLower() == "xml" ? GenerateXmlResponse(differences) : GenerateJsonResponse(differences);
    }

    // Method to compare two documents line by line
    private List<Difference> CompareDocuments(string[] doc1Lines, string[] doc2Lines)
    {
        var differences = new List<Difference>();

        int lineCount = Math.Max(doc1Lines.Length, doc2Lines.Length);

        for (int i = 0; i < lineCount; i++)
        {
            if (i >= doc1Lines.Length) // Line exists only in doc2
            {
                differences.Add(new Difference
                {
                    LineNumber = i + 1,
                    ChangeType = "Addition",
                    Content = doc2Lines[i]
                });
            }
            else if (i >= doc2Lines.Length) // Line exists only in doc1
            {
                differences.Add(new Difference
                {
                    LineNumber = i + 1,
                    ChangeType = "Deletion",
                    Content = doc1Lines[i]
                });
            }
            else if (doc1Lines[i] != doc2Lines[i]) // Lines differ
            {
                differences.AddRange(CompareLineCharacters(i + 1, doc1Lines[i], doc2Lines[i]));
            }
        }

        return differences;
    }

    // Helper method for character-by-character comparison.
    private List<Difference> CompareLineCharacters(int lineNumber, string line1, string line2)
    {
        var differences = new List<Difference>();

        int maxLength = Math.Max(line1.Length, line2.Length);
        for (int i = 0; i < maxLength; i++)
        {
            char char1 = i < line1.Length ? line1[i] : '\0';
            char char2 = i < line2.Length ? line2[i] : '\0';

            if (char1 != char2)
            {
                if (char1 == '\0') // Character only in line2 (addition)
                {
                    differences.Add(new Difference
                    {
                        LineNumber = lineNumber,
                        ChangeType = "Character Addition",
                        Content = $"{char2} at position {i + 1}"
                    });
                }
                else if (char2 == '\0') // Character only in line1 (deletion)
                {
                    differences.Add(new Difference
                    {
                        LineNumber = lineNumber,
                        ChangeType = "Character Deletion",
                        Content = $"{char1} at position {i + 1}"
                    });
                }
                else // Character change
                {
                    differences.Add(new Difference
                    {
                        LineNumber = lineNumber,
                        ChangeType = "Character Change",
                        Content = $"{char1} to {char2} at position {i + 1}"
                    });
                }
            }
        }

        return differences;
    }

    // Method to extract text from a file based on its extension (txt, docx, or pdf).
    private string[] ExtractTextFromFile(string filePath)
    {
        string fileExtension = Path.GetExtension(filePath).ToLower();

        switch (fileExtension)
        {
            case ".txt":
                return File.ReadAllLines(filePath);

            case ".docx":
                return ExtractTextFromDocx(filePath).Split(Environment.NewLine);

            case ".pdf":
                return ExtractTextFromPdf(filePath).Split(Environment.NewLine);

            default:
                throw new NotSupportedException("File format not supported.");
        }
    }

    // Extract text from a DOCX file using Open XML SDK.
    private string ExtractTextFromDocx(string filePath)
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
        {
            return wordDoc.MainDocumentPart.Document.Body.InnerText;
        }
    }

    // Extract text from a PDF file using iTextSharp.
    private string ExtractTextFromPdf(string filePath)
    {
        using (PdfReader pdfReader = new PdfReader(filePath))
        using (PdfDocument pdfDoc = new PdfDocument(pdfReader))
        {
            string text = "";
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                text += PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)) + Environment.NewLine;
            }
            return text;
        }
    }

    // Method to generate the JSON response.
    private string GenerateJsonResponse(List<Difference> differences)
    {
        return JsonSerializer.Serialize(differences, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    // Method to generate the XML response.
    private string GenerateXmlResponse(List<Difference> differences)
    {
        var serializer = new XmlSerializer(typeof(List<Difference>));
        using (var stringWriter = new StringWriter())
        {
            serializer.Serialize(stringWriter, differences);
            return stringWriter.ToString();
        }
    }
}


