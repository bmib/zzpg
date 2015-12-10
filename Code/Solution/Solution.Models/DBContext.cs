using System.Data.Entity;

namespace Solution.Models
{
    /// <summary>
    /// Enable-Migrations -ContextTypeName "Solution.Models.DBContext" -ProjectName "Solution.Models" -StartUpProjectName "Solution.Web" -ConnectionStringName "SolutionContext"
    /// add-migration [ver] -ProjectName "Solution.Models" -StartUpProjectName "Solution.Web" -ConnectionStringName "SolutionContext"
    /// update-database -ProjectName "Solution.Models" -StartUpProjectName "Solution.Web" -ConnectionStringName "SolutionContext"
    ////update-database 失败处理方式：
    //删除数据库
    //删除 Migrations 文件夹内除 Configuration.cs 的所有文件
    //执行 add-migration 后更新 update-database ，可重新创建数据库表
    //
    //
    //项目无法加载，注释掉 CEAIR.Models.csproj
    //<Import Project="$(SolutionDir)\.nuget\nuget.targets" />
    /// </summary>
    public class DBContext: DbContext
    {
        static DBContext()
        {
            Database.SetInitializer<DBContext>(null);
        }

        public DBContext()
            : base("Name=SolutionContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Company> Company { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<User> User { get; set; }
    }
}
