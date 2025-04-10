﻿Add-Migration InitialMainDbContext -Context MainDbContext -OutputDir Migrations -Verbose
Update-Database -Context MainDbContext
Update-Database -Context MainDbContext -Connection "Data Source=.\\SQLEXPRESS;Initial Catalog=Minibox;Encrypt=False;Persist Security Info=True;User ID=sa;Password=Cucdang9999@@;TrustServerCertificate=True;"