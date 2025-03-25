## Run HelloSudoku on all devices

- Device SilveroDev
- Device VIVOTIX
- Device WSL Ubuntu 24.04.1
- Device Strato - Ubuntu 24.04.1 LTS (GNU/Linux 6.8.0-52-generic x86_64)
- Device Docker Container
- Device Linux-X64
- Device Win-X64

### Device SilveroDev

- Get-ComputerInfo
- Get-NetIPAddress

| Property | Value |
| ----------- | ----------- |
| CsManufacturer | innotek GmbH |
| CsModel | VirtualBox |
| CsName | SILVERODEV |
| CsProcessors | 12th Gen Intel(R) Core(TM) i7-12650H |
| CsSystemFamily | Virtual Machine |
| CsSystemType | x64-based PC |
| OsName | Microsoft Windows 11 Home |
| OsType | WINNT |
| OsVersion | 10.0.26100 |
| OsArchitecture | 64 bits |
| IPv4 | 10.0.2.15 |

#### Setup Machine
- Install PowerShell
- Setup SSH keys
- Install Git
- Install DotNet
- Install VSCode

#### Execute HelloSudoku Demo

1. cd GitHub
2. git clone git@github.com:SilveroLavall/DreamOn.HelloSudoku.git
3. cd DreamOn.HelloSudoku/DreamOn.HelloSudoku
4. dotnet run

## Run HelloSudoku on VIVOTIX

### Device VIVOTIX

- Get-ComputerInfo
- Get-NetIPAddress

| Property | Value |
| ----------- | ----------- |
| CsManufacturer | ASUSTeK COMPUTER INC. |
| CsModel | Vivobook_ASUSLaptop K3605ZU_K3605ZU |
| CsName | VIVOTIX |
| CsProcessors | 12th Gen Intel(R) Core(TM) i7-12650H |
| CsSystemFamily | Vivobook |
| CsSystemType | x64-based PC |
| OsName | Microsoft Windows 11 Home |
| OsType | WINNT |
| OsVersion | 10.0.26100 |
| OsArchitecture | 64 bits |
| IPv4 | 192.168.56.1 |
| IPv4 | 192.168.1.93 |
| IPv4 | 169.254.108.191 |
| IPv4 | 169.254.51.133 |
| IPv4 Wi-Fi | 169.254.29.134 |

#### Setup Machine
- Install PowerShell
- Setup SSH keys
- Install Git
- Install DotNet
- Install VSCode

#### Execute HelloSudoku Demo

1. cd GitHub
2. git clone git@github.com:SilveroLavall/DreamOn.HelloSudoku.git
3. cd DreamOn.HelloSudoku/DreamOn.HelloSudoku
4. dotnet run

## Run HelloSudoku on WSL Ubuntu 24.04.1

### Device WSL Ubuntu 24.04.1

- lshw

| Property | Value |
| ----------- | ----------- |
| CsName | VIVOTIX |
| CsProcessors | 12th Gen Intel(R) Core(TM) i7-12650H |
| OsName | Ubuntu 24.04.1 LTS |
| OsType | Ubuntu |
| OsArchitecture | 64 bits |
| Vendor | Red Hat, Inc. |
| IPv4 | 172.24.5.206 |

#### Setup Machine
- Setup SSH keys
- Install Git
- Install DotNet

#### Execute HelloSudoku Demo

1. cd GitHub
2. git clone git@github.com:SilveroLavall/DreamOn.HelloSudoku.git
3. cd DreamOn.HelloSudoku/DreamOn.HelloSudoku
4. vi DreamOn.HelloSudoku.csproj
5. dotnet run

## Run HelloSudoku on Strato - Ubuntu 24.04.1 LTS (GNU/Linux 6.8.0-52-generic x86_64)

- ssh -v root@87.106.173.246

### Device Strato - Ubuntu 24.04.1 LTS (GNU/Linux 6.8.0-52-generic x86_64)

- lshw

| Property | Value |
| ----------- | ----------- |
| CsName | vigilant-proskuriakova |
| CsProcessors | AMD EPYC-Milan Processor |
| OsName | Ubuntu 24.04.1 LTS |
| OsType | Ubuntu |
| OsArchitecture | 64 bits |
| Vendor | QEMU |
| IPv4 | 87.106.173.246 |

#### Setup Machine
- Setup SSH keys
- Install Git
- Install DotNet

#### Execute HelloSudoku Demo

1. cd GitHub
2. git clone git@github.com:SilveroLavall/DreamOn.HelloSudoku.git
3. cd DreamOn.HelloSudoku/DreamOn.HelloSudoku
4. vi DreamOn.HelloSudoku.csproj
5. dotnet run

## Run HelloSudoku on Docker Container

### Device Docker Container

#### Setup Machine
- Setup SSH keys
- Install Git
- Install DotNet
- Install Docker

#### Execute HelloSudoku Demo

1. cd GitHub/DreamOn.HelloSudoku/DreamOn.HelloSudoku
2. dotnet publish DreamOn.HelloSudoku.csproj -t:PublishContainer -p:EnableSdkContainerSupport=true
3. docker run dreamon-hellosudoku

## Run HelloSudoku on Linux-x64

### Device Linux-x64

#### Setup Machine
- Setup SSH keys
- Install Git
- Install DotNet

#### Execute HelloSudoku Demo

1. cd GitHub/DreamOn.HelloSudoku/DreamOn.HelloSudoku
2. dotnet publish DreamOn.HelloSudoku.csproj -r linux-x64
3. cd bin/Release/net8.0/linux-x64/publish/
4. ./DreamOn.HelloSudoku

## Run HelloSudoku on Win-x64

### Device Win-x64

#### Setup Machine
- Setup SSH keys
- Install Git
- Install DotNet

#### Execute HelloSudoku Demo

1. cd GitHub/DreamOn.HelloSudoku/DreamOn.HelloSudoku
2. dotnet publish DreamOn.HelloSudoku.csproj -r win-x64
3. cd bin/Release/net9.0/win-x64/publish/
4. DreamOn.HelloSudoku.exe

## Create HelloSudoku as Console

### Solution Console

#### Setup Machine
- Setup SSH keys
- Install Git
- Install DotNet
- Install Visual Studio Code

#### Create HelloSudoku Demo

1. cd GitHub/DreamOn.HelloSudoku
2. dotnet new console -o DreamOn.HelloSudokuConsole

## Create HelloSudoku as WebApi

### Solution WebApi

#### Setup Machine
- Setup SSH keys
- Install Git
- Install DotNet
- Install Visual Studio Code

#### Create HelloSudoku Demo

1. cd GitHub/DreamOn.HelloSudoku
2. dotnet new webapi -o DreamOn.HelloSudokuWebAPI
3. code .
4. dotnet run
5.
6. dotnet publish DreamOn.HelloSudokuWebAPI.csproj -t:PublishContainer -p:EnableSdkContainerSupport=true
7. docker run -p 5000:8080 dreamon-hellosudoku




    




## Setup Git & Clone Repository
- git config --global user.name "Silvero van Henningen"
- git config --global user.email silvero_vanhenningen@hotmail.com
- # SSH Key for GitHub should be configured
- cd GitHub\repos
- git clone git@github.com:SilveroLavall/DreamOn.HelloSudoku.git

## Install Tools on Windows
- winget install Git.Git
- winget install Microsoft.DotNet.SDK.9
- winget install Microsoft.PowerShell
- winget install Microsoft.VisualStudioCode

## Install Tools on Ubuntu
### Upgrade applications
- apt list --upgradable
- sudo apt upgrade
### Shutdown Restart
- reboot
### Install
- apt install dotnet-sdk-8.0





## Run HelloSudoku on all devices
- Windows 11 ARM64
- macOS Sequola 15.0.1 Apple M3 Pro
- Ubuntu 24.xx ARM64
### Windows 11 ARM64
- Install Git
- Install DotNet
### macOS Sequola 15.0.1 Apple M3
- Install Git
- Install DotNet
|||
cd sources
git clone https://github.com/SilveroLavall/SudokuAspire.git SudokuAspire
cd SudokuAspire/DreamOn.Sudoku.Solver
dotnet run
|||
