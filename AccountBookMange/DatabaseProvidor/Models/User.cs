using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseProvidor.Models
{
    [Table("user")]
    public class User
    {
        /// <summary>ユーザID</summary>
        [Column("id")]
        [Key]
        [Required]        
        public long Id { get; set; }

        /// <summary>ユーザ名</summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>アカウント名</summary>
        [Column("account")]
        public string Account { get; set; }

        /// <summary>パスワード</summary>
        [Column("password")]
        public string Password { get; set; }
         
        /// <summary>削除フラグ</summary>
        [Column("delete_flag")]
        public long? DeleteFlag { get; set; }

        public User() { }

        public User(long id)
        {
            this.Id = id;
        }
    }
}
