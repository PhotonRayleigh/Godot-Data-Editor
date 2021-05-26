using System;
using System.Threading;
using NetThread = System.Threading.Thread;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SparkLib
{
    public class AsyncWorkerSync
    {
        //private int jobsStarted = 0;
        //private int jobsCompleted = 0;
        private List<Task> taskList = new();

        public enum ThreadStatus { running, paused, cancelled, prestart };

        public AsyncWorkerSync()
        {
            //Console.WriteLine("AsyncWorkerAsync initialized");
        }

        private void CleanJob(Task t)
        {
            taskList.Remove(t);
            // Console.WriteLine($"{++jobsCompleted} jobs completed");
        }

        private void RunJob(Task t)
        {
            //Console.WriteLine($"{++jobsStarted} jobs have been started");
            // PrintThreadStart("RunJob");
            taskList.Add(t);
            t.Wait();
            CleanJob(t);
            // PrintThreadEnd("RunJob");
        }
        public void SubmitJob(Action job)
        {
            // PrintThreadStart("SubmitJob");
            RunJob(Task.Run(job));
        }

        public void SubmitJob(Func<Task> job)
        {
            RunJob(job());
        }

        // private void PrintThreadStart(string funcName)
        // {
        //     Console.WriteLine($"{funcName} called on thread {NetThread.CurrentThread.ManagedThreadId}");
        // }

        // private void PrintThreadEnd(string funcName)
        // {
        //     Console.WriteLine($"{funcName} ended on thread {NetThread.CurrentThread.ManagedThreadId}");
        // }
    }
}