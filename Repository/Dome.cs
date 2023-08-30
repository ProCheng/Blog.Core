using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace Blog.Core.Repository
{
    public class Dome
    {
        public Dome()
        {
            new ConnectionConfig()
            {
                ConnectionString = "连接符字串",
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true
            };
        }
    }
}
