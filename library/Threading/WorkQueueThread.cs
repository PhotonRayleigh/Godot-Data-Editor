using System;
using System.Collections.Concurrent;
using System.Threading;

namespace SparkLib
{
    public delegate void ThreadAction();
    public delegate void ThreadMethod(Object[] args);
    public delegate void ThreadFunction(ref Object output);
    public delegate void ThreadFunctionParam(Object[] args, ref Object output);

    public class WorkQueueThread
    {
        protected Thread workThread;
        protected CancellationTokenSource source = new CancellationTokenSource(); // Saw this somewhere, not sure why they used it.
                                                                                  // Using it here to remind myself to look into it.
        protected EventWaitHandle threadSuspend = new EventWaitHandle(false, EventResetMode.AutoReset);
        protected ConcurrentQueue<ThreadQueueItem> workQueue = new ConcurrentQueue<ThreadQueueItem>();

        public WorkQueueThread()
        {
            workThread = new Thread(ThreadProc);
            workThread.IsBackground = true;
            workThread.Start();
        }

        protected void ThreadProc()
        {
            while (!source.IsCancellationRequested)
            {
                while (workQueue.Count != 0)
                {
                    ThreadQueueItem workItem;
                    if (workQueue.TryDequeue(out workItem))
                    {
                        workItem.taskStatus = QueueItemStatus.InProgress;
                        workItem.CallMethod();
                        //workItem.Task.Invoke(workItem.Args!);
                        workItem.taskStatus = QueueItemStatus.Complete;
                    }
                }
                threadSuspend.WaitOne();
            }
            return;
        }

        public void Stop()
        {
            source.Cancel();
        }

        // This is the preferred version to queue work with.
        public ThreadQueueItem EnqueueWork(Object[] args, ThreadMethod task)
        {
            ThreadQueueItem newItem = new ThreadQueueItem(task, args);
            workQueue.Enqueue(newItem);
            if (workThread.ThreadState == ThreadState.WaitSleepJoin)
            {
                threadSuspend.Set();
            }
            return newItem;
        }

        public ThreadQueueItem EnqueueWork(ThreadMethod task, Object[]? args = null)
        {
            ThreadQueueItem newItem = new ThreadQueueItem(task, args);
            workQueue.Enqueue(newItem);
            if (workThread.ThreadState == ThreadState.WaitSleepJoin)
            {
                threadSuspend.Set();
            }
            return newItem;
        }

        public class ThreadQueueItem
        {
            private ThreadMethod task;
            public ThreadMethod Task
            {
                get => task;
            }
            private Object[] args;
            public Object[] Args
            {
                get => args;
            }
            internal QueueItemStatus taskStatus = QueueItemStatus.NotStarted;
            public QueueItemStatus TaskStatus
            {
                get => taskStatus;
            }

            internal ThreadQueueItem(ThreadMethod method, Object[]? args = null)
            {
                this.task = method;
                this.args = args ?? new Object[0];
            }

            public void CallMethod()
            {
                task(args);
            }
        }
        public enum QueueItemStatus { NotStarted, InProgress, Complete }
    }
}
