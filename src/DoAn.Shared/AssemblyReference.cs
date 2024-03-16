using System.Reflection;

namespace DoAn.Shared;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}