using Quartz;
using QuartzEX.Models;
using QuartzEX.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzEX.Jobs
{
    public  class SingleJob : IJob
    {
        IEmailService _emailService;

        public SingleJob(IEmailService emailService)
        {
            _emailService = emailService;
        } 

        public async Task Execute(IJobExecutionContext context)
        {
            //_emailService.Send("info@devhow.net", "DI", "Dependency injection in quartz jobs.");

            //JobDataMap dataMap = context.JobDetail.JobDataMap;
            JobDataMap dataMap = context.MergedJobDataMap;
            string username = dataMap.GetString("username");
            string password = dataMap.GetString("password");
            string triggerparam = dataMap.GetString("triggerparam");

            JobUserParameter user = (JobUserParameter)dataMap.Get("user");

            var message = $"Sinple executed with username {user.Username} and password {user.Password} with trigger param '{triggerparam}'";
            Debug.WriteLine(message);
        }
    }
}
