
1) bin -->
This is a auto-generates Folder.
when you run commands like dotnet run or dotnet build to compile files,
then the generated .exe and .dll files are stored here.
--> In short , it contain all compiled files

2) obj -->
Yeh bhi auto-generated hai, build process ke beech ki temporary/intermediate files yahan store hoti hai 

3) Properties/ → launchSettings.json — 
Yeh batata hai project kaise run hoga — kaunsa port use hoga (localhost:5080), browser automatically khulega ya nahi, environment kya hoga (Development/Production). Isme zyada chedna nahi padega abhi.

4) appsettings.json & appsettings.Development.json
-->.env and .env.production