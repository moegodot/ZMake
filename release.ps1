Remove-Item -Recurse -Force release -ErrorAction Ignore
New-Item -ItemType Directory -Path release -ErrorAction Ignore

if ($IsWindows) {
    $dotnetOs = "win"
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

dotnet publish ZMake/ZMake.csproj -c Release -r $dotnetOs-$dotnetArch -p:IS_ZMAKE_BOOTSTRAP_MODE=true -o release
