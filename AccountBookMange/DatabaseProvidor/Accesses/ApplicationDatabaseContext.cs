using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using DatabaseProvidor.Models;

namespace DatabaseProvidor.Accesses
{
    public class ApplicationDatabaseContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Move> Moves { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString =
                new SqliteConnectionStringBuilder
                {
                    DataSource = @"C:\Users\tetsumoto\Documents\05.Lern\01.C#\prism\AccountBookManage\AccountBookMange\DatabaseProvidor\AccountBookManage.db"
                }.ToString();

            optionsBuilder.UseSqlite(new SqliteConnection(connectionString));

            base.OnConfiguring(optionsBuilder);
        }
    }
}
