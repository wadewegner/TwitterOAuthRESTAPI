tools/NuGet.exe pack src/RestAPI/RestAPI.csproj -\Symbols -OutputDirectory packages -Version "$1"

tools/NuGet.exe push packages/WadeWegner.Twitter.RestApi."$1".nupkg