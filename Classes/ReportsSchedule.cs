using Quartz;
using Quartz.Impl;

namespace ARM_Hotel.Classes
{
    public class ReportsSchedule
    {
        public static async void Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();
            IJobDetail job = JobBuilder.Create<ReportSender>().Build();
            // создаем триггер
            ITrigger trigger = TriggerBuilder.Create()
            // идентифицируем триггер с именем и группой
            .WithIdentity("trigger1", "group1")
            // запуск сразу после начала выполнения
            .StartNow()
            // настраиваем выполнение действия
            .WithSimpleSchedule(x => x
            .WithIntervalInMinutes(10)// через 10 минут
            .RepeatForever()) // бесконечное повторение
                              // создаем триггер
            .Build();
            await scheduler.ScheduleJob(job, trigger); // начинаем выполнение работы
 }
    }
}
