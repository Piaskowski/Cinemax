using System.ComponentModel.DataAnnotations;

namespace Cinemax.Client.Pages.Order
{
    public class OrderModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Range(typeof(bool), "true", "true")]
        public bool IsAcceptedTerms { get; set; }
    }
}
