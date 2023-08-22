using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CashSwiftDataAccess.Entities
{
    [Table("Country")]
    public partial class Country
    {
        public Country()
        {
            Banks = new HashSet<Bank>();
        }

        [Key]
        [StringLength(2)]
        // [Unicode(false)]
        public string country_code { get; set; }
        [Required]
        [StringLength(100)]
        // [Unicode(false)]
        public string country_name { get; set; }

        // [InverseProperty("country_codeNavigation")]
        public virtual ICollection<Bank> Banks { get; set; }
    }
}