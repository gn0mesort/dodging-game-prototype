using Godot;
using System;

public class CubeObstacleHorizontalMover : KinematicBody {
  [Export]
  public Vector3 MovementExtents { get; set; } = new Vector3(12f, 0f, 0f);
  [Export]
  public Vector3 BaseSpeed { get; set; } = new Vector3(6f, 0f, 0f);


  private Vector3 _velocity = new Vector3();
  private Tween _tween = null;

  public override void _Ready() {
    _tween = GetNode<Tween>("Tween");
    _velocity = BaseSpeed;
  }

  public override void _PhysicsProcess(float delta) {
    if (Mathf.IsEqualApprox(Mathf.Abs(Translation.x), MovementExtents.x, 0.2f))
    {
      var start = RotationDegrees.z;
      var end = RotationDegrees.z + 180;
      _velocity *= -1f;
      _tween.InterpolateProperty(this, "rotation_degrees:z", start, end, 1f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
      _tween.Start();


    }
    MoveAndCollide(_velocity * delta);
  }
}
