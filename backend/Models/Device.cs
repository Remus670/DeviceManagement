using System.ComponentModel.DataAnnotations;

namespace DeviceManagementApi.Models
{
    public class Device
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Manufacturer { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string OperatingSystem { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string OsVersion { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Processor { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string RamAmount { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}