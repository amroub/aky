$csprojPath = $args[0] # path to the file i.e. 'C:\Users\ben\Code\csproj powershell\MySmallLibrary.csproj'
#$newVersion = $args[1] # the build version, from VSTS build i.e. "1.1.20170323.1"

$targetVersion
#$csprojPath = "D:\GitRepos\diatly.foundation\Diatly.Foundation.CacheManager\Diatly.Foundation.CacheManager.csproj"

Write-Host "Starting process of generating new version number for the $csprojPath"

$xml=New-Object XML
$xml.Load($csprojPath)

$propertyGroup = $xml.SelectSingleNode(("//PropertyGroup[AssemblyVersion]"))

if ($propertyGroup -ne $null) 
{
    $targetVersion = $propertyGroup.AssemblyVersion;
}
else
{
    # If you have a new project and have not changed the version number the Version tag may not exist

    $propertyGroup = $xml.SelectSingleNode(("//PropertyGroup"))

    $targetVersion = "1.0.0.0"
    $fileVersionNode = $xml.CreateElement("FileVersion")
    $assembyVersionNode = $xml.CreateElement("AssemblyVersion")

    $propertyGroup.AppendChild($fileVersionNode)
    $propertyGroup.AppendChild($assembyVersionNode)

    Write-Host "Version XML tag added to the $csprojPath"

    $propertyGroup.FileVersion = $targetVersion
    $propertyGroup.AssemblyVersion = $targetVersion

	$xml.Save($csprojPath)
}

$nuPkgVersion = $xml.SelectSingleNode(("//PropertyGroup[Version]"))

if($nuPkgVersion -ne $null)
{
    $nuPkgVersion.Version = $targetVersion
}
else
{
    $propertyGroup = $xml.SelectSingleNode(("//PropertyGroup"))

    $pkgVersion = $xml.CreateElement("Version")

    $propertyGroup.AppendChild($pkgVersion)

    $propertyGroup.Version = $targetVersion
}

$xml.Save($csprojPath)

Write-Host "Assembly version set is: $targetVersion"
