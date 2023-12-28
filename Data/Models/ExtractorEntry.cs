using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ExtractorEntry
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime Time { get; set; }

    public TimeSpan DeltaToPrevious { get; set; }

    public string Url { get; set; } = string.Empty;

    public bool DataChanged = false;

    public ExtractorEntry() { }

    public ExtractorEntry(TimeSpan deltaToPrevious, string url, bool dataChanged)
    {
        Time = DateTime.Now;
        DeltaToPrevious = deltaToPrevious;
        Url = url;
        DataChanged = dataChanged;
    }
}
