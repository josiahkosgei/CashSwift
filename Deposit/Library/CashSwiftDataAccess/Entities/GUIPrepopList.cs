
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("GUIPrepopList")]
    public partial class GUIPrepopList
    {
        public GUIPrepopList()
        {
            GUIPrepopListItems = new HashSet<GUIPrepopListItem>();
            GuiScreenListScreens = new HashSet<GuiScreenListScreen>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }
        [StringLength(100)]
        public string description { get; set; }
        [Required]
        public bool? enabled { get; set; }
        public bool AllowFreeText { get; set; }
        public int DefaultIndex { get; set; }
        [Required]
        public bool? UseDefault { get; set; }

        public virtual ICollection<GUIPrepopListItem> GUIPrepopListItems { get; set; }
        public virtual ICollection<GuiScreenListScreen> GuiScreenListScreens { get; set; }
    }
}