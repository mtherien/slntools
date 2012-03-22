mkdir bin
mkdir bin\Release
"C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\msbuild" SLNTools.sln /p:Configuration=Release /verbosity:normal /consoleloggerparameters:NoItemAndPropertyList /logger:FileLogger,Microsoft.Build.Engine;logfile=bin\Release\build.log;verbosity=normal /logger:FileLogger,Microsoft.Build.Engine;logfile=bin\Release\buildperf.log;verbosity=quiet;PerformanceSummary 

@echo =============================
@echo ========== Results ==========
@echo =============================
dir \s ".\bin\Release"
pause