using System;

namespace Ps3Debugger
{
  public interface IETACalculator
  {
    bool ETAIsAvailable { get; }

    DateTime ETA { get; }

    TimeSpan ETR { get; }

    void Reset();

    void Update(float progress);
  }
}
