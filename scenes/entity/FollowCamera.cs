using Godot;

public class FollowCamera : Camera {
  private Spatial _target = null;
  private Tween _tween = null;

  [Export]
  public NodePath Target { get; set; } = "";

  [Export]
  public float DepthOffset { get; set; } = 0f;

  [Export]
  public bool FollowRotation { get; set; } = true;

  [Export]
  public bool FollowTranslation { get; set; } = true;

  public override void _Ready() {
    _target = GetNode<Spatial>(Target);
    _tween = GetNode<Tween>("Tween");
  }

  public override void _PhysicsProcess(float delta) {
    var next = _target.Translation - new Vector3(0f, 0f, DepthOffset);
    Translation = new Vector3(Translation.x, Translation.y, next.z);
    if (FollowTranslation)
    {
      _tween.RemoveAll();
      _tween.InterpolateProperty(this, "translation", Translation, next, 0.5f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
      _tween.Start();
    }
    if (FollowRotation)
    {
      RotationDegrees = _target.RotationDegrees;
    }
  }
}
