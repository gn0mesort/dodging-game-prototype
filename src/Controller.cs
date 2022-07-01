using Godot;
using System;

public class Controller : Reference {
  public enum Control : int {
    NONE = 0,
    UP,
    LEFT,
    DOWN,
    RIGHT,
    MAX
  }

  private readonly ulong[] _controls = new ulong[(int) Control.MAX];

  public static Control ToControlIf(bool condition, Control control) {
    return (Control) ((~Convert.ToInt32(condition) + 1) & (int) control);
  }

  public void SetControl(Control control) {
    _controls[(int) control] = OS.GetTicksMsec();
  }

  public void SetControlIf(bool condition, Control control) {
    // Explicit two's complement to make C# happy.
    // Bit fiddling is too "error prone" apparently.
    _controls[(int) control] = (~Convert.ToUInt64(condition) + 1) & OS.GetTicksMsec();
  }

  public void ClearControl(Control control) {
    _controls[(int) control] = 0;
  }

  public void ClearControlIf(bool condition, Control control) {
    // Same explicit two's complement as above.
    _controls[(int) control] = (~Convert.ToUInt64(!condition) + 1) & _controls[(int) control];
  }

  public ulong GetControlTimestamp(Control control) {
    return _controls[(int) control];
  }

  public bool IsSet(Control control) {
    return _controls[(int) control] != 0;
  }

  public bool IsClear(Control control) {
    return _controls[(int) control] == 0;
  }

  public override string ToString() {
    var res = "[ ";
    for (int i = (int) Control.NONE + 1; i < _controls.Length; ++i)
    {
      res += $"{_controls[i]}, ";
    }
    return res + "]";
  }
}
