@ECHO OFF

SET /p "MIGRATION_NAME=Migration Name?: "
SET "COMMAND=dotnet ef migrations add %MIGRATION_NAME% -p %~dp0..\..\src\Acme.Infrastructure -s %~dp0..\..\src\Acme.Presentation.Website -o EntityFrameworkCore\Migrations --context EntityFrameworkCoreContext"
ECHO %COMMAND%

CALL %COMMAND%

pause