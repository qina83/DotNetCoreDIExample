using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace ScopedDI
{
    class Program
    {
        static void Main(string[] args)
        {
            using(ServiceProvider container = RegisterServices())
            {
                for (int i = 0; i < 3; i++)
                {

                    using (IServiceScope scope1 = container.CreateScope())
                    {
                        IServiceA a1 = scope1.ServiceProvider.GetService<IServiceA>();
                        a1.A();
                        IServiceA a2 = scope1.ServiceProvider.GetService<IServiceA>();
                        a2.A();
                        IServiceB b1 = scope1.ServiceProvider.GetService<IServiceB>();
                        b1.B();
                    }
                    Console.WriteLine($"End of scope {i}");
                }
            }
            Console.Read();
        }


        static ServiceProvider RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<INumberService, NumberService>();
            services.AddScoped<IServiceA, ServiceA>();
            services.AddSingleton<IServiceB, ServiceB>();
            return services.BuildServiceProvider();
        }
    }
    public class ControllerX : IDisposable
    {
        private readonly IServiceA _serviceA;
        private readonly IServiceB _serviceB;
        private readonly int _n;
        private int _countm = 0;
        public ControllerX(IServiceA serviceA, IServiceB serviceB, INumberService numberService)
        {
            _n = numberService.GetNumber();
            Console.WriteLine($"ctor {nameof(ControllerX)}, {_n}");
            _serviceA = serviceA;
            _serviceB = serviceB;
        }
        public void M()
        {
            Console.WriteLine($"invoked {nameof(M)} for the { ++_countm}. time");
            _serviceA.A();
            _serviceB.B();
        }
        public void Dispose() => Console.WriteLine($"disposing {nameof(ControllerX)}, {_n}");
    }


    public interface INumberService
    {
        int GetNumber();
    }

    public class NumberService : INumberService
    {
        private int _number = 0;
        public int GetNumber() => Interlocked.Increment(ref _number);
    }

    public interface IServiceA
    {
        void A();
    }

    public class ServiceA : IServiceA, IDisposable
    {
        private int _n;
        public ServiceA(INumberService numberService)
        {
            _n = numberService.GetNumber();
            Console.WriteLine($"ctor {nameof(ServiceA)}, {_n}");
        }
        public void A() => Console.WriteLine($"{nameof(A)}, {_n}");
        public void Dispose() => Console.WriteLine($"disposing {nameof(ServiceA)}, {_n}");
    }


    public interface IServiceB
    {
        void B();
    }

    public class ServiceB : IServiceB, IDisposable
    {
        private int _n;
        public ServiceB(INumberService numberService)
        {
            _n = numberService.GetNumber();
            Console.WriteLine($"ctor {nameof(ServiceB)}, {_n}");
        }
        public void B() => Console.WriteLine($"{nameof(B)}, {_n}");
        public void Dispose() => Console.WriteLine($"disposing {nameof(ServiceB)}, {_n}");
    }
}
