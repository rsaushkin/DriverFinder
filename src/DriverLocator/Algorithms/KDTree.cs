using System;
using System.Collections.Generic;
using System.Linq;
using DriverLocator.Models;

namespace DriverLocator.Algorithms;

class KDNode
{
    public int DriverId { get; }
    public int X { get; }
    public int Y { get; }
    public KDNode? Left { get; set; }
    public KDNode? Right { get; set; }

    public KDNode(int driverId, int x, int y)
    {
        DriverId = driverId;
        X = x;
        Y = y;
    }
}

public class KDTree : INearestDriver
{
    private KDNode? _root;

    public List<(int driverId, double distance)> FindNearest(
        Driver registry, int orderX, int orderY, int count = 5)
    {
        // Собираем все точки
        var points = registry.GetAllDrivers()
            .Select(kv => new KDNode(kv.Key, kv.Value.x, kv.Value.y))
            .ToList();

        if (points.Count == 0) 
            return new();

        // Строим дерево
        _root = BuildTree(points, depth: 0);

        // Собираем ВСЕ точки из дерева
        var allPoints = new List<KDNode>();
        CollectAll(_root, allPoints);

        // Сортируем по расстоянию до заказа
        var result = allPoints
            .Select(node => (
                driverId: node.DriverId,
                distance: Math.Sqrt(Math.Pow(node.X - orderX, 2) + Math.Pow(node.Y - orderY, 2))
            ))
            .OrderBy(x => x.distance)
            .ThenBy(x => x.driverId) // для стабильности
            .Take(count)
            .ToList();

        return result;
    }

    private KDNode? BuildTree(List<KDNode> points, int depth)
    {
        if (points.Count == 0) return null;

        bool useX = depth % 2 == 0;
        points.Sort((a, b) => useX ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y));

        int mid = points.Count / 2;
        var node = points[mid];
        node.Left = BuildTree(points.Take(mid).ToList(), depth + 1);
        node.Right = BuildTree(points.Skip(mid + 1).ToList(), depth + 1);
        return node;
    }

    private void CollectAll(KDNode? node, List<KDNode> list)
    {
        if (node == null) return;
        list.Add(node);
        CollectAll(node.Left, list);
        CollectAll(node.Right, list);
    }
}