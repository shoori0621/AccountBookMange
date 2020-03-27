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
using System.Collections.ObjectModel;

namespace DatabaseProvidor.Models
{
    [Table("user")]
    public class User : ModelBase
    {
        private long id;
        /// <summary>ユーザID</summary>
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

        private string name;
        /// <summary>ユーザ名</summary>
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

        private string account;
        /// <summary>アカウント名</summary>
        [Column("account")]
        public string Account
        {
            get
            {
                return account;
            }
            set
            {
                SetProperty(ref account, value);
            }
        }

        private string password;
        /// <summary>パスワード</summary>
        [Column("password")]
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                SetProperty(ref password, value);
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

        public ObservableCollection<Income> Incomes { get; set; }
        public ObservableCollection<Payment> Payments { get; set; }
        public ObservableCollection<Move> Moves { get; set; }
        public ObservableCollection<Account> Accounts { get; set; }
        public ObservableCollection<CreditCard> CreditCards { get; set; }

        public User()
        {
            //this.Incomes = new ObservableCollection<Income>();
            //this.Payments = new ObservableCollection<Payment>();
            //this.Moves = new ObservableCollection<Move>();
        }

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

                context.SaveChanges();
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
                var user = context.Users.SingleOrDefault(x => x.Id == id);
                if (user != null)
                {
                    this.Id = user.Id;
                    this.Name = user.Name;
                    this.Account = user.Account;
                    this.Password = user.Password;
                    this.DeleteFlag = user.DeleteFlag;

                    this.IsValuable = true;

                    context.Entry(user).Collection(x => x.Incomes).Load();
                    context.Entry(user).Collection(x => x.Payments).Load();
                    context.Entry(user).Collection(x => x.Moves).Load();
                    context.Entry(user).Collection(x => x.Accounts).Load();
                    context.Entry(user).Collection(x => x.CreditCards).Load();

                    this.Incomes = new ObservableCollection<Income>(user.Incomes);
                    this.Payments = new ObservableCollection<Payment>(user.Payments);
                    this.Moves = new ObservableCollection<Move>(user.Moves);
                    this.Accounts = new ObservableCollection<Account>(user.Accounts);
                    this.CreditCards = new ObservableCollection<CreditCard>(user.CreditCards);
                }

                context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 全件取得
        /// </summary>
        /// <param name="user"></param>
        public static List<User> FindAll()
        {
            using (var context = new ApplicationDatabaseContext())
            {
                return context.Users.ToList();
            }
        }

    }
}
