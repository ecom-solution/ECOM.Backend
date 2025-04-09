﻿Add-Migration Initial -Context MainLoggingDbContext -OutputDir Migrations -Verbose
Update-Database -Context MainLoggingDbContext
Update-Database -Context MainLoggingDbContext -Connection "Data Source=.\\SQLEXPRESS;Initial Catalog=MiniboxLogging;Encrypt=False;Persist Security Info=True;User ID=sa;Password=Cucdang9999@@;TrustServerCertificate=True;"