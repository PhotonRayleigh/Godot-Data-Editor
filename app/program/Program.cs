using Godot;
using System;
using System.Linq;
using System.Threading;
using NetThread = System.Threading.Thread;
using System.Threading.Tasks;
//using System.Data.SQLite; // SQLite ADO.Net data provider
using Newtonsoft.Json; // Full featured JSON serializer
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
//using MySql.Data; // MySQLADO.Net data provider
using System.Xml.Linq;
using LiteDB;
// using System.IO;
using SparkLib;

public class Program : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Hello world!");
        GD.Print($"Main Thread # {System.Threading.Thread.CurrentThread.ManagedThreadId}");
        GD.Print($"user:// data directory: {OS.GetUserDataDir()}");
        return;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    const int threadCount = 100;
    bool threadsSpawned = false;
    System.Collections.Generic.List<System.Threading.Thread> threadList = new();
    ManualResetEventSlim re = new(false);

    AsyncWorkerAsync myWorker = new();

    protected void _OnThreadButtonPressed()
    {
        for (int i = 0; i < 1_000; i++)
        {
            //var t = Task.Run(() =>
            myWorker.SubmitJob(() =>
           {
               Random myRandom = new();
               int myTotal = 0;
               for (int j = 0; j < 100_000_000; j++)
               {
                   myTotal += myRandom.Next(-10, 10);
               }
               Console.WriteLine($"Thread {System.Threading.Thread.CurrentThread.ManagedThreadId}: Total = {myTotal}");
           });
        }
    }

    protected async void ThreadSpawner()
    {
        if (!threadsSpawned)
        {
            for (int i = 0; i < threadCount; i++)
            {
                System.Threading.Thread t = new(ThreadProc);
                t.IsBackground = true;
                threadList.Add(t);
                t.Start();
            }
            threadsSpawned = true;
        }
        else
        {
            re.Set();
            await Task.Delay(100);
            re.Reset();
        }

        return;

        void ThreadProc()
        {
            int threadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            int q = 0;
            while (true)
            {
                q++;
                if (!re.IsSet)
                {
                    Console.WriteLine($"Thread {threadID}: {q}");
                    re.Wait();
                }
            }
        }
    }
}
