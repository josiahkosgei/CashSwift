
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("Role")]
    public partial class Role
    {
        public Role()
        {
            AlertMessageRegistries = new HashSet<AlertMessageRegistry>();
            ApplicationUsers = new HashSet<ApplicationUser>();
            Permissions = new HashSet<Permission>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(512)]
        public string description { get; set; }

        // [InverseProperty("role")]
        public virtual ICollection<AlertMessageRegistry> AlertMessageRegistries { get; set; }
        // [InverseProperty("role")]
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        // [InverseProperty("role")]
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}