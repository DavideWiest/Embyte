using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Embyte.Data.Models;

public class WebsiteUsage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(450)]
    [Index(IsUnique = true)]
    public string Url { get; set; }

    [Required]
    public int RequestCount { get; set; }

    public WebsiteUsage()
    {
        RequestCount = 1;
    }
}
