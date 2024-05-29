void TaskMethod(string name)
{
    Console.WriteLine("Task {0} is running on a thread id{1}.Is thread pool thread:{2}", name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);

}
var t1 = new Task(() => TaskMethod("Task1"));
var t2 = new Task(() => TaskMethod("Task2"));
t2.Start();
t1.Start();

Task.Run(() => TaskMethod("Task3"));
Task.Factory.StartNew(() => TaskMethod("Task4"));
Task.Factory.StartNew(() => TaskMethod("Task5"),
    TaskCreationOptions.LongRunning
);
Thread.Sleep(TimeSpan.FromSeconds(1));
