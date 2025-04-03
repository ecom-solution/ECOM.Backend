﻿Add-Migration Initial -Context MainDbContext -OutputDir Migrations -Verbose
Update-Database -Context MainDbContext
Update-Database -Context MainDbContext -Connection "Server=localhost,1433;Database=Minibox;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"