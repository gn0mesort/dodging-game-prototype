using System.Diagnostics;
using Godot;

public class GameOver : Control, IDependsOnMain {
  private Main _main = null;

  public void SetMainNode(Main main) {
    _main = main;
  }

  public Main GetMainNode() {
    return _main;
  }

  public override void _Ready() {
    Debug.Assert(_main != null);
  }

  public override void _Input(InputEvent ev) {
    if (ev.IsActionPressed("ui_accept") || ev.IsActionPressed("ui_cancel"))
    {
      _main.Scenes.LoadScene("res://scenes/TitleMenu.tscn");
    }
  }
}
