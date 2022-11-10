using System;

public static class Utility {
  public static uint Clamp(uint x, uint min, uint max) {
    return Math.Min(Math.Max(x, min), max);
  }

  public static int Clamp(int x, int min, int max) {
    return Math.Min(Math.Max(x, min), max);
  }

  public static int Clamp(ushort x, ushort min, ushort max) {
    return Math.Min(Math.Max(x, min), max);
  }
}
