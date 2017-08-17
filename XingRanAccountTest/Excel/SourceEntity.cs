using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCS.Library.Data.DataObjects;

namespace XingRanAccountTest.Excel
{
    public class SourceEntity
    {
        [Description("BU")]
        public string BU { get; set; }

        [Description("Voucher ID")]
        public string VoucherID { get; set; }

        [Description("Type")]
        public string Type { get; set; }

        [Description("Invoice No.")]
        public string InvoiceNo { get; set; }

        [Description("Invoice Date")]
        public string InvoiceDate { get; set; }

        [Description("Vendor ID")]
        public string VendorID { get; set; }

        [Description("Vendor Name")]
        public string VendorName { get; set; }

        [Description("Date Entered")]
        public string DateEntered { get; set; }

        [Description("Acctg Date")]
        public string AcctgDate { get; set; }

        [Description("Due Date")]
        public string DueDate { get; set; }

        [Description("Post Status")]
        public string PostStatus { get; set; }

        [Description("Account")]
        public string Account { get; set; }

        [Description("Dept")]
        public string Dept { get; set; }

        [Description("Product")]
        public string Product { get; set; }

        [Description("Monetary Amount")]
        public decimal MonetaryAmount { get; set; }

        [Description("Project ID")]
        public string ProjectID { get; set; }

        [Description("Payment Status")]
        public string PaymentStatus { get; set; }

        [Description("Bank")]
        public string Bank { get; set; }

        [Description("Pay Group")]
        public string PayGroup { get; set; }

        [Description("User")]
        public string User { get; set; }

        [Description("Old Vend #")]
        public string OldVend { get; set; }

        [Description("Contract No")]
        public string ContractNo { get; set; }

        [Description("Line Descr")]
        public string LineDescr { get; set; }

        [Description("More Info")]
        public string MoreInfo { get; set; }

        [Description("Line")]
        public string Line { get; set; }


        [Description("Method")]
        public string Method { get; set; }

        [Description("Distribution Li")]
        public string DistributionLi { get; set; }
    }

    public class SourceEntityColl : EditableDataObjectCollectionBase<SourceEntity> { }
}
