using System;
using System.Threading;

// Define a delegate for stopwatch events
public delegate void StopwatchEventHandler(string message);

class Stopwatch
{
    private int timeElapsed;
    public bool IsRunning { get; private set; }

    // Events
    public event StopwatchEventHandler? OnStarted;
    public event StopwatchEventHandler? OnStopped;
    public event StopwatchEventHandler? OnReset;

    public void Start()
    {
        if (!IsRunning)
        {
            IsRunning = true;
            OnStarted?.Invoke("Stopwatch Started!");
        }
    }

    public void Stop()
    {
        if (IsRunning)
        {
            IsRunning = false;
            OnStopped?.Invoke($"Stopwatch Stopped! Total Time: {timeElapsed} seconds");
        }
    }

    public void Reset()
    {
        timeElapsed = 0;
        OnReset?.Invoke("Stopwatch Reset!");
    }

    public void Tick()
    {
        if (IsRunning)
        {
            timeElapsed++;
            Console.WriteLine($"Time Elapsed: {timeElapsed} seconds");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();

        // Subscribe to events
        stopwatch.OnStarted += MessageHandler;
        stopwatch.OnStopped += MessageHandler;
        stopwatch.OnReset += MessageHandler;

        while (true)
        {
            Console.WriteLine("Enter S to Start, T to Stop, R to Reset, Q to Quit:");
            string? input = Console.ReadLine()?.ToUpper();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Invalid input! Try again.");
                continue;
            }

            switch (input)
            {
                case "S":
                    stopwatch.Start();
                    while (stopwatch.IsRunning)
                    {
                        Thread.Sleep(1000);
                        stopwatch.Tick();

                        if (Console.KeyAvailable)
                        {
                            input = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                            if (input == "T")
                                stopwatch.Stop();
                            else if (input == "R")
                                stopwatch.Reset();
                            else if (input == "Q")
                                return;
                        }
                    }
                    break;
                case "R":
                    stopwatch.Reset();
                    break;
                case "Q":
                    return;
                default:
                    Console.WriteLine("Invalid input! Try again.");
                    break;
            }
        }
    }

    // Event handler method
    static void MessageHandler(string message)
    {
        Console.WriteLine(message);
    }
}
