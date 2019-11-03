# dotnet_ScaffoldExample

## 試してみたこと

- ASP.NET Core MVC + EF Core での Scaffolding
  - Controller / View は :ok:
- dotnet script での既存の Model クラスを使っての DB 操作
  - 失敗
- rails generator と rails console に近づけられるか

## ASP.NET Core MVC + EF Core での Scaffolding

- dotnet aspnet-codegenerator で可能
  - https://docs.microsoft.com/en-us/aspnet/core/fundamentals/tools/dotnet-aspnet-codegenerator?view=aspnetcore-3.0
  - 公式の tutorial としても紹介されている
    - https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/adding-model?view=aspnetcore-3.0&tabs=visual-studio-code
- Model class だけは手書きになってしまうが、Model を別の project に分けることも多いと思うので、実際はこちらの方が便利そう
  - Rails でもだいたい Model は手書きしてた
- `Microsoft.EntityFrameworkCore.SqlServer` まで入れないと生成できないのが少し微妙
  - Scaffold だけする project を別途作ってみたが、Controller / View を作るプロジェクトに入っていないと動作しないため NG
  - シングルバイナリにする場合には考えた方がいいかも？
  
## dotnet script での既存の Model クラスを使っての DB 操作

- DbContext は Design-time DbContext で生成できる
  - https://docs.microsoft.com/en-US/ef/core/miscellaneous/cli/dbcontext-creation#from-a-design-time-factory
  - MVC project のように Startup で設定があるものはそちらを見るが、例えば Model project の方だけで migration を実行したい場合に接続設定をしたい場合にも使える
- ASP.NET Core MVC に Model も包含した状態だと、MVC 関連の package も必要なり、それを dotnet script に入れられず失敗
- Model を別の project に話した状態で dotnet script を実行したが、以下のようなエラーで DB からの読み込み部分で失敗

```
> dotnet script .\main.csx -i
> var d = new MovieContext(options.Options);
> d
[Example.Models.MovieContext]
> d.Movie.First();
System.TypeInitializationException: System.TypeInitializationException: The type initializer for 'Microsoft.Data.SqlClient.TdsParser' threw an exception.
  + Microsoft.Data.SqlClient.TdsParser..ctor(bool, bool) 場所 E:\agent1\_work\31\s\src\Microsoft.Data.SqlClient\netcore\src\Microsoft\Data\SqlClient\TdsParser.cs : 60
  + Microsoft.Data.SqlClient.SqlInternalConnectionTds.LoginNoFailover(Microsoft.Data.SqlClient.ServerInfo, string, System.Security.SecureString, bool, Microsoft.Data.SqlClient.SqlConnectionString, Microsoft.Data.SqlClient.SqlCredential, Microsoft.Data.ProviderBase.TimeoutTimer) 場所 E:\agent1\_work\31\s\src\Microsoft.Data.SqlClient\netcore\src\Microsoft\Data\SqlClient\SqlInternalConnectionTds.cs : 1409
  + Microsoft.Data.SqlClient.SqlInternalConnectionTds.OpenLoginEnlist(Microsoft.Data.ProviderBase.TimeoutTimer, Microsoft.Data.SqlClient.SqlConnectionString, Microsoft.Data.SqlClient.SqlCredential, string, System.Security.SecureString, bool) 場所 E:\agent1\_work\31\s\src\Microsoft.Data.SqlClient\netcore\src\Microsoft\Data\SqlClient\SqlInternalConnectionTds.cs : 1312
  + Microsoft.Data.SqlClient.SqlInternalConnectionTds..ctor(Microsoft.Data.ProviderBase.DbConnectionPoolIdentity, Microsoft.Data.SqlClient.SqlConnectionString, Microsoft.Data.SqlClient.SqlCredential, object, string, System.Security.SecureString, bool, Microsoft.Data.SqlClient.SqlConnectionString, Microsoft.Data.SqlClient.SessionData, bool, string, Microsoft.Data.ProviderBase.DbConnectionPool, Microsoft.Data.SqlClient.SqlAuthenticationProviderManager) 場所 E:\agent1\_work\31\s\src\Microsoft.Data.SqlClient\netcore\src\Microsoft\Data\SqlClient\SqlInternalConnectionTds.cs : 439
  + Microsoft.Data.SqlClient.SqlConnectionFactory.CreateConnection(Microsoft.Data.Common.DbConnectionOptions, Microsoft.Data.Common.DbConnectionPoolKey, object, Microsoft.Data.ProviderBase.DbConnectionPool, System.Data.Common.DbConnection, Microsoft.Data.Common.DbConnectionOptions) 場所 E:\agent1\_work\31\s\src\Microsoft.Data.SqlClient\netcore\src\Microsoft\Data\SqlClient\SqlConnectionFactory.cs : 135
  + Microsoft.Data.ProviderBase.DbConnectionFactory.CreatePooledConnection(Microsoft.Data.ProviderBase.DbConnectionPool, System.Data.Common.DbConnection, Microsoft.Data.Common.DbConnectionOptions, Microsoft.Data.Common.DbConnectionPoolKey, Microsoft.Data.Common.DbConnectionOptions) 場所 E:\agent1\_work\31\s\src\Microsoft.Data.SqlClient\netcore\src\Common\src\Microsoft\Data\ProviderBase\DbConnectionFactory.cs : 111
  + Microsoft.Data.ProviderBase.DbConnectionPool.CreateObject(System.Data.Common.DbConnection, Microsoft.Data.Common.DbConnectionOptions, Microsoft.Data.ProviderBase.DbConnectionInternal) 場所 E:\agent1\_work\31\s\src\Microsoft.Data.SqlClient\netcore\src\Microsoft\Data\ProviderBase\DbConnectionPool.cs : 695
  + Microsoft.Data.ProviderBase.DbConnectionPool.UserCreateRequest(System.Data.Common.DbConnection, Microsoft.Data.Common.DbConnectionOptions, Microsoft.Data.ProviderBase.DbConnectionInternal) 場所 E:\agent1\_work\31\s\src\Microsoft.Data.SqlClient\netcore\src\Microsoft\Data\ProviderBase\DbConnectionPool.cs : 1640
  + Microsoft.Data.ProviderBase.DbConnectionPool.TryGetConnection(System.Data.Common.DbConnection, uint, bool, bool, Microsoft.Data.Common.DbConnectionOptions, out Microsoft.Data.ProviderBase.DbConnectionInternal) 場所 E:\agent1\_work\31\s\src\Microsoft.Data.SqlClient\netcore\src\Microsoft\Data\ProviderBase\DbConnectionPool.cs : 1100
  + Microsoft.Data.ProviderBase.DbConnectionPool.TryGetConnection(System.Data.Common.DbConnection, TaskCompletionSource<Microsoft.Data.ProviderBase.DbConnectionInternal>, Microsoft.Data.Common.DbConnectionOptions, out Microsoft.Data.ProviderBase.DbConnectionInternal) 場所 E:\agent1\_work\31\s\src\Microsoft.Data.SqlClient\netcore\src\Microsoft\Data\ProviderBase\DbConnectionPool.cs : 1068
  + Microsoft.Data.ProviderBase.DbConnectionFactory.TryGetConnection(System.Data.Common.DbConnection, TaskCompletionSource<Microsoft.Data.ProviderBase.DbConnectionInternal>, Microsoft.Data.Common.DbConnectionOptions, Microsoft.Data.ProviderBase.DbConnectionInternal, out Microsoft.Data.ProviderBase.DbConnectionInternal) 場所 E:\agent1\_work\31\s\src\Microsoft.Data.SqlClient\netcore\src\Microsoft\Data\ProviderBase\DbConnectionFactory.cs : 154
  + Microsoft.Data.ProviderBase.DbConnectionInternal.TryOpenConnectionInternal(System.Data.Common.DbConnection, Microsoft.Data.ProviderBase.DbConnectionFactory, TaskCompletionSource<Microsoft.Data.ProviderBase.DbConnectionInternal>, Microsoft.Data.Common.DbConnectionOptions) 場所 E:\agent1\_work\31\s\src\Microsoft.Data.SqlClient\netcore\src\Common\src\Microsoft\Data\ProviderBase\DbConnectionInternal.cs : 328
  + Microsoft.Data.ProviderBase.DbConnectionClosed.TryOpenConnection(System.Data.Common.DbConnection, Microsoft.Data.ProviderBase.DbConnectionFactory, TaskCompletionSource<Microsoft.Data.ProviderBase.DbConnectionInternal>, Microsoft.Data.Common.DbConnectionOptions) 場所 E:\agent1\_work\31\s\src\Microsoft.Data.SqlClient\netcore\src\Common\src\Microsoft\Data\ProviderBase\DbConnectionClosed.cs : 39
  + Microsoft.Data.SqlClient.SqlConnection.TryOpen(TaskCompletionSource<Microsoft.Data.ProviderBase.DbConnectionInternal>) 場所 E:\agent1\_work\31\s\src\Microsoft.Data.SqlClient\netcore\src\Microsoft\Data\SqlClient\SqlConnection.cs : 1425    + Microsoft.Data.SqlClient.SqlConnection.Open() 場所 E:\agent1\_work\31\s\src\Microsoft.Data.SqlClient\netcore\src\Microsoft\Data\SqlClient\SqlConnection.cs : 974
  + Microsoft.EntityFrameworkCore.Storage.RelationalConnection.OpenDbConnection(bool)
  + Microsoft.EntityFrameworkCore.Storage.RelationalConnection.Open(bool)
  + Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReader(Microsoft.EntityFrameworkCore.Storage.RelationalCommandParameterObject)
  + Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor.QueryingEnumerable<T>.Enumerator.MoveNext()
  + System.Linq.Enumerable.Single<TSource>(IEnumerable<TSource>)
  + Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.Execute<TResult>(System.Linq.Expressions.Expression)       + Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider.Execute<TResult>(System.Linq.Expressions.Expression)
  + System.Linq.Queryable.First<TSource>(IQueryable<TSource>)
  ```
