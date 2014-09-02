using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace JumonyMonoDemo.SimpleTodo
{

	/// <summary>
	/// Task 的摘要说明
	/// </summary>
	public class TodoTask
	{
		public int TodoTaskID { get; set; }
		public string Title { get; set; }
		public bool Completed { get; set; }

	}
}