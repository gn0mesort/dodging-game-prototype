using Godot;
using System;

public class Field : Area {
  private void _OnBodyEntered(Node body) {
    var player = body as Player;
    if (player != null)
    {
      player.SpeedMultiplier = 0.5f;
    }
  }

  private void _OnBodyExited(Node body) {
    var player = body as Player;
    if (player != null)
    {
      player.SpeedMultiplier = 1f;
    }
  }

  public override void _Ready() {
    Connect("body_entered", this, "_OnBodyEntered");
    Connect("body_exited", this, "_OnBodyExited");
  }
}
