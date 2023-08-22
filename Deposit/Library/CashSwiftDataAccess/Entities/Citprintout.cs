
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("CITPrintout")]
    public partial class CITPrintout
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public DateTime datetime { get; set; }
        public Guid cit_id { get; set; }
        public Guid print_guid { get; set; }
        public string print_content { get; set; }
        public bool is_copy { get; set; }

        [ForeignKey("cit_id")]
        public virtual CIT cit { get; set; }
    }
}