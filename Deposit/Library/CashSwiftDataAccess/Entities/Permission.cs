
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("Permission")]
    public partial class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid role_id { get; set; }
        public Guid activity_id { get; set; }
        public bool standalone_allowed { get; set; }
        public bool standalone_authentication_required { get; set; }
        public bool standalone_can_Authenticate { get; set; }

        [ForeignKey("activity_id")]
        // [InverseProperty("Permissions")]
        public virtual Activity activity { get; set; }
        [ForeignKey("role_id")]
        // [InverseProperty("Permissions")]
        public virtual Role role { get; set; }
    }
}