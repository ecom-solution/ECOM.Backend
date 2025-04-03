﻿Add-Migration Initial -Context MainLoggingDbContext -OutputDir Migrations -Verbose
Update-Database -Context MainLoggingDbContext
Update-Database -Context MainLoggingDbContext -Connection "Server=localhost,1433;Database=Minibox;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"