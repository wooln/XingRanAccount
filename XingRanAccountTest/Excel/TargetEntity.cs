using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCS.Library.Data.DataObjects;

namespace XingRanAccountTest.Excel
{
    public class TargetEntity : ScopeEntity
    {
        public string 金额 { get; set; }
        public string 税率 { get; set; }
        public string 税额 { get; set; }
        public string Description { get; set; }
        public string LedgerExpAc { get; set; }
        public string ExpCostCenter { get; set; }
        public string ExpenseAmount { get; set; }
        public string AmountSpecial { get; set; }
        public string TranexpenseAmount { get; set; }
    }

    public class TargetEntityColl : EditableDataObjectCollectionBase<TargetEntity> { }

}
