using System.ComponentModel.DataAnnotations;

namespace apbd_cw06.Models.DTOs;

public class EditAnimalReq
{
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    [Required]
    public string Category { get; set; }
    [Required]
    public string Area { get; set; }
}