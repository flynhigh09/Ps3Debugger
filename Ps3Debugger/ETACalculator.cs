using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ps3Debugger
{
  public class ETACalculator : IETACalculator
  {
    private int minimumData;
    private long maximumTicks;
    private readonly Stopwatch timer;
    private readonly Queue<KeyValuePair<long, float>> queue;
    private KeyValuePair<long, float> current;
    private KeyValuePair<long, float> oldest;

    public TimeSpan ETR
    {
      get
      {
        KeyValuePair<long, float> keyValuePair1 = oldest;
        KeyValuePair<long, float> keyValuePair2 = current;
        if (queue.Count < minimumData || (double) keyValuePair1.Value == (double) keyValuePair2.Value)
          return TimeSpan.MaxValue;
        else
          return TimeSpan.FromSeconds((1.0 - (double) keyValuePair2.Value) * (double) (keyValuePair2.Key - keyValuePair1.Key) / ((double) keyValuePair2.Value - (double) keyValuePair1.Value) / (double) Stopwatch.Frequency);
      }
    }

    public DateTime ETA
    {
      get
      {
        return DateTime.Now.Add(ETR);
      }
    }

    public bool ETAIsAvailable
    {
      get
      {
        if (queue.Count >= minimumData)
          return (double) oldest.Value != (double) current.Value;
        else
          return false;
      }
    }

    public ETACalculator(int minimumData, double maximumDuration)
    {
      this.minimumData = minimumData;
      maximumTicks = (long) (maximumDuration * (double) Stopwatch.Frequency);
      queue = new Queue<KeyValuePair<long, float>>(minimumData * 2);
      timer = Stopwatch.StartNew();
    }

    public void Reset()
    {
      queue.Clear();
      timer.Reset();
      timer.Start();
    }

    private void ClearExpired()
    {
      long num = timer.ElapsedTicks - maximumTicks;
      while (queue.Count > minimumData && queue.Peek().Key < num)
        oldest = queue.Dequeue();
    }

    public void Update(float progress)
    {
      if ((double) current.Value == (double) progress)
        return;
      ClearExpired();
      current = new KeyValuePair<long, float>(timer.ElapsedTicks, progress);
      queue.Enqueue(current);
      if (queue.Count != 1)
        return;
      oldest = current;
    }
  }
}
