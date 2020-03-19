using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DatabaseProvidor.Models
{
    [Table("move")]
    public class Move
    {
        /// <summary>ID</summary>
        [Column("id")]
        [Key]
        [Required]
        public long Id { get; set; }

        /// <summary>ユーザID</summary>
        [Column("user_id")]
        public long? UserId { get; set; }

        /// <summary>移動開始日</summary>
        [Column("start_date")]
        public string StartDate { get; set; }
                
        /// <summary>移動元口座ID</summary>
        [Column("pre_account_id")]
        public long? PreAccountId { get; set; }

        /// <summary>移動先口座ID</summary>
        [Column("next_account_id")]
        public long? NextAccountId { get; set; }

        /// <summary>金額</summary>
        [Column("move_price")]
        public long? MovePrice { get; set; }

        [Column("comment")]
        public string Comment { get; set; }

        public Move() { }

        public Move(long id)
        {
            this.Id = id;
        }
    }
}
