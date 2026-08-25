using System.Reflection;
using System.Runtime.InteropServices;

namespace SciFor.Api.Tests;

public sealed class ContainmentTests
{
    [Fact]
    public void no_libscifor_pinvoke()
    {
        var apiAsm = typeof(Program).Assembly;
        var domainAsm = typeof(SciFor.Domain.Ports.ILinearGrid).Assembly;

        foreach (var asm in new[] { apiAsm, domainAsm })
        {
            foreach (var reference in asm.GetReferencedAssemblies())
            {
                var refName = reference.Name ?? string.Empty;
                Assert.DoesNotContain("libscifor", refName, StringComparison.OrdinalIgnoreCase);
            }

            foreach (var type in asm.GetTypes())
            {
                foreach (var method in type.GetMethods(
                             BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static |
                             BindingFlags.DeclaredOnly))
                {
                    var dllImport = method.GetCustomAttribute<DllImportAttribute>();
                    if (dllImport is not null)
                    {
                        var value = dllImport.Value ?? string.Empty;
                        Assert.DoesNotContain("libscifor", value, StringComparison.OrdinalIgnoreCase);
                        Assert.DoesNotContain("scifor", value, StringComparison.OrdinalIgnoreCase);
                    }

                    foreach (var attr in method.GetCustomAttributes(typeof(LibraryImportAttribute), inherit: false))
                    {
                        var lib = ((LibraryImportAttribute)attr).LibraryName;
                        Assert.DoesNotContain("libscifor", lib, StringComparison.OrdinalIgnoreCase);
                        Assert.DoesNotContain("scifor", lib, StringComparison.OrdinalIgnoreCase);
                    }
                }
            }
        }
    }
}
