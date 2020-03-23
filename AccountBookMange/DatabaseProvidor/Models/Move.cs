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
    [Table("move")]
    public class Move : ModelBase
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

        private string startDate;
        /// <summary>移動開始日</summary>
        [Column("start_date")]
        public string StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                SetProperty(ref startDate, value);
            }
        }

        private string endDate;
        /// <summary>移動完了日</summary>
        [Column("end_date")]
        public string EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                SetProperty(ref endDate, value);
            }
        }

        private long? preAccountId;
        /// <summary>移動元口座ID</summary>
        [Column("pre_account_id")]
        public long? PreAccountId
        {
            get
            {
                return preAccountId;
            }
            set
            {
                SetProperty(ref preAccountId, value);
            }
        }

        private long? nextAccountId;
        /// <summary>移動先口座ID</summary>
        [Column("next_account_id")]
        public long? NextAccountId
        {
            get
            {
                return nextAccountId;
            }
            set
            {
                SetProperty(ref nextAccountId, value);
            }
        }

        private long? movePrice;
        /// <summary>金額</summary>
        [Column("move_price")]
        public long? MovePrice
        {
            get
            {
                return nextAccountId;
            }
            set
            {
                SetProperty(ref nextAccountId, value);
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

        public Move() { }

        public Move(long id)
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
                    context.Moves.Add(this);
                }
                else
                {
                    //更新処理
                    var data = context.Moves.Single(x => x.Id == this.Id && x.UserId == this.UserId);

                    data.UserId = this.UserId;
                    data.StartDate = this.StartDate;
                    data.EndDate = this.EndDate;
                    data.PreAccountId = this.PreAccountId;
                    data.NextAccountId = this.NextAccountId;
                    data.MovePrice = this.MovePrice;
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
                    var data = context.Moves.Single(x => x.Id == this.Id);
                    context.Moves.Remove(data);
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
                var move = context.Moves.Single(x => x.Id == id);
                if (move != null)
                {
                    this.Id = move.Id;
                    this.UserId = move.UserId;
                    this.StartDate = move.StartDate;
                    this.EndDate = move.EndDate;
                    this.PreAccountId = move.PreAccountId;
                    this.NextAccountId = move.NextAccountId;
                    this.MovePrice = move.MovePrice;
                    this.Comment = move.Comment;

                    this.IsValuable = true;
                }

                context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// N件取得　ユーザIDが条件
        /// </summary>
        /// <param name="income"></param>
        public static List<Move> FindByUserId(long userId)
        {
            using (var context = new ApplicationDatabaseContext())
            {
                return context.Moves.Where(x => x.UserId == userId).OrderByDescending(x => x.StartDate).ToList();
            }
        }
       
    }
}
