using System.Reflection;

[assembly: AssemblyTitle("Caerostris")]
[assembly: AssemblyProduct("Caerostris")]
[assembly: AssemblyVersion("1.0.*")]

#if (DEBUG)
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif