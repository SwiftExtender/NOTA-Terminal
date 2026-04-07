using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NOTATerminal.Models
{
    public class FavoriteCommandModel
    {
        [Key, Required]
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public string CommandLine { get; set; }
    }
}
