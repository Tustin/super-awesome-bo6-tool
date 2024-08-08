@echo off
setlocal enabledelayedexpansion

REM Initialize flags

REM Check if the control files exist

set performInstall=0

if not exist ".conan\CONANDATA_%CONAN_BUILD_CONFIG%" set "performInstall=1"
if not exist ".conan\CONANFILE_%CONAN_BUILD_CONFIG%" set "performInstall=1"

REM Check for changes in conandata.yml and conanfile.py

if exist ".conan\CONANDATA_%CONAN_BUILD_CONFIG%" (
    echo Checking changes in conandata.yml
    fc "conandata.yml" ".conan\CONANDATA_%CONAN_BUILD_CONFIG%" > nul
    if %errorlevel% equ 1 set performInstall=1
)

if exist ".conan\CONANFILE_%CONAN_BUILD_CONFIG%" (
    echo Checking changes in conanfile.py
    fc "conanfile.py" ".conan\CONANFILE_%CONAN_BUILD_CONFIG%" > nul
    if %errorlevel% equ 1 set performInstall=1
)

if %performInstall% equ 1 (
    REM Echo changes detected
    echo Changes detected, executing conan install...
    
    set "args="

    :args_loop
    if "%~1"=="" goto after_args_loop
    set "args=!args! %1"
    shift
    goto args_loop

    :after_args_loop
    echo Arguments for conan install: !args!

    "C:\Program Files\Conan\conan\conan.exe" install !args!

    if !errorlevel! neq 0 (
        echo ERROR: Conan installation failed. Please check the console output to troubleshoot issues.
        exit /b 1
    )

    REM Update control files to reflect current state
    copy /Y conandata.yml .conan\CONANDATA_%CONAN_BUILD_CONFIG%
    copy /Y conanfile.py .conan\CONANFILE_%CONAN_BUILD_CONFIG%

    REM Display the message indicating Conan install finished
    echo ****************************************************************
    echo *                                                              *
    echo *   Conan installation completed successfully.                 *
    echo *   Please relaunch the build to apply the new changes.        *
    echo *                                                              *
    echo ****************************************************************
    echo ERROR: Conan installation completed successfully. Please relaunch the build to apply the new changes.
    exit /b 1
)

REM Echo no changes detected
echo No changes detected, skipping conan install...
