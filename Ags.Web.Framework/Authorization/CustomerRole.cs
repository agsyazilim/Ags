namespace Ags.Web.Framework.Authorization
{
    public static class CustomerRole
    {
        /// <summary>
        /// Class Constants.
        /// </summary>
        public static class Constants
        {
            /// <summary>
            /// The create operation name
            /// </summary>
            public static readonly string CreateOperationName = "Create";
            /// <summary>
            /// The read operation name
            /// </summary>
            public static readonly string ReadOperationName = "Read";
            /// <summary>
            /// The update operation name
            /// </summary>
            public static readonly string UpdateOperationName = "Update";
            /// <summary>
            /// The delete operation name
            /// </summary>
            public static readonly string DeleteOperationName = "Delete";
            /// <summary>
            /// The approve operation name
            /// </summary>
            public static readonly string ApproveOperationName = "Approve";
            /// <summary>
            /// The reject operation name
            /// </summary>
            public static readonly string RejectOperationName = "Reject";
            /// <summary>
            /// The customer administrators role
            /// </summary>
            public static readonly string CustomerAdministratorsRole = "Admin";
            /// <summary>
            /// The customer managers role
            /// </summary>
            public static readonly string CustomerManagersRole = "Editor";

            /// <summary>
            /// The customer member
            /// </summary>
            public static readonly string CustomerMemberRole = "Member";

            /// <summary>
            /// AccessAdminPanel
            /// </summary>
            public static readonly string AccessAdminPanel = "AccessAdminPanel";
            /// <summary>
            /// PublicAccessSite
            /// </summary>
            public static readonly string PublicAccessSite = "PublicAccessSite";

        }
    }
}