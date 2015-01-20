$tempDirectoryName = '.\__squirrel_temp__'

if ($DTE -eq $null) {
  echo "Run this from the NuGet Package Console inside VS"
  exit 1
}

mkdir -Path $tempDirectoryName
rm -r -fo "$tempDirectoryName\*.nupkg"
NuGet pack .\src\AudioSwitcher\AudioSwitcher.csproj -OutputDirectory "$tempDirectoryName" -Prop Configuration=Release
ls "$tempDirectoryName\*.nupkg" | %{Squirrel --releasify $_ -p .\src\packages -r .\Releases}
rm -r -fo "$tempDirectoryName"
