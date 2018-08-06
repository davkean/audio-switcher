$tempDirectoryName = '.\__squirrel_temp__'

if ($DTE -eq $null) {
  echo "Run this from the NuGet Package Console inside VS"
  exit 1
}

mkdir -p $tempDirectoryName
rm -r -fo "$tempDirectoryName\*.nupkg"
NuGet pack .\src\AudioSwitcher.sln -OutputDirectory "$tempDirectoryName"
ls "$tempDirectoryName\*.nupkg" | %{Squirrel --releasify $_ -p .\packages}
