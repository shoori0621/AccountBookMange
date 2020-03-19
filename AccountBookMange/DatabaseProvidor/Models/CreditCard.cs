using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseProvidor.Models
{
    [Table("credit_card")]
    public class CreditCard
    {
        /// <summary>ID</summary>
        [Column("id")]
        [Key]
        [Required]
        public long Id { get; set; }

        /// <summary>ユーザID</summary>
        [Column("user_id")]
        [Required]
        public long UserId { get; set; }

        /// <summary>カード名</summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>削除フラグ</summary>
        [Column("delete_flag")]
        public long? DeleteFlag { get; set; }

        public CreditCard() { }

        public CreditCard(long id, long userId)
        {
            this.Id = id;
            this.UserId = userId;
        }
    }
}
