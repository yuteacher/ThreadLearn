using System.Collections.Concurrent;

namespace ConsoleApp24
{
    internal class Program
    {
        private const int CollectionsNumber = 4;
        private const int Count = 10;
        static void Main(string[] args)
        {
           var cts = new CancellationTokenSource();
            Task.Run(() =>
            {
                if (Console.ReadKey().KeyChar == 'c')
                {
                    cts.Cancel();
                }
            });
            var sourceArrays = new BlockingCollection<int>[CollectionsNumber];
            for (int i = 0; i < sourceArrays.Length; i++)
            {
                sourceArrays[i] = new BlockingCollection<int>(Count);
            }
            var filter1 = new PipelineWorker<int, decimal>(sourceArrays,
                (n)=> Convert.ToDecimal(n*0.97),cts.Token,"filter1");
            var filter2 = new PipelineWorker<decimal, string>(filter1.Output,
                (n) => string.Format("--{0}--",n), cts.Token, "filter2");
            var filter3 = new PipelineWorker<string, string>(filter2.Output,
                (s)=>Console.WriteLine("the  final result is {0} on thread id is {1}",s,Thread.CurrentThread.ManagedThreadId),cts.Token,"filters");

            try
            {
                Parallel.Invoke(
                    () =>
                    {
                        Parallel.For(0, sourceArrays.Length * Count, (j, state) =>
                        {
                            if (cts.Token.IsCancellationRequested)
                            {
                                state.Stop();
                            }
                            int k = BlockingCollection<int>.TryAddToAny(sourceArrays, j);
                            if (k >= 0)
                            {
                                Console.WriteLine(" added {0} to source data on thread id {1}", j, Thread.CurrentThread.ManagedThreadId);
                                Thread.Sleep(TimeSpan.FromMilliseconds(100));
                            }

                        });
                        foreach (var output in sourceArrays)
                        {
                            output.CompleteAdding();
                        }
                    },
                    () => filter1.Run(),
                    () => filter2.Run(),
                    () => filter3.Run()
                );
            }catch(AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                {
                    Console.WriteLine(ex.Message +ex.StackTrace);
                }
            }
            if (cts.Token.IsCancellationRequested)
            {
                Console.WriteLine("Operation has been canceled! Press ENTER to exit");
            }
            else
            {
                Console.WriteLine("Press ENTER to exit");
            }
            Console.ReadLine();
        }
        class PipelineWorker<TInput, TOutput>
        {
            Func<TInput, TOutput> _processor = null;
            Action<TInput> _outputProcessor = null;
            BlockingCollection<TInput>[] _intput;
            CancellationToken _token;
            public BlockingCollection<TOutput>[] Output { get; private set; }
            public string Name {  get; private set; }
            public PipelineWorker(BlockingCollection<TInput>[] inputs, Func<TInput, TOutput> func, CancellationToken token, string name)
            {
                _intput = inputs;
                Output = new BlockingCollection<TOutput>[_intput.Length];
                for (int i = 0; i < Output.Length; i++)
                {
                    Output[i] = null == inputs[i] ? null : new BlockingCollection<TOutput>(Count);
                }
                _processor = func;
                _token = token;
                Name = name;
            }
            public PipelineWorker(BlockingCollection<TInput>[] inputs,  Action<TInput> renderer, CancellationToken token, string name)
            {
                _intput = inputs;
                _outputProcessor = renderer;
                _token = token;
                Name = name;
                Output =null;
            }
            public void Run()
            {
                Console.WriteLine($"{this.Name} is running");
                while(!_intput.All(bc =>bc.IsCompleted)&&!_token.IsCancellationRequested) 
                {
                    TInput receivedItem;
                    int i = BlockingCollection<TInput>.TryTakeFromAny(_intput, out receivedItem, 50, _token);
                    if (i >= 0)
                    {
                        if (Output != null)
                        {
                            TOutput outputItem = _processor(receivedItem);
                            BlockingCollection<TOutput>.AddToAny(Output, outputItem);
                            Console.WriteLine("{0} sent {1} to next ,on thread is {2} ", Name, outputItem, Thread.CurrentThread.ManagedThreadId);
                            Thread.Sleep(TimeSpan.FromMilliseconds(100));
                        }
                        else
                        {
                            _outputProcessor(receivedItem);
                        }

                    }
                    else
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(50));
                    }
                }
                if (Output != null)
                {
                    foreach (var output in Output)
                    {
                        output.CompleteAdding();
                    }
                }
            }
        }
    }
   
}
