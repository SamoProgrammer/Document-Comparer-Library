    public class Difference
    {
        public int LineNumber { get; set; }
        public string ChangeType { get; set; }
        public string ContentInDoc1 { get; set; }  // Content in document 1 (empty if not applicable).
        public string ContentInDoc2 { get; set; }  // Content in document 2 (empty if not applicable).
        public int StartIndex { get; set; }        // Start index of the change.
        public int EndIndex { get; set; }          // End index of the change.
    }