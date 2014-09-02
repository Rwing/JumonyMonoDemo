using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Ivony.Html;
using Ivony.Html.Web;
using Ivony.Data;
using Ivony.Web;
using System.Threading.Tasks;
using System.IO;
//using Ivony.Data.SQLiteClient;

namespace JumonyMonoDemo.SimpleTodo
{

	public class TodoController : Controller
	{

		//private SQLiteExecutor dbUtility = SQLite.ConnectFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db.sqlite"));

		private MySqlContext dbContext = new MySqlContext();

		//public async Task<ActionResult> Init()
		//{
		//	dbUtility.T("CREATE TABLE Tasks (ID integer primary key AutoIncrement, Title varchar(50), Completed bit)").ExecuteNonQuery();

		//	return RedirectToAction("Index");
		//}

		public async Task<ActionResult> Index()
		{
			var items = dbContext.TodoTasks.ToArray();
			return View("index", items);
		}


		public async Task<ActionResult> Add(string title)
		{
			dbContext.TodoTasks.Add(new TodoTask (){
				 Title = title
			});
			dbContext.SaveChanges();
			return RedirectToAction("Index");
		}

		public async Task<ActionResult> Complete(int taskId)
		{
			var item = dbContext.TodoTasks.FirstOrDefault(i=>i.TodoTaskID == taskId);
			if(item!=null)
			{
				item.Completed = true;
				dbContext.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		public async Task<ActionResult> Revert(int taskId)
		{
			var item = dbContext.TodoTasks.FirstOrDefault(i => i.TodoTaskID == taskId);
			if (item != null)
			{
				item.Completed = false;
				dbContext.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		public async Task<ActionResult> Remove(int taskId)
		{
			var item = dbContext.TodoTasks.FirstOrDefault(i => i.TodoTaskID == taskId);
			if (item != null)
			{
				dbContext.TodoTasks.Remove(item);
				dbContext.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<ActionResult> Modify(int taskId)
		{
			var item = dbContext.TodoTasks.FirstOrDefault(i => i.TodoTaskID == taskId);
			return View("modify", item);

		}

		[HttpPost]
		public async Task<ActionResult> Modify(int taskId, string title)
		{

			if (!ViewData.ModelState.IsValid)
				return View("Index");


			var item = dbContext.TodoTasks.FirstOrDefault(i => i.TodoTaskID == taskId);
			if (item != null)
			{
				item.Title = title;
				dbContext.SaveChanges();
			}
			return RedirectToAction("Index");
		}

	}


	public class TodoCachePolicyProvider : ControllerCachePolicyProvider
	{
		public CachePolicy Index(ControllerContext context, IDictionary<string, object> args)
		{

			var token = CacheToken.FromCookies(context.HttpContext) + CacheToken.CreateToken("Index");

			return null;
			return new StandardCachePolicy(context.HttpContext, token, this, TimeSpan.FromMinutes(1), true);

		}
	}
}