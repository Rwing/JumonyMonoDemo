using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.History;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumonyMonoDemo.SimpleTodo
{
	public class MySqlContext : DbContext
	{
		public MySqlContext()
			: base("MySqlContext")
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<MySqlContext, MyDbInitializer>());
		}

		public DbSet<TodoTask> TodoTasks { get; set; }
	}

	public class MySqlHistoryContext : HistoryContext
	{

		public MySqlHistoryContext(DbConnection connection, string defaultSchema)
			: base(connection, defaultSchema)
		{

		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<HistoryRow>().Property(h => h.MigrationId).HasMaxLength(100).IsRequired();
			modelBuilder.Entity<HistoryRow>().Property(h => h.ContextKey).HasMaxLength(200).IsRequired();
		}
	}

	public class MyDbInitializer : DbMigrationsConfiguration<MySqlContext>
	{
		public MyDbInitializer()
		{
			SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
			SetHistoryContextFactory("MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, schema));
			this.AutomaticMigrationsEnabled = true;
			this.AutomaticMigrationDataLossAllowed = true;
		}
	}
}
