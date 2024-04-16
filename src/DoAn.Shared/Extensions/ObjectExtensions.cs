using System.Reflection;

namespace DoAn.Shared.Extensions;

public static class ObjectExtension
{
    public static bool TryGetValue(this object input, string key, out object value)
    {
        var myType = input.GetType();
        value = myType?.GetType()!.GetProperty(key)?.GetValue(input, null);
        return value is not null;
    }
}