using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseProvidor.Models
{
    [Table("payment")]
    public class Payment
    {
        /// <summary>ID</summary>
        [Column("id")]
        [Key]
        [Required]
        public long Id { get; set; }

        /// <summary>ユーザID</summary>
        [Column("user_id")]
        public long? UserId { get; set; }

        /// <summary>支払日</summary>
        [Column("payment_date")]
        public string PaymentDate { get; set; }

        /// <summary>支払額</summary>
        [Column("payment_price")]
        public long? PaymentPrice { get; set; }

        /// <summary>支払区分</summary>
        [Column("payment_kind")]
        public long? PaymentKind { get; set; }

        /// <summary>支払方法</summary>
        [Column("payment_way")]
        public long? PaymentWay { get; set; }

        /// <summary>口座ID</summary>
        [Column("account_id")]
        public long? AccountId { get; set; }

        /// <summary>クレジットカードID</summary>
        [Column("card_id")]
        public long? CardId { get; set; }

        [Column("comment")]
        public string Comment { get; set; }

        public Payment() { }

        public Payment(long id)
        {
            this.Id = id;
        }
    }
}
