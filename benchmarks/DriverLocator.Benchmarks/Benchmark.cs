using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using DriverLocator.Models;
using DriverLocator.Algorithms;
using System;

[MemoryDiagnoser]
[RankColumn]
public class FinderBenchmark
{
    private Driver _registry = null!;
    private int _orderX, _orderY;
    private readonly Random _rnd = new(42);

    [Params(100, 1000, 10000)]
    public int DriverCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        const int width = 1000, height = 1000;
        _registry = new Driver(width, height);
        for (int i = 0; i < DriverCount; i++)
        {
            int x = _rnd.Next(width);
            int y = _rnd.Next(height);
            _registry.AddOrUpdateDriver(i, x, y);
        }
        _orderX = _rnd.Next(width);
        _orderY = _rnd.Next(height);
    }

    [Benchmark]
    public object SimpleSort() => new SimpleSort().FindNearest(_registry, _orderX, _orderY);

    [Benchmark]
    public object PriorityQueue() => new PriorityQueue().FindNearest(_registry, _orderX, _orderY);

    [Benchmark]
    public object KDTree() => new KDTree().FindNearest(_registry, _orderX, _orderY);
}