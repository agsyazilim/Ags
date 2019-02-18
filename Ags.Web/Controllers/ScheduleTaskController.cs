using Ags.Services.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Controllers
{
    //do not inherit it from BasePublicController. otherwise a lot of extra action filters will be called
    //they can create guest account(s), etc
    public partial class ScheduleTaskController : BasePublicController
    {
        private readonly IScheduleTaskService _scheduleTaskService;

        public ScheduleTaskController(IScheduleTaskService scheduleTaskService)
        {
            this._scheduleTaskService = scheduleTaskService;
        }

        [HttpPost]
        public virtual IActionResult RunTask(string taskType)
        {
            var scheduleTask = _scheduleTaskService.GetTaskByType(taskType);
            if (scheduleTask == null)
                //schedule task cannot be loaded
                return NoContent();

            Task task = new Task(scheduleTask);
            task.Execute();

            return NoContent();
        }
    }
}