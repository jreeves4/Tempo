using System.Diagnostics;

public class Tempo {
    // Built in stopwatch, more accurate than Datetime.now
    Stopwatch clock = Stopwatch.StartNew();
    // Queue storing our n most recent times
    Queue<double> taps = new();
    // maxTaps is length of the queue
    int maxTaps;
    // timeout val to possibly make const
    double timeoutMs;

    public Tempo(int maxTaps, double timeoutMs){
        this.maxTaps = maxTaps;
        this.timeoutMs = timeoutMs;
    }

    public double? Tap(){
        double now = this.clock.Elapsed.TotalMilliseconds;

        /*
         Queue.Count is length of a queue in C#
        If this tap is past timeout compared to most recent, clear it all
        */
        if (taps.Count > 0 && now - taps.Last() > timeoutMs) {
            taps.Clear();
        }

        //Add this tap to the queue of taps
        taps.Enqueue(now);
        /*
         Remove all old taps past the limit
         This will likely only ever be 1, but it is
         a while loop in case of odd overlaps
        */
        while (taps.Count > maxTaps){
            taps.Dequeue();
        }

        // Cant have less than 2 timestamps
        if (taps.Count < 2) return null;

        /*
         Take the 
        */
        double span = taps.Last() - taps.First();
        double avg = span / (taps.Count - 1);
        double res = 60000.0 / avg;
        return res;
    }

    public void Reset() => taps.Clear();
}

public class Program {

    // Flushes the 
    private static void Show(string text){
        Console.Write("\r" + text.PadRight(9));
    }
    public static void Main(string[] args) {
        
        int maxTapsArg = 3;
        int timeoutArg = 5000;
        // Parse cmd line args for queue length and timeout
        for (int i = 0; i < args.Length; i++){
            switch (args[i]) {
                case "--length":
                    // Clever ++i to read the value following this flag
                    maxTapsArg = int.Parse(args[++i]);
                    break;
                case "--timeout":
                    timeoutArg = int.Parse(args[++i]);
                    break;
                default:
                    Console.WriteLine("Invalid args");
                    return;
            }
        }

        var tempo = new Tempo(maxTapsArg, timeoutArg);
        Console.WriteLine("_______________\n|             |\n|  Tap Tempo  |\n|_____________|");
        Console.WriteLine(" Any key : tap");
        Console.WriteLine(" R       : reset");
        Console.WriteLine(" q | esc : quit");
        Console.WriteLine("");

        while (true){
            ConsoleKey key = Console.ReadKey(intercept: true).Key;
            if (key == ConsoleKey.Escape || key == ConsoleKey.Q) break;
            if (key == ConsoleKey.R) {
                tempo.Reset();
                Show("Reset");
                continue;
            }
            double? bpm = tempo.Tap();
            Show(bpm is null ? "..." : $"{bpm.Value:F1} bpm");
        }
    }
}