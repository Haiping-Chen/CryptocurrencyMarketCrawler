using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;
using System.Linq;
using Quartz.Impl.Triggers;
using DotNetToolkit;
using EntityFrameworkCore.BootKit;

namespace Crawler
{
    public class SchedulerInitializer
    {
        public static void Initialize()
        {
            bool enable = bool.Parse(Database.Configuration.GetSection("Scheduler:Enable").Value);
            if (!enable) return;

            // init
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            // get a scheduler
            IScheduler sched = schedFact.GetScheduler().Result;

            TypeHelper.GetClassesWithInterface<IScheduleJob>(Database.Assemblies).ForEach(type => Schedule(sched, type, Database.Configuration));

            sched.Start();
        }

        private static void Schedule(IScheduler sched, Type type, IConfiguration config)
        {
            IScheduleJob scheduleJob = (IScheduleJob)Activator.CreateInstance(type);

            // define the job and tie it
            IJobDetail job = JobBuilder.Create(type)
                .WithIdentity(type.Name)
                .Build();

            sched.PauseJob(job.Key);

            String cron = config.GetSection($"Scheduler:Crons:{type.Name}").Value;

            // Update trigger
            if (String.IsNullOrEmpty(cron))
            {
                Console.WriteLine(scheduleJob.ToString() + " pasued.");
                (scheduleJob.ToString() + " pasued.").Log();
            }
            else
            {
                CronScheduleBuilder cronBuilder = CronScheduleBuilder.CronSchedule(cron);

                ICronTrigger cronTrigger = (ICronTrigger)TriggerBuilder.Create()
                  .WithIdentity(type.Name + "Trigger")
                  .WithSchedule(cronBuilder)
                  .ForJob(job)
                  .Build();

                IReadOnlyCollection<ITrigger> triggers = sched.GetTriggersOfJob(job.Key).Result;

                ICronTrigger currentTrigger = triggers.FirstOrDefault(x => x.GetType().Equals(typeof(CronTriggerImpl))) as ICronTrigger;

                if (currentTrigger == null)
                {
                    sched.ScheduleJob(job, cronTrigger);
                }
                else if (currentTrigger.CronExpressionString != cronTrigger.CronExpressionString)
                {
                    sched.RescheduleJob(cronTrigger.Key, cronTrigger);
                }

                sched.ResumeJob(job.Key);

                Console.WriteLine(scheduleJob.ToString() + " scheduled: " + cronTrigger.CronExpressionString);
                (scheduleJob.ToString() + " scheduled: " + cronTrigger.CronExpressionString).Log();

                try
                {
                    scheduleJob.ResumeJob();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    ex.Log();
                }
            }
        }
    }
}
