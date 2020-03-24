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
    [Table("income")]
    public class Income : ModelBase
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

        private string incomeDate;
        /// <summary>入金日</summary>
        [Column("income_date")]        
        public string IncomeDate
        {
            get
            {
                return incomeDate;
            }
            set
            {
                SetProperty(ref incomeDate, value);
            }
        }

        /// <summary>
        /// 入金日　日付型
        /// </summary>
        [NotMapped]
        public DateTime? DateTimeIncomeDate
        {
            get
            {
                DateTime? result = null;
                if(this.IncomeDate != null)
                {
                    result = DateTime.Parse(this.IncomeDate);
                }

                return result;
            }
            set
            {
                this.IncomeDate = value != null ? value.ToString() : null;
            }
        }

        private long? incomeKind;
        /// <summary>入金区分</summary>
        [Column("income_kind")]
        public long? IncomeKind
        {
            get
            {
                return incomeKind;
            }
            set
            {
                SetProperty(ref incomeKind, value);
            }
        }

        private long? incomePrice;
        /// <summary>入金額</summary>
        [Column("income_price")]
        public long? IncomePrice
        {
            get
            {
                return incomePrice;
            }
            set
            {
                SetProperty(ref incomePrice, value);
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

        private string comment;
        /// <summary>備考</summary>
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

        [ForeignKey("account_id")]
        public Account Account { get; set; }

        public Income() { }

        public Income(long id)
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
                    context.Incomes.Add(this);
                }
                else
                {
                    //更新処理
                    var data = context.Incomes.Single(x => x.Id == this.Id && x.UserId == this.UserId);

                    data.UserId = this.UserId;
                    data.IncomeDate = this.IncomeDate;
                    data.IncomePrice = this.IncomePrice;
                    data.IncomeKind = this.IncomeKind;
                    data.AccountId = this.AccountId;
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
                    var data = context.Incomes.Single(x => x.Id == this.Id);
                    context.Incomes.Remove(data);
                   
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
                var income = context.Incomes.SingleOrDefault(x => x.Id == id);
                if (income != null)
                {
                    this.Id = income.Id;
                    this.UserId = income.UserId;
                    this.IncomeDate = income.IncomeDate;
                    this.IncomePrice = income.IncomePrice;
                    this.IncomeKind = income.IncomeKind;
                    this.AccountId = income.AccountId;
                    this.Comment = income.Comment;

                    this.IsValuable = true;

                    context.Entry(income).Reference(x => x.Account).Load();

                    this.Account = income.Account;
                }

                context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// N件取得　ユーザIDが条件
        /// </summary>
        /// <param name="income"></param>
        public static List<Income> FindByUserId(long userId)
        {
            using (var context = new ApplicationDatabaseContext())
            {
                return context.Incomes.Where(x => x.UserId == userId).OrderByDescending(x => x.IncomeDate).ToList();
            }
        }
    }
}
