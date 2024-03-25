namespace DoAn.API.Common;

public static class Url
{
    public static class ADMIN
    {
        public static class Workflow
        {
            public const string ViewListWorkflowDefinition = "WorkflowDefinitions";
            public const string CreateWorkflowDefinition = "WorkflowDefinition";
            public const string UpdateWorkflowDefinition = "WorkflowDefinition/{id}";
            public const string DeleteWorkflowDefinition = "WorkflowDefinition/{id}";
            public const string ViewWorkflowDefinition = "WorkflowDefinitions/{id}";
        }
        public static class Identity
        {
            public const string Login = "Login";
            public const string Logout = "Logout";

            public static class User
            {
                public const string Create = "User";
                public const string Update = "User/{id}"; 
                public const string Delete = "User/{id}"; 
                public const string ViewList = "Users"; 
                public const string View = "User/{id}"; 
            }
        }
    }
}