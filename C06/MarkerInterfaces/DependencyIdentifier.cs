using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace MarkerInterfaces
{
    public class DependencyIdentifier
    {
        public class CodeSmell : DependencyIdentifier
        {
            private readonly IServiceProvider _serviceProvider;

            public CodeSmell()
            {
                _serviceProvider = new ServiceCollection()
                    .AddSingleton<IStrategyA, StrategyA>()
                    .AddSingleton<IStrategyB, StrategyB>()
                    .AddSingleton<Consumer>()
                    .BuildServiceProvider();
            }

            [Fact]
            public void ConsumerTest()
            {
                var consumer = _serviceProvider.GetService<Consumer>();
                Assert.IsType<StrategyA>(consumer.StrategyA);
                Assert.IsType<StrategyB>(consumer.StrategyB);
            }

            public interface IStrategyA : IStrategy { }
            public interface IStrategyB : IStrategy { }

            public class StrategyA : IStrategyA
            {
                public string Execute() => "StrategyA";
            }

            public class StrategyB : IStrategyB
            {
                public string Execute() => "StrategyB";
            }

            public class Consumer
            {
                public IStrategyA StrategyA { get; }
                public IStrategyB StrategyB { get; }

                public Consumer(IStrategyA strategyA, IStrategyB strategyB)
                {
                    StrategyA = strategyA ?? throw new ArgumentNullException(nameof(strategyA));
                    StrategyB = strategyB ?? throw new ArgumentNullException(nameof(strategyB));
                }
            }
        }

        public class FixedUsage : DependencyIdentifier
        {
            public class UseCase1 : FixedUsage
            {
                private readonly IServiceProvider _serviceProvider;

                public UseCase1()
                {
                    _serviceProvider = new ServiceCollection()
                        .AddSingleton<StrategyA>()
                        .AddSingleton<StrategyB>()
                        .AddSingleton(serviceProvider =>
                        {
                            var strategyA = serviceProvider.GetService<StrategyA>();
                            var strategyB = serviceProvider.GetService<StrategyB>();
                            return new Consumer(strategyA, strategyB);
                        })
                        .BuildServiceProvider();
                }

                [Fact]
                public void ConsumerTest()
                {
                    var consumer = _serviceProvider.GetService<Consumer>();
                    Assert.IsType<StrategyA>(consumer.StrategyA);
                    Assert.IsType<StrategyB>(consumer.StrategyB);
                }
            }

            public class UseCase2 : FixedUsage
            {
                private readonly IServiceProvider _serviceProvider;

                public UseCase2()
                {
                    _serviceProvider = new ServiceCollection()
                        .AddSingleton<StrategyA>()
                        .AddSingleton<StrategyB>()
                        .AddSingleton(serviceProvider =>
                        {
                            var strategyA = serviceProvider.GetService<StrategyA>();
                            var strategyB = serviceProvider.GetService<StrategyB>();
                            return new Consumer(strategyB, strategyA);
                        })
                        .BuildServiceProvider();
                }

                [Fact]
                public void ConsumerTest()
                {
                    var consumer = _serviceProvider.GetService<Consumer>();
                    Assert.IsType<StrategyB>(consumer.StrategyA);
                    Assert.IsType<StrategyA>(consumer.StrategyB);
                }
            }

            public class StrategyA : IStrategy
            {
                public string Execute() => "StrategyA";
            }

            public class StrategyB : IStrategy
            {
                public string Execute() => "StrategyB";
            }

            public class Consumer
            {
                public IStrategy StrategyA { get; }
                public IStrategy StrategyB { get; }

                public Consumer(IStrategy strategyA, IStrategy strategyB)
                {
                    StrategyA = strategyA ?? throw new ArgumentNullException(nameof(strategyA));
                    StrategyB = strategyB ?? throw new ArgumentNullException(nameof(strategyB));
                }
            }
        }

        public interface IStrategy
        {
            string Execute();
        }
    }
}
