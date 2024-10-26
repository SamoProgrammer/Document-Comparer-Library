using DocumentComparisonLib;
using FluentAssertions;
using Xunit;
using System.IO;

namespace DocumentComparisonLib.Tests.ComparerTests
{
    public class DocumentComparerTests
    {
        private readonly DocumentComparer _comparer;

        public DocumentComparerTests()
        {
            _comparer = new DocumentComparer();
        }

        [Fact]
        public void CompareDocuments_ShouldReturnJson_WhenFormatIsJson()
        {
            // Arrange
            string file1Path = "test1.txt";
            string file2Path = "test2.txt";

            File.WriteAllText(file1Path, "This is a test document.");
            File.WriteAllText(file2Path, "This is another test document.");

            // Act
            var result = _comparer.CompareDocuments(file1Path, file2Path, "json");

            // Assert
            result.Should().Contain("\"differences\":");
            result.Should().Contain("\"startIndex\":");
            result.Should().Contain("\"endIndex\":");

            // Cleanup
            File.Delete(file1Path);
            File.Delete(file2Path);
        }

        [Fact]
        public void CompareDocuments_ShouldReturnXml_WhenFormatIsXml()
        {
            // Arrange
            string file1Path = "test1.txt";
            string file2Path = "test2.txt";

            File.WriteAllText(file1Path, "This is a test document.");
            File.WriteAllText(file2Path, "This is another test document.");

            // Act
            var result = _comparer.CompareDocuments(file1Path, file2Path, "xml");

            // Assert
            result.Should().Contain("<differences>");
            result.Should().Contain("<startIndex>");
            result.Should().Contain("<endIndex>");

            // Cleanup
            File.Delete(file1Path);
            File.Delete(file2Path);
        }

        [Fact]
        public void CompareDocuments_ShouldReturnHtml_WhenFormatIsHtml()
        {
            // Arrange
            string file1Path = "test1.txt";
            string file2Path = "test2.txt";

            File.WriteAllText(file1Path, "This is a test document.");
            File.WriteAllText(file2Path, "This is another test document.");

            // Act
            var result = _comparer.CompareDocuments(file1Path, file2Path, "html");

            // Assert
            result.Should().Contain("<html>");
            result.Should().Contain("<body>");
            result.Should().Contain("</html>");

            // Cleanup
            File.Delete(file1Path);
            File.Delete(file2Path);
        }
    }
}
