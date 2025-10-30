
# download dotnet-sdk.zip to ZMake/resources/dotnet

$dotnetVersion = "9.0.306"

$dotnetFileSuffix = "tar.gz"

if ($IsWindows) {
    $dotnetOs = "win"
    $dotnetFileSuffix = "zip"
}
elseif ($IsLinux) {
    $dotnetOs = "linux"
}
elseif ($IsMacOS) {
    $dotnetOs = "osx"
}
else {
    throw "Unsupported operating system"
}

$dotnetArch = [System.Runtime.InteropServices.RuntimeInformation]::OSArchitecture

switch ($dotnetArch) {
    'Arm64' {
        $dotnetArch = "arm64"
    }
    'X64' {
        $dotnetArch = "x64"
    }
    'X86' {
        $dotnetArch = "x86"
    }
    Default {
        throw "Unsupported architecture: $dotnetArch"
    }
}

$dotnetUrl = "https://builds.dotnet.microsoft.com/dotnet/Sdk/$dotnetVersion/dotnet-sdk-$dotnetVersion-$dotnetOs-$dotnetArch.$dotnetFileSuffix"

New-Item -ItemType Directory -Path ZMake/resources/ -ErrorAction Ignore
Write-Host "Downloading dotnet-sdk from $dotnetUrl to ZMake/resources/dotnet"
Invoke-WebRequest -Uri $dotnetUrl -OutFile "ZMake/resources/dotnet"
