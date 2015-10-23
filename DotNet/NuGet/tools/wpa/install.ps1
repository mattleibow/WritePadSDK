param($installPath, $toolsPath, $package, $project)

	$sqliteReference = $project.Object.References.AddSDK(
        "Microsoft Visual C++ 2013 Runtime Package for Windows Phone", 
        "Microsoft.VCLibs, Version=12.0")
    Write-Host "Successfully added a reference to the extension SDK 'Microsoft Visual C++ 2013 Runtime Package for Windows Phone'."
