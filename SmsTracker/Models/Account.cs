using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SmsTracker.Models;

public class Account
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string AccountName { get; set; }

    public virtual ICollection<Number> AssociatedNumbers { get; set; } = new List<Number>();
    
    [ForeignKey(nameof(OwnedByUserId))]
    public IdentityUser OwnedByUser { get; set; }
    public string OwnedByUserId { get; set; }
    
    /// <summary>
    /// Determines if this is the primary account. Can only have one per user.
    /// </summary>
    public bool IsPrimary { get; set; }
    
    [MaxLength(5)]
    public string? Prefix { get; set; }
}