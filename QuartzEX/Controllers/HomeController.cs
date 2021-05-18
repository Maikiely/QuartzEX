using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quartz;
using QuartzEX.Jobs;
using QuartzEX.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzEX.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        IScheduler _schedule;

        public HomeController(IScheduler schedule)
        {
            _schedule = schedule;
        }

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> StartSimpleJob()
        {
            IJobDetail job = JobBuilder.Create<SingleJob>()
                .UsingJobData("username", "devhow")
                .UsingJobData("password", "Security!!")
                .WithIdentity("simpleJob", "qurtzexamples")
                .StoreDurably()
                .Build();

            job.JobDataMap.Put("user", new JobUserParameter { Username = "devhow", Password = "Security!!" });

            //Save the job
            await _schedule.AddJob(job, true);

            // USE CRON SCHEDULE
            //ITrigger trigger = TriggerBuilder.Create()
            //  .ForJob(job)
            //  .UsingJobData("triggerparam", "Simple trigger 1 Parameter")
            //  .WithIdentity("testtrigger", "qurtzexamples")
            //  .StartNow()
            //  .WithCronSchedule("0 0/1 * 1/1 * ? *")
            //  .Build();

            //await _schedule.ScheduleJob(trigger);

            // USE CALENDAR INTERVIL
            //ITrigger trigger = TriggerBuilder.Create()
            //   .ForJob(job)
            //   .UsingJobData("triggerparam", "Simple trigger 1 Parameter")
            //   .WithIdentity("testtrigger", "qurtzexamples")
            //   .StartNow()
            //   .WithCalendarIntervalSchedule(x => x.WithIntervalInDays(1)
            //                                       .PreserveHourOfDayAcrossDaylightSavings(true)
            //                                       .SkipDayIfHourDoesNotExist(true))
            //   .Build();

            //await _schedule.ScheduleJob(trigger);

            //ITrigger trigger = TriggerBuilder.Create()
            //    .ForJob(job)
            //    .UsingJobData("triggerparam","Simple trigger 1 Parameter")
            //    .WithIdentity("testtrigger", "qurtzexamples")
            //    .StartNow()
            //    .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(5)).RepeatForever())
            //    .WithDailyTimeIntervalSchedule(x => x.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(15,0))
            //                                         .EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(22,0))
            //                                         .OnDaysOfTheWeek(DayOfWeek.Monday,DayOfWeek.Thursday)
            //                                         .WithIntervalInSeconds(5))
            //    .Build();

            //await _schedule.ScheduleJob(trigger);

            ITrigger trigger2 = TriggerBuilder.Create()
                .ForJob(job)
                .UsingJobData("triggerparam", "Simple trigger 2 Parameter")
                .WithIdentity("testtrigger2", "qurtzexamples")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).WithRepeatCount(5))
                .Build();

            await _schedule.ScheduleJob(trigger2);

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
