using System;
using System.Threading.Tasks;
using System.Collections.Generic;
#if GODOT
using GDT = Godot;
#endif
namespace SparkLib
{
#if GODOT
    [GDT.Tool]
#endif
    public class AsyncWorkerAsync
    {
        private List<Task> taskList = new();

        private void CleanJob(Task t)
        {
            taskList.Remove(t);
        }

        private async Task RunJob(Task t)
        {
            taskList.Add(t);
            await t;
            CleanJob(t);
        }
        public async void SubmitJob(Action job)
        {
            await RunJob(Task.Run(job));
        }

        public async void SubmitJob(Func<Task> job)
        {
            await RunJob(job());
        }
    }
}