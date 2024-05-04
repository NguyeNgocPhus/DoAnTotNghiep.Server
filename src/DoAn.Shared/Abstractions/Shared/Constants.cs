using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DoAn.Shared.Abstractions.Shared

{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        /// <summary>
        /// To enable database auto increment if needed
        /// </summary>
        public const bool EnableAi = true;

        /// <summary>
        /// Number of digits to be rounded. E.g. 1.231 -> 1.23
        /// </summary>
        public const int RoundDigits = 2;
        public static class ApiVersion
        {
            public const string V1_0 = "1.0";
        }
        public static class QueueName
        {
            public const string Notification = "notification";
            public const string Reminder = "reminder";
            public const string Default = "default";
            public const string Synchronization = "synchronization";
        }
        public static class SwaggerTags
        {
            public const string Account = "To manage user accounts in this system";
            public const string Permission = "To manage permissions in this system";
            public const string Role = "To manage roles in this system";
            public const string File = "To manage resources in this system";
            public const string Tenant = "To manage tenants in this system";
            public const string KhuVuc = "To manage khu vuc in this system";
            public const string Mien = "To manage mien in this system";
            public const string MatHang = "To manage mat hang in this system";
            public const string NhaHang = "To manage nha hang in this system";
            public const string ThuongHieu = "To manage thuong hieu in this system";
            public const string Tinh = "To manage tinh in this system";
            public const string Main = "To manage main report in this system";
            public const string NhaCungCap = "To manage nha cung cap in this system";
            public const string MaterialGroup = "To manage material group in this system";
        }

        public static class EmailProvider
        {
            public const string Gmail = "GMAIL";
            public const string Hotmail = "HOTMAIL";
            public const string Exchange = "EXCHANGE";
        }

        public static class FieldLength
        {
            public const int TextMinLength = 3;
            public const int TextMaxLength = 255;
            public const int MiddleTextLength = 50;
            public const int UrlMaxLength = 1000;
            public const int DescriptionMaxLength = 1000;
            public const int RecaptchaMaxLength = 1000;
            public const int FirebaseTokenMaxLength = 700;
        }

        public static class CommonFields
        {
            public const string RecaptchaToken = "RecaptchaToken";
        }

        public static class MaterializedPath
        {
            public const int PathLength = 5; // supported up to 50 levels deep
            public const long Max = 60466175; // ZZZZZ = 60 466 175
            public const string MaxString = "ZZZZZ";
            public const long Min = 0;
            public const string MinString = "00000";
        }

        public static class MimeTypes
        {
            public static class Text
            {
                public const string Plain = "text/plain";
                public const string Html = "text/html";
                public const string Xml = "text/xml";
                public const string RichText = "text/richtext";
            }

            public static class Application
            {
                public const string Soap = "application/soap+xml";
                public const string Octet = "application/octet-stream";
                public const string Rtf = "application/rtf";
                public const string Pdf = "application/pdf";
                public const string Zip = "application/zip";
                public const string Json = "application/json";
                public const string Xml = "application/xml";
            }

            public static class Image
            {
                public const string Gif = "image/gif";
                public const string Tiff = "image/tiff";
                public const string Jpeg = "image/jpeg";
            }
        }

        /// <summary>
        /// For ActionLog usage. Each action should be under ObjectName|ActionName format
        /// </summary>
        public static class Actions
        {
            public static class Identity
            {
                public static class Account
                {
                    public const string Create = "Identity|Account|Create";
                    public const string SignInWithPhoneNumber = "Identity|Account|SignInWithPhoneNumber";
                    public const string Deactivate = "Identity|Account|Deactivate";
                    public const string Activate = "Identity|Account|Activate";
                    public const string Delete = "Identity|Account|Delete";
                    public const string View = "Identity|Account|View";
                    public const string ViewList = "Identity|Account|ViewList";
                    public const string ViewMyAccount = "Identity|Account|ViewMyAccount";
                    public const string SignInWithOAuth = "Identity|Account|SignInWithOAuth";
                    public const string ForgotMyPassword = "Identity|Account|ForgotMyPassword";
                    public const string ActivateMyAccount = "Identity|Account|ActivateMyAccount";
                    public const string UpdateMyAccount = "Identity|Account|UpdateMyAccount";
                    public const string Logout = "Identity|Account|Logout";
                    public const string ChangeMyPassword = "Identity|Account|ChangeMyPassword";
                    public const string UpdateAccountFCMToken = "Identity|Account|UpdateFCMToken";
                    public const string Lock = "Identity|Account|Lock";
                    public const string Unlock = "Identity|Account|Unlock";
                    public const string SetAccountPassword = "Identity|Account|SetPassword";
                    public const string Update = "Identity|Account|Update";
                    public const string SetMyNewPassword = "Identity|Account|SetMyNewPassword";
                    public const string ChangePasswordAtFirstLogin = "Identity|Account|ChangePasswordFirstLogin";
                    public const string SignInWithUserName = "Identity|Account|SignInWithUserName";

                }

                public static class Permission
                {
                    public const string Update = "Identity|Permission|Update";
                }

                public static class Role
                {
                    public const string Update = "Identity|Role|Update";
                    public const string Delete = "Identity|Role|Delete";
                    public const string Activate = "Identity|Role|Activate";
                    public const string Deactivate = "Identity|Role|Deactivate";
                    public const string Create = "Identity|Role|Create";
                }
            }

            public static class Resource
            {
                public static class File
                {
                    public const string Upload = "Resource|File|Upload";
                }
            }

            public static class DMP
            {
                public static class DimKhuVuc
                {
                    public const string ViewList = "DMP|DimKhuDuc|ViewList";
                }
                public static class DimMien
                {
                    public const string ViewList = "DMP|DimMien|ViewList";
                }
                public static class DimMatHang
                {
                    public const string ViewList = "DMP|DimMatHang|ViewList";
                }
                public static class DimNhaHang
                {
                    public const string ViewList = "DMP|DimNhaHang|ViewList";
                }
                public static class DimThuongHieu
                {
                    public const string ViewList = "DMP|DimThuongHieu|ViewList";
                }
                public static class DimTinh
                {
                    public const string ViewList = "DMP|DimTinh|ViewList";
                }

                public static class DimMain
                {
                    public const string ViewList = "DMP|DimMain|ViewList";
                }
                public static class DimNhaCungCap
                {
                    public const string ViewList = "DMP|DimNhaCungCap|ViewList";
                }
                public static class DimMaterialGroup
                {
                    public const string ViewList = "DMP|DimMaterialGroup|ViewList";
                }
            }

            public static class Tenant
            {
                public const string Create = "Tenant|Tenant|Create";
                public const string Update = "Tenant|Tenant|Update";
                public const string Delete = "Tenant|Tenant|Delete";
                public const string Activate = "Tenant|Tenant|Activate";
                public const string Deactivate = "Tenant|Tenant|Deactivate";
            }
        }

        
        public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
        };

        public static string JsonDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ";

        public static class LoginProviders
        {
            public const string Self = "SELF";
            public const string Ldap = "LDAP";
            public const string Google = "GOOGLE";
            public const string Microsoft = "MICROSOFT";
            public const string Facebook = "FACEBOOK";
        }

        public static class SupportedCultures
        {
            public static string[] Cultures =
            {
                English,
                Japanese,
                Vietnamese
            };

            public const string English = "en-US";
            public const string Japanese = "ja-JP";
            public const string Vietnamese = "vi-VN";
        }

        public static class Pagination
        {
            public const int DefaultPage = 1;
            public const int DefaultSize = 10;
            public const bool DefaultOrderByDesc = true;
            public const int DefaultCurrentPage = 0;
            public const int DefaultTotalPages = 0;
            public const int DefaultTotalItems = 0;
            public const string DefaultOrderBy = "";
        }

        public static class Others
        {
            public const string DateTimeFormat = "yyyy/MM/dd";
            public const string ExcelFileDateTimeFormat = "yyyyMMddHHMMssfff";
            public const string JsonDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ";
            public const string PermissionClaimType = "Permission";
            public const bool AutoIncrementEnabled = true;
        }
        
        public static class Permissions
        {
            // Structure area:module:perm
            public const string SysAdmin = "ROOT:ROOT:SYSADMIN";
            public const string ScpUser = "SC:PROVIDER:USER";
            public const string TenantAdmin = "TENANT:ROOT:ADMIN";
            public const string UnlockAccount = "IDENTITY:ACCOUNT:UNLOCK";
            public const string LockAccount = "IDENTITY:ACCOUNT:LOCK";
        }
    }
}