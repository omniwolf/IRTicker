using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
public class HotspotsBenchmark
{
    private List<Tuple<DateTime, decimal>> priceList;
    private ConcurrentDictionary<decimal, int> concurrentDict;

    [GlobalSetup]
    public void Setup()
    {
        priceList = new List<Tuple<DateTime, decimal>>();
        var now = DateTime.Now;
        for (int i = 0; i < 2000; i++)
        {
            priceList.Add(new Tuple<DateTime, decimal>(now - TimeSpan.FromMinutes(10) + TimeSpan.FromSeconds(i * 0.3), 100 + (i % 50)));
        }

        concurrentDict = new ConcurrentDictionary<decimal, int>();
        for (int i = 0; i < 20000; i++)
        {
            concurrentDict.TryAdd(i, i);
        }
    }

    [Benchmark]
    public void Utilities_PriceColour_Like_Benchmark()
    {
        // A simplified inlined version of PriceColour focusing on DateTime.Now usage
        if (priceList.Count <= 1) return;
        var priceListFirst = priceList[0];
        var priceListLast = priceList[priceList.Count - 1];
        if (priceList.Count < 5) return;
        if (priceListLast.Item1 - priceListFirst.Item1 < TimeSpan.FromMinutes(5)) return;

        foreach (Tuple<DateTime, decimal> pricePoint in priceList)
        {
            if (pricePoint.Item1 >= DateTime.Now - TimeSpan.FromMinutes(5))
            {
                decimal lastPrice = priceList[priceList.Count - 1].Item2;
                if (lastPrice > pricePoint.Item2 * 1.01m) return;
                if (lastPrice > pricePoint.Item2 * 1.005m) return;
                if (lastPrice < pricePoint.Item2 * 0.99m) return;
                if (lastPrice < pricePoint.Item2 * 0.995m) return;
                return;
            }
        }
    }

    [Benchmark]
    public int ConcurrentDictionary_Count_Benchmark()
    {
        return concurrentDict.Count; // measures cost of Count
    }

    [Benchmark]
    public int ConcurrentDictionary_Keys_Iteration_Benchmark()
    {
        int count = 0;
        var keys = concurrentDict.Keys;
        foreach (var k in keys)
            count++;
        return count;
    }
}