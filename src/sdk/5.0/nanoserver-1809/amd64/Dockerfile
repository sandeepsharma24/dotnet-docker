# escape=`

ARG REPO=mcr.microsoft.com/dotnet/aspnet
ARG DOTNET_SDK_VERSION=5.0.100

# Installer image
FROM mcr.microsoft.com/windows/servercore:1809-amd64 AS installer
ARG DOTNET_SDK_VERSION

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

RUN `
    # Retrieve .NET SDK
    Invoke-WebRequest -OutFile dotnet.zip https://dotnetcli.azureedge.net/dotnet/Sdk/$Env:DOTNET_SDK_VERSION/dotnet-sdk-$Env:DOTNET_SDK_VERSION-win-x64.zip; `
    $dotnet_sha512 = '6836916bc6f3f9f7c183eb49fa1f6130640bcf623f310785466cb36c98f0554d656ec69ce3524f22d1e0d21528796182c7b4f0ed7c66e37cbc0ecbcd9f59bcd5'; `
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $dotnet_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    mkdir dotnet; `
    tar -C dotnet -oxzf dotnet.zip; `
    Remove-Item -Force dotnet.zip; `
    `
    # Install PowerShell global tool
    $powershell_version = '7.1.0'; `
    Invoke-WebRequest -OutFile PowerShell.Windows.x64.$powershell_version.nupkg https://pwshtool.blob.core.windows.net/tool/$powershell_version/PowerShell.Windows.x64.$powershell_version.nupkg; `
    $powershell_sha512 = 'ce3fca7de7cf45db939c7b4c251f285f515b74b5f5ee50888ef65c3f22e66866cab4759c4b1752703610eb64cc97c5d38dda5ec96c2171e2fbc9188b16eb8763'; `
    if ((Get-FileHash PowerShell.Windows.x64.$powershell_version.nupkg -Algorithm sha512).Hash -ne $powershell_sha512) { `
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; `
        exit 1; `
    }; `
    \dotnet\dotnet tool install --add-source . --tool-path \powershell --version $powershell_version PowerShell.Windows.x64; `
    \dotnet\dotnet nuget locals all --clear; `
    Remove-Item -Force PowerShell.Windows.x64.$powershell_version.nupkg; `
    Remove-Item -Path \powershell\.store\powershell.windows.x64\$powershell_version\powershell.windows.x64\$powershell_version\powershell.windows.x64.$powershell_version.nupkg -Force; `
    `
    # Delete everything in the dotnet folder that's not needed in the SDK layer but will instead be derived from base layers
    Get-ChildItem -Exclude 'LICENSE.txt','ThirdPartyNotices.txt','packs','sdk','templates','shared' -Path dotnet `
        | Remove-Item -Force -Recurse; `
    Get-ChildItem -Exclude Microsoft.WindowsDesktop.App -Path dotnet\shared | Remove-Item -Force -Recurse

# SDK image
FROM $REPO:5.0-nanoserver-1809
ARG DOTNET_SDK_VERSION

ENV `
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= `
    DOTNET_SDK_VERSION=$DOTNET_SDK_VERSION `
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true `
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip `
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-NanoServer-1809

# In order to set system PATH, ContainerAdministrator must be used
USER ContainerAdministrator
RUN setx /M PATH "%PATH%;C:\Program Files\powershell"
USER ContainerUser

COPY --from=installer ["/dotnet", "/Program Files/dotnet"]

COPY --from=installer ["/powershell", "/Program Files/powershell"]

# Trigger first run experience by running arbitrary cmd
RUN dotnet help
