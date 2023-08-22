
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("UserLock")]
    // [Index("ApplicationUserLoginDetail", Name = "iApplicationUserLoginDetail_UserLock")]
    // [Index("InitiatingUser", Name = "iInitiatingUser_UserLock")]
    public partial class UserLock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LogDate { get; set; }
        public Guid? ApplicationUserLoginDetail { get; set; }
        public int? LockType { get; set; }
        public bool? WebPortalInitiated { get; set; }
        public Guid? InitiatingUser { get; set; }

        [ForeignKey("InitiatingUser")]
        // [InverseProperty("UserLocks")]
        public virtual ApplicationUser InitiatingUserNavigation { get; set; }
    }
}