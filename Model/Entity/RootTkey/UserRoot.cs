using SqlSugar;
using System;
using System.Collections.Generic;

namespace Blog.Core.Model.Entity.RootTkey
{
    /// <summary>
    /// 用户根 (放sqlsugar忽略的字段，如RIDs)
    /// </summary>
    public class UserRoot<Tkey>: BaseEntity where Tkey : IEquatable<Tkey>
    {

        [SugarColumn(IsIgnore = true)]
        public List<Tkey> RIDs { get; set; }

    }
}
