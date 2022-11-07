using Godot;

public abstract class BoundedKinematicBody : KinematicBody {
  protected Vector2 _bounds = new Vector2();
  protected Vector2 _maxPosition = new Vector2();
  protected Vector2 _minPosition = new Vector2();

  [Export]
  public Vector3 Speed { get; set; } = new Vector3(1f, 1f, 1f);

  [Export]
  public Vector2 MovementBounds {
    get { return _bounds; }
    set { _UpdateMovementBounds(_GetOrigin(), value); }
  }

  abstract protected void _SetOrigin(Vector3 origin);
  abstract protected Vector3 _GetOrigin();

  protected void _UpdateMovementBounds(Vector3 origin, Vector2 bounds) {
    var origin2d = new Vector2(origin.x, origin.y);
    _maxPosition = origin2d + bounds;
    _minPosition = origin2d - bounds;
    _bounds = bounds;
  }

  protected Vector3 _BoundTranslation(Vector3 translation) {
    translation.x = Mathf.Clamp(translation.x, _minPosition.x, _maxPosition.x);
    translation.y = Mathf.Clamp(translation.y, _minPosition.y, _maxPosition.y);
    return translation;
  }
}
