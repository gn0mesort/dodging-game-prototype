using Godot;
using System;

public class VerticalMoverObstacle : KinematicBody {
  [Export]
  public Vector3 MovementExtents { get; set; } = new Vector3(0f, 12f, 0f);
  [Export]
  public Vector3 BaseSpeed { get; set; } = new Vector3(0f, 6f, 0f);


  private Vector3 _velocity = new Vector3();
  private Tween _tween = null;

  public override void _Ready() {
    _tween = GetNode<Tween>("Tween");
    _velocity = BaseSpeed;
  }

  public override void _PhysicsProcess(float delta) {
    if (Mathf.IsEqualApprox(Mathf.Abs(Translation.y), MovementExtents.y, 0.2f))
    {
      _velocity *= -1f;
      var end = Mathf.IsEqualApprox(RotationDegrees.x, 180f) ? 0f : -180f;
      _tween.InterpolateProperty(this, "rotation_degrees:x", RotationDegrees.x, end, 1f,
                                 Tween.TransitionType.Linear, Tween.EaseType.InOut);
      _tween.Start();
    }
    MoveAndCollide(_velocity * delta);
  }
}
