@echo off
setlocal enabledelayedexpansion

REM Set the old and new project names
set oldProjectName=AspNetCoreBackendBase
set /p newProjectName="Enter the new project name: "

set "mainFolderPath=%cd%"
set "parentDir=%~dp0"
set "newMainFolderPath=%parentDir%%newProjectName%"

REM Step 1: Rename .csproj files before renaming the main folder
echo Renaming .csproj files...
for /r "%mainFolderPath%" %%f in (*%oldProjectName%*.csproj) do (
    set "filePath=%%f"
    set "fileName=%%~nxf"
    set "newFileName=!fileName:%oldProjectName%=%newProjectName%!"

    REM Ensure that the new file name is different before renaming
    if not "!fileName!"=="!newFileName!" (
        echo Renaming project file "!filePath!" to "!newFileName!"
        ren "!filePath!" "!newFileName!"
    ) else (
        echo No renaming needed for "!filePath!"
    )
)

REM Step 2: Rename subdirectories (API, Application, Domain, etc.)
echo Renaming subdirectories...
for %%d in (API Application Domain Infrastructure Persistence) do (
    if exist "%mainFolderPath%\%oldProjectName%.%%d" (
        echo Renaming "%oldProjectName%.%%d" to "%newProjectName%.%%d"...
        ren "%mainFolderPath%\%oldProjectName%.%%d" "%newProjectName%.%%d"
    ) else (
        echo Directory "%oldProjectName%.%%d" does not exist, skipping.
    )
)

REM Step 3: Update project references in .csproj files
echo Updating project references in .csproj files...
for /r "%mainFolderPath%" %%f in (*.csproj) do (
    set "filePath=%%f"
    echo Updating references in "!filePath!"

    REM Replace old project name with new project name in each .csproj file
    powershell -Command "(Get-Content '!filePath!') -replace '%oldProjectName%', '%newProjectName%' | Set-Content '!filePath!'"
)

REM Step 4: Rename the old .sln file (if it exists)
echo Renaming solution file if it exists...
if exist "%mainFolderPath%\%oldProjectName%.sln" (
    echo Renaming "%oldProjectName%.sln" to "%newProjectName%.sln"...
    ren "%mainFolderPath%\%oldProjectName%.sln" "%newProjectName%.sln"
) else (
    echo Solution file does not exist, skipping renaming.
)

REM Step 5: Create a new solution file if it doesn't exist
echo Checking for solution file...
if not exist "%mainFolderPath%\%newProjectName%.sln" (
    echo Creating solution file...
    pushd "%mainFolderPath%"
    dotnet new sln || (
        echo ERROR: Failed to create solution file.
        popd
        pause
        exit /b 1
    )
    echo Solution file created.
    popd
) else (
    echo Solution file already exists.
)

REM Step 6: Add projects to the solution
echo Adding projects to the solution...
pushd "%mainFolderPath%"
dotnet sln add "%mainFolderPath%\%newProjectName%.API\%newProjectName%.API.csproj"
dotnet sln add "%mainFolderPath%\%newProjectName%.Application\%newProjectName%.Application.csproj"
dotnet sln add "%mainFolderPath%\%newProjectName%.Domain\%newProjectName%.Domain.csproj"
dotnet sln add "%mainFolderPath%\%newProjectName%.Infrastructure\%newProjectName%.Infrastructure.csproj"
dotnet sln add "%mainFolderPath%\%newProjectName%.Persistence\%newProjectName%.Persistence.csproj"
popd

echo Projects added to the solution.

pause
