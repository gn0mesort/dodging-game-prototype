using Godot;

public class FollowCamera : Camera {
  private Spatial _target = null;

  [Export]
  public NodePath Target { get; set; } = "";

  [Export]
  public Vector3 PositionOffset { get; set; } = new Vector3();

  [Export]
  public bool FollowRotation { get; set; } = true;

  [Export]
  public bool FollowTranslation { get; set; } = true;

  public override void _Ready() {
    _target = GetNode<Spatial>(Target);
  }

  public override void _PhysicsProcess(float delta) {
    var position = _target.Translation - PositionOffset;
    var targetPosition = new Vector3();
    var up = new Vector3(0f, 1f, 0f);
    if (FollowTranslation)
    {

    }
    LookAtFromPosition(position, targetPosition, new Vector3(0f, 1f, 0f));
    if (FollowRotation)
    {
      RotationDegrees = _target.RotationDegrees;
    }
  }
}
