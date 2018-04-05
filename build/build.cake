Task("Build")
    .Does(() =>
{
    Information("Hello");
});


Task("Default")
    .IsDependentOn("Build");

RunTarget("Default");