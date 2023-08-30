using SqlSugar;
using System;

namespace Blog.Core.Model.Entity.RootTkey
{
    public class RootEntityTkey<Tkey> where Tkey : IEquatable<Tkey>
    {
        /// <summary>
        /// ID
        /// 泛型主键Tkey
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Tkey Id { get; set; }
    }
}