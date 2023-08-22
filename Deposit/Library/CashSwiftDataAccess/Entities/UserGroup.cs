using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("UserGroup")]
    // [Index("parent_group", Name = "iparent_group_UserGroup")]
    public partial class UserGroup
    {
        public UserGroup()
        {
            ApplicationUsers = new HashSet<ApplicationUser>();
            Devices = new HashSet<Device>();
            Inverseparent_groupNavigation = new HashSet<UserGroup>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(512)]
        public string description { get; set; }
        public int? parent_group { get; set; }

        [ForeignKey("parent_group")]
        // [InverseProperty("Inverseparent_groupNavigation")]
        public virtual UserGroup parent_groupNavigation { get; set; }
        // [InverseProperty("user_groupNavigation")]
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        // [InverseProperty("user_groupNavigation")]
        public virtual ICollection<Device> Devices { get; set; }
        // [InverseProperty("parent_groupNavigation")]
        public virtual ICollection<UserGroup> Inverseparent_groupNavigation { get; set; }
    }
}