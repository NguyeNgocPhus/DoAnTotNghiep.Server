namespace DoAn.API.Common;

public static class Url
{
    public static class ADMIN
    {
        public static class Roles
        {
            public const string ViewList = "Roles";
        }
        public static class Workflow
        {
            public const string TestStartWorkflow = "StartWorkflow";
            public const string ExecuteWorkflow = "ExecuteWorkflow";
            public const string GetCurrentStepWorkflow = "CurrentStepWorkflow/{fileId}";
            public const string GetWorkflowActivity = "WorkflowActivity/{fileId}";
            public const string GetWorkflowHistory = "WorkflowHistory/{fileId}";
            public const string ViewListWorkflowDefinition = "WorkflowDefinitions";
            public const string CreateWorkflowDefinition = "WorkflowDefinition";
            public const string UpdateWorkflowDefinition = "WorkflowDefinition/{id}";
            public const string DeleteWorkflowDefinition = "WorkflowDefinition/{id}";
            public const string ViewWorkflowDefinition = "WorkflowDefinition/{id}";
            public const string ViewNode = "WorkflowDefinition/{id}/Node/{type}";
        }
        public static class FileStorage
        {
            public const string Upload = "FileStorage/Upload";
            public const string Get = "FileStorage/Get/{id}";
          
        }
        public static class ImportTemplate
        {
            public const string Create = "ImportTemplate";
            public const string Update = "ImportTemplate/{id}";
            public const string Delete = "ImportTemplate/{id}";
            public const string ImportData = "ImportTemplate/ImportData";
            public const string View = "ImportTemplate/{id}";
            public const string ViewList = "ImportTemplates";
        }
        public static class Notification
        {
            
            public const string ViewList = "Notifications";
            public const string CountUnread = "Notifications/CountUnread";
        }
        public static class ImportHistory
        {
            public const string ViewList = "ImportHistories";
        }
        public static class Identity
        {
            public const string Login = "Login";
            public const string Logout = "Logout";

            public static class User
            {
                public const string Create = "User";
                public const string Update = "User"; 
                public const string Delete = "User/{id}"; 
                public const string ViewList = "Users"; 
                public const string View = "User/{id}"; 
            }
        }
    }
}