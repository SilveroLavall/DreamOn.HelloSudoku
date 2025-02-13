# Hello Sudoku

Hello Sudoku contains single threaded logic written in C# to solve sudoku puzzles.

- Repository: git@github.com:SilveroLavall/DreamOn.HelloSudoku.git

## PowerShell

- Get-ComputerInfo
- Get-NetIPAddress
- winget install Git.Git
- winget install Microsoft.DotNet.SDK.9
- winget install Microsoft.PowerShell
- winget install Microsoft.VisualStudioCode
- Setup SSH keys

## Ubuntu

- lshw
- Setup SSH keys
- Install Git
- apt list --upgradable
- sudo apt upgrade
- reboot
- apt install dotnet-sdk-8.0

## Docker

- docker ps
- docker images

## Setup Git & Clone Repository
- git config --global user.name "Silvero van Henningen"
- git config --global user.email silvero_vanhenningen@hotmail.com
- git config --list
- # SSH Key for GitHub should be configured
- cd GitHub\repos
- git clone git@github.com:SilveroLavall/DreamOn.HelloSudoku.git

## Setup SSH
- ssh-keygen -t rsa -b 4096 -C "silvero_vanhenningen@hotmail.com"
- eval "$(ssh-agent -s)"
- ssh-add ~/.ssh/id_rsa
- sudo apt install openssh-client
- sudo apt install openssh-server

## Execute HelloSudoku Demo

1. cd GitHub
2. git clone git@github.com:SilveroLavall/DreamOn.HelloSudoku.git
3. cd DreamOn.HelloSudoku/DreamOn.HelloSudoku
4. dotnet run

## Run HelloSudoku on Strato - Ubuntu 24.04.1 LTS

- ssh -v root@87.106.173.246

## Publish Container

1. dotnet publish DreamOn.HelloSudoku.csproj -t:PublishContainer -p:EnableSdkContainerSupport=true
2. docker run dreamon-hellosudoku

1. dotnet publish DreamOn.HelloSudokuWebAPI.csproj -t:PublishContainer -p:EnableSdkContainerSupport=true
2. docker run -p 5000:8080 dreamon-hellosudoku

1. dotnet publish DreamOn.HelloSudokuWorker.csproj -t:PublishContainer -p:EnableSdkContainerSupport=true
2. docker run -p dreamon-hellosudokuworker

## Publish Executable

1. dotnet publish DreamOn.HelloSudoku.csproj -r linux-x64
2. cd bin/Release/net8.0/linux-x64/publish/
3. ./DreamOn.HelloSudoku

1. dotnet publish DreamOn.HelloSudoku.csproj -r win-x64
2. cd bin/Release/net9.0/win-x64/publish/
3. DreamOn.HelloSudoku.exe
