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
        /// <summary>ID</summary>
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
                var income = context.Incomes.Single(x => x.Id == id);
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
                }

                context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// N件取得　ユーザIDが条件
        /// </summary>
        /// <param name="income"></param>
        public List<Income> FindByUserId(long userId)
        {
            using (var context = new ApplicationDatabaseContext())
            {
                return context.Incomes.Where(x => x.UserId == userId).ToList();
            }
        }
    }
}
