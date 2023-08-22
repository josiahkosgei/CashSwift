
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("PasswordHistory")]
    // [Index("User", Name = "iUser_PasswordHistory")]
    public partial class PasswordHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LogDate { get; set; }
        public Guid? User { get; set; }
        [StringLength(71)]
        public string Password { get; set; }

        [ForeignKey("User")]
        // [InverseProperty("PasswordHistories")]
        public virtual ApplicationUser UserNavigation { get; set; }
    }
}