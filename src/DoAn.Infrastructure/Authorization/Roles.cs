using System.ComponentModel;

namespace DoAn.Infrastructure.Authorization;

public enum ERoles
{
    [Description("SuperAdmin")]
    Admin = 1,
    [Description("Upload tài liệu")]
    Upload = 2,
    [Description("Quy trình phê duyệt")]
    Workflow = 3,
    [Description("Admin đơn vị")]
    DonVi = 4,
    [Description("Admin tập đoàn")]
    TapDoan = 5,
    
}
public static class ERolesExtensions
{
    public static T GetAttributeOfType<T>(this ERoles enumVal) where T : System.Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return (attributes.Length > 0) ? (T)attributes[0] : null;
    }
    
    public static string[] TypeSupported()
    {
        return Enum.GetNames<ERoles>();
    }

    public static IDictionary<int, string> TypeSupports()
    {
        return Enum.GetValues<ERoles>().ToDictionary(x => (int)x, x => x.ToString());
    }

    public static ERoles GetEDataType(this string name)
    {
        return Enum.GetValues<ERoles>().FirstOrDefault(f => f.ToString() == name);
    }
}