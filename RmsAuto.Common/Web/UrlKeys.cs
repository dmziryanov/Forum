using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Common.Web
{
    /// <summary>
    /// Давайте будем хранить тут всевозможные ключи урлов
    /// </summary>
    public static class UrlKeys
    {
        public static class Ajax
        {
            /// <summary>
            /// This const is to use with url rewriting
            /// </summary>
            public const string Action = "Action";
        }

        public static class OrderLineRequests
        {
            public const string OrderLineId = "olid";
			public const string AcctgOrderLineId = "acolid";
        }

        public static class VinRequests
        {
            public const string RequestId = "rid";
            public const string CarId = "cid";
            public const string Action = "Action";

            public static class Operations
            {
                public const string New = "New";
                public const string Delete = "Delete";
            }
        }

        public static class Forum
        {
            public static class Operations
            {
                public const string Delete = "delete";
                public const string Quote = "quote";
                public const string Edit = "edit";
                public const string Create = "create";
            }

            /// <summary>
            /// This const is to use with url rewriting
            /// </summary>
            public const string PostId = "PostId";
            /// <summary>
            /// This const is to use with url rewriting
            /// </summary>
            public const string ForumId = "ForumId";
            /// <summary>
            /// This const is to use with url rewriting
            /// </summary>
            public const string Action = "Action";
        }

        public static class Paging
        {
            public const string PageNum = "page";
            public const string PageSize = "size";
        }

        public static class AcctgTrace
        {
            public const string ItemId = "id";
            public const string ItemPart = "t";
            public const string RequestPart = "req";
            public const string ResponsePart = "rsp";
            public const string Clear = "clear";
        }

        public static class StoreAndTecdoc
        {
			public const string ManufacturerName = "mfr";
            public const string EnteredPartNumber = "pn";
            public const string SearchCounterparts = "st";
			public const string ExcludeSupplierID = "suppl";
			public const string ArticleId = "aid";

            public const string ManufacturerId = "mfrid";
            public const string ModelId = "mid";
            public const string CarTypeId = "modid";
            public const string SearchTreeNodeId = "tid";
            public const string IsCarModelId = "t";
            public const string StepId = "s";
        }

        public static class Activation
        {
            public const string MaintUid = "cc";
            public const string FranchCode = "fc";
        }

        public const string Operation = "op";
        public const string Id = "id";
		public const string SparePartID = "spid";

        public static class CommonOperations
        {
            public const string Refresh = "refresh";
        }
    }
}
