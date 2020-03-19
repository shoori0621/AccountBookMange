using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseProvidor.Models
{
    [Table("income")]
    public class Income
    {
        /// <summary>ID</summary>
        [Column("id")]
        [Key]
        [Required]
        public long Id { get; set; }

        /// <summary>ユーザID</summary>
        [Column("user_id")]
        public long? UserId { get; set; }

        /// <summary>入金日</summary>
        [Column("income_date")]        
        public string IncomeDate { get; set; }

        /// <summary>入金区分</summary>
        [Column("income_kind")]
        public long? IncomeKind { get; set; }

        /// <summary>入金額</summary>
        [Column("income_price")]
        public long? IncomePrice { get; set; }

        /// <summary>口座ID</summary>
        [Column("account_id")]
        public long? AccountId { get; set; }

        [Column("comment")]
        public string Comment { get; set; }

        public Income() { }

        public Income(long id)
        {
            this.Id = id;
        }
    }
}
