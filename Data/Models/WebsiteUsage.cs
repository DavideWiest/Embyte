using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Embyte.Data.Models;

public class WebsiteUsage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(450)]
    public string Url { get; set; }

    [Required]
    public int RequestCount { get; set; }

    public WebsiteUsage()
    {
        RequestCount = 1;
    }
}
