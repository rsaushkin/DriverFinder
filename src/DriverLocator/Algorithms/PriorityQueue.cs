using System;
using System.Collections.Generic;
using System.Linq;
using DriverLocator.Models;

namespace DriverLocator.Algorithms;

public class PriorityQueue : INearestDriver
{
    public List<(int driverId, double distance)> FindNearest(
        Driver registry, int orderX, int orderY, int count = 5)
    {
        var drivers = registry.GetAllDrivers();
        if (drivers.Count == 0) return new();

      
        var pq = new PriorityQueue<(int id, double dist), double>();

        foreach (var (id, pos) in drivers)
        {
            double dist = Math.Sqrt(Math.Pow(pos.x - orderX, 2) + Math.Pow(pos.y - orderY, 2));
            
            if (pq.Count < count)
            {
                pq.Enqueue((id, dist), -dist); 
            }
            else if (dist < -pq.Peek().dist) 
            {
                pq.Dequeue();
                pq.Enqueue((id, dist), -dist);
            }
        }

        var result = new List<(int, double)>();
        while (pq.Count > 0)
            result.Add(pq.Dequeue());

        result.Reverse(); 
        return result;
    }
}