using Godot;

public class Cube : KinematicBody {
  public enum Axis {
    None = 0,
    X,
    Y
  }

  [Export]
  public Axis MovementAxis { get; set; } = Axis.None;

  [Export]
  public Vector3 Speed { get; set; } = new Vector3(1f, 1f, 0f);


  private Vector3 _movementBounds = new Vector3(3f, 3f, 0f);
  [Export]
  public Vector3 MovementBounds {
    get { return _movementBounds; }
    set { _UpdateMovementBounds(value); }
  }

  private void _UpdateMovementBounds(Vector3 bounds) {
    _maxPosition = _origin + bounds;
    _minPosition = _origin - bounds;
    _movementBounds = bounds;
  }

  private Vector3 _origin = new Vector3();
  private Vector3 _maxPosition = new Vector3();
  private Vector3 _minPosition = new Vector3();
  private float _direction = 1f;

  public override void _Ready() {
    _origin = Translation;
    _UpdateMovementBounds(_movementBounds);
  }


  public override void _PhysicsProcess(float delta) {
    var velocity = new Vector3();
    switch (MovementAxis)
    {
    case Axis.X:
      if (Mathf.IsEqualApprox(Translation.x, _minPosition.x) || Mathf.IsEqualApprox(Translation.x, _maxPosition.x))
      {
        _direction *= -1f;
      }
      velocity = _direction * new Vector3(1f, 0f, 0f) * Speed;
      break;
    case Axis.Y:
      if (Mathf.IsEqualApprox(Translation.y, _minPosition.y) || Mathf.IsEqualApprox(Translation.y, _maxPosition.y))
      {
        _direction *= -1f;
      }
      velocity = _direction * new Vector3(0f, 1f, 0f) * Speed;
      break;
    }
    var collision = MoveAndCollide(velocity * delta);
    // TODO: logic to handle colliding into the player.
    var clamped  = Translation;
    clamped.x = Mathf.Clamp(clamped.x, _minPosition.x, _maxPosition.x);
    clamped.y = Mathf.Clamp(clamped.y, _minPosition.y, _maxPosition.y);
    Translation = clamped;
  }
}
