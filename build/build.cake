
var target = Argument("target", "Default");
var solutionFile = "./../src/AudioSwitcher.sln";
var buildConfiguration = "Release";
var artifactPath = "./artifacts";

Task("Build")
    .Does(() =>
{
    MSBuild(solutionFile, 
        new MSBuildSettings()
            .WithTarget("Rebuild")
            .SetConfiguration(buildConfiguration)
            .SetPlatformTarget(PlatformTarget.MSIL)
    );
   
});

Task("Restore-NuGet-Packages")
    .Does(() => 
{
    NuGetRestore(solutionFile);
});

Task("Package-Zip")
    .Does(() => 
{
    EnsureDirectoryExists(artifactPath);
    CleanDirectory(artifactPath);
    Zip("./../src/AudioSwitcher/bin/" + buildConfiguration, artifactPath + "/AudioSwitcher.zip");
});

Task("Default")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Build");

Task("Ci-Build")
    .IsDependentOn("Default")
    .IsDependentOn("Package-Zip");

RunTarget(target);