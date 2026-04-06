using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NOTATerminal.Models
{
    //public enum MacrosTypes
    //{
    //    RightClick = 1,
    //    Manually = 2,
    //    Combined = 3,
    //}
    public class Macros
    {
        [Key, Required]
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsArgsRequired { get; set; } = false;
        public string? Name { get; set; }
        public string? SourceCode { get; set; } = "";
        public string? Checksum { get; set; }
        public string? HotKey { get; set; }
        public string? MenuItemColor { get; set; }
        public string? MenuTextColor { get; set; }
        public byte[]? BinaryExecutable { get; set; }
        [NotMapped]
        public bool IsSaved { get; set; } = true; //false = temp, true = in DB
        public Macros(bool isSaved)
        {
            IsSaved = isSaved;
        }
        public Macros(bool isSaved, string code)
        {
            IsSaved = isSaved;
            SourceCode = code;
        }
        public Macros(int id, bool isActive, string name)
        {
            Id = id;
            IsActive = isActive;
            Name = name;
        }
    }
}