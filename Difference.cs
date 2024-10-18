namespace DocumentComparisonLib
{
    public class Difference
    {
        public int LineNumber { get; set; }
        public string ChangeType { get; set; }
        public string Content { get; set; }
    }
}
