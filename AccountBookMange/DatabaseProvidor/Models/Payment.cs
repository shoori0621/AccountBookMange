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
    [Table("payment")]
    public class Payment : ModelBase
    {
        private long id;
        /// <summary>ID</summary>
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        private long? userId;
        /// <summary>ユーザID</summary>
        [Column("user_id")]
        public long? UserId
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

        private string paymentDate;
        /// <summary>支払日</summary>
        [Column("payment_date")]
        public string PaymentDate
        {
            get
            {
                return paymentDate;
            }
            set
            {
                SetProperty(ref paymentDate, value);
            }
        }

        private long? paymentPrice;
        /// <summary>支払額</summary>
        [Column("payment_price")]
        public long? PaymentPrice
        {
            get
            {
                return paymentPrice;
            }
            set
            {
                SetProperty(ref paymentPrice, value);
            }
        }

        private long? paymentKind;
        /// <summary>支払区分</summary>
        [Column("payment_kind")]
        public long? PaymentKind
        {
            get
            {
                return paymentKind;
            }
            set
            {
                SetProperty(ref paymentKind, value);
            }
        }

        private long? paymentWay;
        /// <summary>支払方法</summary>
        [Column("payment_way")]
        public long? PaymentWay
        {
            get
            {
                return paymentWay;
            }
            set
            {
                SetProperty(ref paymentWay, value);
            }
        }

        private long? accountId;
        /// <summary>口座ID</summary>
        [Column("account_id")]
        public long? AccountId
        {
            get
            {
                return accountId;
            }
            set
            {
                SetProperty(ref accountId, value);
            }
        }

        private long? cardId;
        /// <summary>クレジットカードID</summary>
        [Column("card_id")]
        public long? CardId
        {
            get
            {
                return cardId;
            }
            set
            {
                SetProperty(ref cardId, value);
            }
        }

        private string comment;
        [Column("comment")]
        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                SetProperty(ref comment, value);
            }
        }

        public Payment() { }

        public Payment(long id)
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
                    context.Payments.Add(this);
                }
                else
                {
                    //更新処理
                    var data = context.Payments.Single(x => x.Id == this.Id && x.UserId == this.UserId);

                    data.UserId = this.UserId;
                    data.PaymentDate = this.PaymentDate;
                    data.PaymentPrice = this.PaymentPrice;
                    data.PaymentKind = this.PaymentKind;
                    data.PaymentWay = this.PaymentWay;
                    data.AccountId = this.AccountId;
                    data.CardId = this.CardId;
                    data.Comment = this.Comment;
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
                    var data = context.Payments.Single(x => x.Id == this.Id);
                    context.Payments.Remove(data);

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
                var payment = context.Payments.Single(x => x.Id == id);
                if (payment != null)
                {
                    this.Id = payment.Id;
                    this.UserId = payment.UserId;
                    this.PaymentDate = payment.PaymentDate;
                    this.PaymentPrice = payment.PaymentPrice;
                    this.PaymentKind = payment.PaymentKind;
                    this.PaymentWay = payment.PaymentWay;
                    this.AccountId = payment.AccountId;
                    this.CardId = payment.CardId;
                    this.Comment = payment.Comment;

                    this.IsValuable = true;
                }

                context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// N件取得　ユーザIDが条件
        /// </summary>
        /// <param name="payment"></param>
        public static List<Payment> FindByUserId(long userId)
        {
            using (var context = new ApplicationDatabaseContext())
            {
                return context.Payments.Where(x => x.UserId == userId).OrderByDescending(x => x.PaymentDate).ToList();
            }
        }
    }
}
