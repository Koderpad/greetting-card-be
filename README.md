# Setup Project

## Tạo ASP.NET (.NET 7) Project và cài đặt package

1. Tạo một ASP.NET Project với lệnh
```
dotnet new webapi --use-program-main
```

2. Cài đặt các package
```
dotnet add package System.Data.SqlClient
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging.Console
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools.DotNet
```

3. Cài đặt .NET CLI
```
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef
dotnet new tool-manifest
cd .config
dotnet tool install dotnet-ef
```
4. Kiểm tra .NET CLI
```
dotnet ef
cd ../
```

## Khởi tạo model với database first

1. Tạo model
```
dotnet ef dbcontext scaffold "Server=.;Database=ModuleCard;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Entities -f -d --context-dir Contexts -c CardContext --context-namespace Contexts
```

2. Đặt ConnectionString ra ngoài Sourcecode
- Khi thực thi lệnh `dotnet build` chương trình sẽ cảnh báo:
```
To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
``` 
- Tại file cs trong mục _Context_, ta đặt ConnectionString ra khỏi source bằng cách đặt nó vào file _appsettings.json_:
```
"ConnectionStrings": {
    "SqlServerConnection": "Server=.;Database=ModuleCard;Trusted_Connection=True;TrustServerCertificate=True"
}
```

- Trong file đổi đoạn code dưới:
```
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=RAGORED;Database=ModuleCard;Trusted_Connection=True;TrustServerCertificate=True");
```

- Bằng đoạn code sau:
```
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
            var connectionString = configuration.GetConnectionString("SqlServerConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
```

## Tài liệu tham khảo

1. https://learn.microsoft.com/en-us/ef/core/cli/dotnet
2. https://stackoverflow.com/questions/67505347/non-nullable-property-must-contain-a-non-null-value-when-exiting-constructor-co
3. https://xuanthulab.net/ef-core-sinh-ra-cac-entity-tu-database-voi-cong-cu-dotnet-ef-trong-c-csharp.html
4. https://www.c-sharpcorner.com/blogs/create-apis-using-net-core-31
5. https://stackoverflow.com/questions/44865133/how-do-i-use-async-taskiactionresult-or-how-to-run-in-async-way-in-my-asp-ne
6. https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-7.0
7. https://xuanthulab.net/git-va-github/