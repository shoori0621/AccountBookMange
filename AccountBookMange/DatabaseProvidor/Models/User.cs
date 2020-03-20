using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DatabaseProvidor.Accesses;

namespace DatabaseProvidor.Models
{
    [Table("user")]
    public class User : ModelBase
    {
        /// <summary>ユーザID</summary>
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
            this.FindByKey(id);
        }

        /// <summary>
        /// 登録・更新
        /// </summary>
        public void Upsert()
        {
            using (var context = new ApplicationDatabaseContext())
            {
                if (!this.IsValuable)
                {
                    //未登録の場合登録
                    context.Users.Add(this);
                }
                else
                {
                    //更新処理
                    var data = context.Users.Single(x => x.Id == this.Id);

                    data.Id = this.Id;
                    data.Name = this.Name;
                    data.Account = this.Account;
                    data.Password = this.Password;
                    data.DeleteFlag = this.DeleteFlag;
                }

                context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 物理削除
        /// </summary>
        public void Delete()
        {
            using (var context = new ApplicationDatabaseContext())
            {
                if (!this.IsValuable)
                {
                    var data = context.Users.Single(x => x.Id == this.Id);
                    context.Users.Remove(data);

                }

                context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 一意取得
        /// </summary>
        /// <param name="id"></param>
        public void FindByKey(long id)
        {
            using (var context = new ApplicationDatabaseContext())
            {
                var user = context.Users.Single(x => x.Id == id);
                if (user != null)
                {
                    this.Id = user.Id;
                    this.Name = user.Name;
                    this.Account = user.Account;
                    this.Password = user.Password;
                    this.DeleteFlag = user.DeleteFlag;

                    this.IsValuable = true;
                }

                context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 全件取得
        /// </summary>
        /// <param name="user"></param>
        public List<User> FindAll()
        {
            using (var context = new ApplicationDatabaseContext())
            {
                return context.Users.ToList();
            }
        }

    }
}
