using Godot;
using System;

public class FollowCamera : Camera {
  [Export]
  public NodePath Target { get; set; } = "";
  private Spatial _target = null;

  public override void _Ready() {
    SetAsToplevel(true);
    _target = Target != "" ? GetNode<Spatial>(Target) : GetParent<Spatial>();
  }

  public override void _PhysicsProcess(float delta) {
    var target = _target.Translation * new Vector3(0f, 0f, 1f);
    LookAtFromPosition(target - new Vector3(0f, 0f, -12f), target, new Vector3(0f, 1f, 0f));
  }
}
