using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCS.Library.Data.DataObjects;

namespace XingRanAccountTest.Excel
{
    public class ScopeEntity
    {
        [Description("序号")]
        public string 序号 { get; set; }

        [Description("认证日期")]
        public string 认证日期 { get; set; }

        [Description("发票类型")]
        public string 发票类型 { get; set; }

        [Description("发票号码")]
        public string 发票号码 { get; set; }
    }

    public class ScopeEntityColl : EditableDataObjectCollectionBase<ScopeEntity> { }

}
