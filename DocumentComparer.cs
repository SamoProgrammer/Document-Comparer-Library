using DocumentFormat.OpenXml.Packaging;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace DocumentComparisonLib
{
    public class DocumentComparer
    {
        // Main method to compare two documents and return structured output (JSON, XML, or HTML).
        public string CompareDocuments(string filePath1, string filePath2, string format = "json")
        {
            string[] doc1Lines = ExtractTextFromFile(filePath1);
            string[] doc2Lines = ExtractTextFromFile(filePath2);

            var differences = CompareDocuments(doc1Lines, doc2Lines);

            return format.ToLower() switch
            {
                "xml" => ResponseGenerator.GenerateXmlResponse(differences),
                "html" => ResponseGenerator.GenerateHtmlResponse(differences),
                _ => ResponseGenerator.GenerateJsonResponse(differences),
            };
        }

        // Method to compare two documents line by line.
        private List<Difference> CompareDocuments(string[] doc1Lines, string[] doc2Lines)
        {
            var differences = new List<Difference>();

            int lineCount = Math.Max(doc1Lines.Length, doc2Lines.Length);

            for (int i = 0; i < lineCount; i++)
            {
                if (i >= doc1Lines.Length) // Line exists only in doc2.
                {
                    differences.Add(new Difference
                    {
                        LineNumber = i + 1,
                        ChangeType = "Addition",
                        ContentInDoc1 = string.Empty, // No content in doc1.
                        ContentInDoc2 = doc2Lines[i], // Full content in doc2.
                        StartIndex = 0,
                        EndIndex = doc2Lines[i].Length - 1
                    });
                }
                else if (i >= doc2Lines.Length) // Line exists only in doc1.
                {
                    differences.Add(new Difference
                    {
                        LineNumber = i + 1,
                        ChangeType = "Deletion",
                        ContentInDoc1 = doc1Lines[i], // Full content in doc1.
                        ContentInDoc2 = string.Empty, // No content in doc2.
                        StartIndex = 0,
                        EndIndex = doc1Lines[i].Length - 1
                    });
                }
                else if (doc1Lines[i] != doc2Lines[i]) // Lines differ.
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
            int startIndex = -1;
            int endIndex = -1;

            for (int i = 0; i < maxLength; i++)
            {
                char char1 = i < line1.Length ? line1[i] : '\0';
                char char2 = i < line2.Length ? line2[i] : '\0';

                if (char1 != char2)
                {
                    if (startIndex == -1)
                    {
                        startIndex = i; // Mark the start index when the difference starts.
                    }
                    endIndex = i; // Update the end index as the difference continues.

                    differences.Add(new Difference
                    {
                        LineNumber = lineNumber,
                        ChangeType = char1 == '\0' ? "Addition" : char2 == '\0' ? "Deletion" : "Modification",
                        ContentInDoc1 = char1 == '\0' ? string.Empty : char1.ToString(),
                        ContentInDoc2 = char2 == '\0' ? string.Empty : char2.ToString(),
                        StartIndex = startIndex,
                        EndIndex = endIndex
                    });
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


    }
}
