# aspnetcore-jwt-ef-boilerplate

use 

```
Scaffold-DbContext "<sqlconn strings>" Microsoft.EntityFrameworkCore.SqlServer -f -OutputDir Models
```

for new db update and add options for context 

```
public cdexdbContext(DbContextOptions<cdexdbContext> options)
       : base(options)
        { }
```
