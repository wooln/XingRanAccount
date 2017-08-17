using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Library.Core;
using MCS.Library.Data.Mapping;
using MCS.Library.Office.OpenXml.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XingRanAccountTest.Excel;

namespace XingRanAccountTest
{
    [TestClass]
    public class ExcelDo
    {
        public static bool Do(WorkBook sourceBook, WorkBook scopeBook, out string msg)
        {
            var sourceTable = DocumentHelper.GetRangeValuesAsTable(sourceBook, sourceBook.Sheets.First().Name, "A1");
            sourceTable.RenameColumnByOriginalType(typeof(SourceEntity));
            SourceEntityColl sourceList = new SourceEntityColl();
            ORMapping.DataViewToCollection(sourceList, sourceTable.DefaultView);

            var scopeTable = DocumentHelper.GetRangeValuesAsTable(scopeBook, "Scope", "A1");
            scopeTable.RenameColumnByOriginalType(typeof(SourceEntity));
            ScopeEntityColl scopeList = new ScopeEntityColl();
            ORMapping.DataViewToCollection(scopeList, scopeTable.DefaultView);


            List<TargetEntity> targetList = ToTarget(sourceList, scopeList, out msg);
            if (msg.IsNullOrEmpty())
            {
                var targetTable = scopeBook.Sheets["Target"].Tables.First();
                targetTable.Rows.Clear();
                targetTable.FillData(targetList.ToDataTable().DefaultView);
                targetTable.TableStyle = ExcelTableStyles.Custom;
            }
            return msg.IsNullOrEmpty();
        }

        private static List<TargetEntity> ToTarget(SourceEntityColl sourceList, ScopeEntityColl scopeList, out string msg)
        {
            List<TargetEntity> targetList = new List<TargetEntity>();
            foreach (ScopeEntity scope in scopeList)
            {
                SourceEntity[] sources = sourceList.Where(p => p.InvoiceNo == scope.发票号码).ToArray();
                if (sources.Length == 0)
                {
                    msg = $"{scope.发票号码}未找到原始记录";
                    return null;
                }

                SourceEntity[] account127800S = sources.Where(p => p.Account == "127800").ToArray();
                if (account127800S.Length == 0)
                {
                    msg = $"{scope.发票号码}找未找到127800";
                    return null;
                }
                if (account127800S.Length > 1)
                {
                    msg = $"{scope.发票号码}找到{account127800S.Length}个127800";
                    return null;
                }
                SourceEntity account127800 = account127800S.Single();

                SourceEntity[] otherSource = sources.Where(p => p.Account != "127800").ToArray();
                if (otherSource.Length == 0)
                {
                    msg = $"{scope.发票号码}未找到普通帐号(非127800)记录";
                    return null;
                }

                TargetEntity[] otherTarget = otherSource.Select(source =>
                {
                    TargetEntity target = new TargetEntity()
                    {
                        ExpenseAmount = source.MonetaryAmount.ToString(),
                        LedgerExpAc = source.Account,
                        ExpCostCenter = source.Dept.Substring(3, source.Dept.Length - 3),
                        //TranexpenseAmount = ??
                    };
                    return target;

                }).ToArray();

                TargetEntity amountSpecialTarge = otherTarget.First();
                amountSpecialTarge.序号 = scope.序号;
                amountSpecialTarge.认证日期 = scope.认证日期;
                amountSpecialTarge.发票类型 = scope.发票类型;
                amountSpecialTarge.发票号码 = scope.发票号码;
                amountSpecialTarge.AmountSpecial = account127800.MonetaryAmount.ToString();

                amountSpecialTarge.金额 = otherSource.Sum(p => p.MonetaryAmount).ToString();
                amountSpecialTarge.税额 = account127800.MonetaryAmount.ToString();
                amountSpecialTarge.税率 = (account127800.MonetaryAmount / otherSource.Sum(p => p.MonetaryAmount)).ToString();


                otherTarget.ForEach(p =>
                {
                    p.TranexpenseAmount = (
                            decimal.Parse(amountSpecialTarge.税额) * decimal.Parse(p.ExpenseAmount)
                            / decimal.Parse(amountSpecialTarge.金额)
                        ).ToString();
                });

                targetList.AddRange(otherTarget);
            }

            msg = null;
            return targetList;
        }
    }
}