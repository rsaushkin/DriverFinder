using NUnit.Framework;
using System.Linq;
using DriverLocator.Models;
using DriverLocator.Algorithms;

namespace DriverLocator.Tests;

[TestFixture]
public class FinderTests
{
    [Test]
    public void AllFinders_ReturnSameResult()
    {
        var registry = new Driver(10, 10);
        registry.AddOrUpdateDriver(1, 0, 0);
        registry.AddOrUpdateDriver(2, 1, 1);
        registry.AddOrUpdateDriver(3, 2, 2);
        registry.AddOrUpdateDriver(4, 3, 3);
        registry.AddOrUpdateDriver(5, 4, 4);
        registry.AddOrUpdateDriver(6, 9, 9);

        var expected = new[] { 1, 2, 3, 4, 5 };
        var finders = new INearestDriver[]
        {
            new SimpleSort(),
            new PriorityQueue(),
            new KDTree()
        };

        foreach (var finder in finders)
        {
            var result = finder.FindNearest(registry, 0, 0, 5);
            var actual = result.Select(r => r.driverId).ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }
    }

    [Test]
    public void HandlesFewerThanFiveDrivers()
    {
        var registry = new Driver(5, 5);
        registry.AddOrUpdateDriver(10, 1, 1);
        registry.AddOrUpdateDriver(20, 2, 2);

        var result = new SimpleSort().FindNearest(registry, 0, 0, 5);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].driverId, Is.EqualTo(10));
    }
}