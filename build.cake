var solution = "IgnoreRemover.sln";

Task("Build").Does(() => {
    DotNetBuild(solution, settings => {
    });
});

Task("Restore").Does(() => {
    var solutions = GetFiles("./**/*.sln");
    foreach(var sol in solutions)
    {
        Information("Restoring {0}", sol);
        NuGetRestore(sol);
    }
});

var target = Argument("target", "default");
RunTarget(target);