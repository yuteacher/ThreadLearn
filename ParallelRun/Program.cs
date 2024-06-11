var firstTask = new Task<int>(() => TaskMethod("firstTask ", 3));
var secondTask = new Task<int>(() => TaskMethod("secondTask ", 3));
var whenAllTask = Task.WhenAll(firstTask, secondTask);

whenAllTask.ContinueWith(t => Console.WriteLine("Excertion caught :{0}", t.Exception), TaskContinuationOptions.OnlyOnRanToCompletion);

firstTask.Start();
secondTask.Start();

Thread.Sleep(1000);

var tasks = new List<Task<int>>();

for (int i = 0; i < 10; i++)
{
    int counter = i;
    var task = new Task<int>(() => TaskMethod(string.Format("Task {0}", counter), counter));
    tasks.Add(task);
    task.Start();
}
while (tasks.Count > 0)
{
    var completerTask =Task.WhenAny(tasks).Result;
    tasks.Remove(completerTask);
    Console.WriteLine("A Task has been completed with result {0}",completerTask.Result);
}
Thread.Sleep(1000);

static int TaskMethod(string name, int seconds)
{
    Console.WriteLine("Task{0} is running on a thread id{1}.Is thread pool thread:{2}", name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
    Thread.Sleep(TimeSpan.FromSeconds(seconds));
    throw new Exception("Boom!");
    //return 42 * seconds;
}