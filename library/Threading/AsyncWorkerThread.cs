using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace SparkLib
{
    public class AsyncWorkerThread
    {
        private CancellationTokenSource cancelTokenSource = new();
        private Thread mainThread;
        private ManualResetEvent triggerWork = new(false);
        private ConcurrentQueue<Action> actionQueue = new();
        private ConcurrentQueue<Func<Task>> actionAsyncQueue = new();
        private List<Task> taskList = new();
        private ThreadStatus threadState = ThreadStatus.prestart;
        public ThreadStatus ThreadState
        {
            get => threadState;
        }

        public enum ThreadStatus { running, paused, cancelled, prestart };

        public AsyncWorkerThread()
        {
            mainThread = new(MainThread);
            mainThread.IsBackground = true;
            mainThread.Start();
        }

        ~AsyncWorkerThread()
        {
            cancelTokenSource.Dispose();
        }

        private void MainThread()
        {
            var cancelToken = cancelTokenSource.Token;
            while (!cancelToken.IsCancellationRequested)
            {
                threadState = ThreadStatus.running;
                triggerWork.Reset();
                while (actionQueue.Count > 0)
                {
                    Action temp;
                    if (actionQueue.TryDequeue(out temp)) taskList.Add(WorkWrapper(temp));
                }

                while (actionAsyncQueue.Count > 0)
                {
                    Func<Task> temp;
                    if (actionAsyncQueue.TryDequeue(out temp)) taskList.Add(WorkWrapper(temp));
                }

                if (taskList.Count > 0)
                {
                    var taskArray = taskList.ToArray();
                    if (Task.WhenAny(taskArray).IsCompleted)
                    {
                        List<Task> completed = new();
                        foreach (Task t in taskArray)
                        {
                            if (t.IsCompleted)
                            {
                                completed.Add(t);
                                taskList.Remove(t);
                            }
                        }
                        Console.WriteLine($"{completed.Count} tasks completed. ");
                    }
                }
                threadState = ThreadStatus.paused;
                triggerWork.WaitOne();
            }
            threadState = ThreadStatus.cancelled;
            return;
        }

        private async Task WorkWrapper(Action job)
        {
            await Task.Run(job);
            triggerWork.Set();
        }

        private async Task WorkWrapper(Func<Task> job)
        {
            await job();
            triggerWork.Set();
        }

        public void SubmitJob(Action job)
        {
            actionQueue.Enqueue(job);
            triggerWork.Set();
        }

        public void SubmitAsyncJob(Func<Task> job)
        {
            actionAsyncQueue.Enqueue(job);
            triggerWork.Set();
        }

        /// <summary>
        /// Warning: Once the thread is cancelled, it will
        /// finish and cannot be undone.
        /// Make a new AsyncWorker to start a new thread.
        /// </summary>
        public void CancelThread()
        {
            cancelTokenSource.Cancel();
            triggerWork.Set();
        }
    }
}