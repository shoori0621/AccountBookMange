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
    [Table("credit_card")]
    public class CreditCard : ModelBase
    {
        private long id;
        /// <summary>ID</summary>
        [Column("id")]
        [Key]
        [Required]
        public long Id
        {
            get
            {
                return id;
            }
            set
            {
                SetProperty(ref id, value);
            }
        }

        private long userId;
        /// <summary>ユーザID</summary>
        [Column("user_id")]
        [Required]
        public long UserId
        {
            get
            {
                return userId;
            }
            set
            {
                SetProperty(ref userId, value);
            }
        }

        private string name;
        /// <summary>カード名</summary>
        [Column("name")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                SetProperty(ref name, value);
            }
        }

        private long? deleteFlag;
        /// <summary>削除フラグ</summary>
        [Column("delete_flag")]
        public long? DeleteFlag
        {
            get
            {
                return deleteFlag;
            }
            set
            {
                SetProperty(ref deleteFlag, value);
            }
        }

        public CreditCard() { }

        public CreditCard(long id, long userId)
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
                    context.CreditCards.Add(this);
                }
                else
                {
                    //更新処理
                    var data = context.CreditCards.Single(x => x.Id == this.Id && x.UserId == this.UserId);

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
                    var data = context.CreditCards.Single(x => x.Id == this.Id && x.UserId == this.UserId);

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
                var creditCard = context.CreditCards.Single(x => x.Id == id && x.UserId == userId);
                if (creditCard != null)
                {
                    this.Id = creditCard.Id;
                    this.UserId = creditCard.UserId;
                    this.Name = creditCard.Name;
                    this.DeleteFlag = creditCard.DeleteFlag;

                    this.IsValuable = true;
                }

                context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// N件取得　ユーザIDが条件
        /// </summary>
        /// <param name="creditCard"></param>
        public static List<CreditCard> FindByUserId(long userId)
        {
            using (var context = new ApplicationDatabaseContext())
            {
                return context.CreditCards.Where(x => x.UserId == userId).ToList();
            }
        }
    }
}
