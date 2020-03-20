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
    [Table("account")]
    public class Account : ModelBase
    {
        /// <summary>ID</summary>
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public long Id { get; set; }

        /// <summary>ユーザID</summary>
        [Column("user_id")]
        [Required]
        public long UserId { get; set; }

        /// <summary>口座名</summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>削除フラグ</summary>
        [Column("delete_flag")]
        public long? DeleteFlag { get; set; }

        public Account() 
        {            
        }

        public Account(long id, long userId)
        {
            this.FindByKey(id, userId);
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
                    context.Accounts.Add(this);
                }
                else
                {
                    //更新処理
                    var data = context.Accounts.Single(x => x.Id == this.Id && x.UserId == this.UserId);

                    data.Name = this.Name;
                }

                context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 論理削除
        /// </summary>
        public void Remove()
        {
            using (var context = new ApplicationDatabaseContext())
            {
                if (!this.IsValuable)
                {
                    var data = context.Accounts.Single(x => x.Id == this.Id && x.UserId == this.UserId);

                    data.DeleteFlag = 1;
                }

                context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 一意取得
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        public void FindByKey(long id, long userId)
        {
            using (var context = new ApplicationDatabaseContext())
            {
                var account = context.Accounts.Single(x => x.Id == id && x.UserId == userId);
                if(account != null)
                {
                    this.Id = account.Id;
                    this.UserId = account.UserId;
                    this.Name = account.Name;
                    this.DeleteFlag = account.DeleteFlag;

                    this.IsValuable = true;
                }

                context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// N件取得　ユーザIDが条件
        /// </summary>
        /// <param name="account"></param>
        public List<Account> FindByUserId(long userId)
        {
            using(var context = new ApplicationDatabaseContext())
            {
                return context.Accounts.Where(x => x.UserId == userId).ToList();
            }
        }
    }
}
