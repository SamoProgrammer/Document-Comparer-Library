![Package Logo](https://github.com/SamoProgrammer/DocumentComparer/blob/main/DocumentComparerLogo.webp)

**DocumentComparer** is a lightweight and flexible .NET library designed to compare text-based files (including `.txt`, `.docx`, and `.pdf`) and return the differences in various formats such as JSON, XML, and HTML (with a GitHub-like diff view). 

---

## **Features**
- Compare text (`.txt`), Microsoft Word documents (`.docx`), and PDF files (`.pdf`).
- Get the differences between two documents, including:
  - Additions, deletions, and modifications.
  - Start and end indices of changes.
- Output formats:
  - **JSON**: Structured output for easy parsing.
  - **XML**: Structured and serialized comparison results.
  - **HTML**: GitHub-style visual representation of changes.
- Efficient line-by-line comparison with support for character-level differences.

---

## **Installation**

You can install the **DocumentComparer** package via NuGet Package Manager Console:

```bash
Install-Package DocumentComparer
```

Or via .NET CLI:

```bash
dotnet add package DocumentComparer
```

---

## **Usage**

### **1. Basic Document Comparison**

You can compare two files of different formats, such as `.txt`, `.docx`, or `.pdf`, and get the differences in JSON format:

```csharp
using DocumentComparisonLib;

var comparer = new DocumentComparer();
string result = comparer.CompareDocuments("path/to/file1.txt", "path/to/file2.docx", "json");

Console.WriteLine(result);
```

### **2. Output Options**

- **JSON Output** (default):
  ```csharp
  var result = comparer.CompareDocuments("path/to/file1.txt", "path/to/file2.pdf", "json");
  ```
  
- **XML Output**:
  ```csharp
  var result = comparer.CompareDocuments("path/to/file1.txt", "path/to/file2.pdf", "xml");
  ```
  
- **HTML Output** (GitHub-style diff):
  ```csharp
  var result = comparer.CompareDocuments("path/to/file1.txt", "path/to/file2.pdf", "html");
  ```


### **4. Supported Formats**
- **Text Files** (`.txt`)
- **Word Documents** (`.docx`)
- **PDF Documents** (`.pdf`)

---

## **Example Project**

Here’s an example of how you can use the library in a .NET console application:

```csharp
using DocumentComparisonLib;

class Program
{
    static void Main(string[] args)
    {
        var comparer = new DocumentComparer();

        // Comparing two documents with HTML output
        string result = comparer.CompareDocuments("document1.pdf", "document2.docx", "html");
        
        // Save the result to an HTML file
        File.WriteAllText("comparison_result.html", result);

        Console.WriteLine("Comparison completed and saved as comparison_result.html");
    }
}
```

---

## **Contributing**

Contributions are welcome! If you'd like to contribute to the project:

1. Fork the repository.
2. Create your feature branch (`git checkout -b feature/AmazingFeature`).
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`).
4. Push to the branch (`git push origin feature/AmazingFeature`).
5. Open a pull request.

---

## **License**

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

---

## **Contact**

If you have any questions or need help, feel free to contact me via GitHub or submit an issue in the repository.
