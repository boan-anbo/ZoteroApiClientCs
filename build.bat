dotnet publish -c Release --self-contained true -r osx-x64 /p:IncludeNativeLibrariesForSelfExtract=true



dotnet publish -c Release --self-contained true -r win-x64 /p:IncludeNativeLibrariesForSelfExtract=true

 

dotnet publish -c Release --self-contained true -r linux-x64 /p:IncludeNativeLibrariesForSelfExtract=true
