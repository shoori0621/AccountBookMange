using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DatabaseProvidor.Models
{
    public class ModelBase
    {
        /// <summary>DBの実データ</summary>
        protected bool IsValuable { get; set; }        
    }
}
