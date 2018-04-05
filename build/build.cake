var solutionFile = "./../src/AudioSwitcher.sln";

Task("Build")
    .Does(() =>
{
    MSBuild(solutionFile, 
        new MSBuildSettings()
            .WithTarget("Rebuild")
            .SetConfiguration("Release")
            .SetPlatformTarget(PlatformTarget.MSIL)
    );
   
});

Task("Restore-NuGet-Packages")
    .Does(() => 
{
    NuGetRestore(solutionFile);
});

Task("Default")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Build");

RunTarget("Default");