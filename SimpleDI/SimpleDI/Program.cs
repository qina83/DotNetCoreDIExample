using Microsoft.Extensions.DependencyInjection;
using System;


namespace SimpleDI
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceProvider container = RegisterServices())
            {
                var controller = container.GetRequiredService<HomeController>();
                string result = controller.Hello("Katharina");
                Console.WriteLine(result);
                Console.Read();
            }
        }

        static ServiceProvider RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IGreetingService, GreetingService>(); //<-- as singleton
            services.AddTransient<HomeController>();    //<-- new every times
            return services.BuildServiceProvider();
        }
    }
}
