using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmsTracker.Models;

public class TrackedItem
{
    [Key]
    public int Id { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public string Text { get; set; }
    
    [ForeignKey(nameof(OwnedByAccountId))]
    public Account OwnedByAccount { get; set; }
    public int OwnedByAccountId { get; set; }
    
    // Eventually support images/rich media?
}