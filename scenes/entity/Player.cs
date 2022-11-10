using Godot;

public class Player : BoundedKinematicBody, IVelocityModifiable {
  public enum Controls {
    None = -1,
    Left = 0,
    Right,
    Up,
    Down,
    RotateCW,
    RotateCCW,
    Max
  }

  private Vector3 _origin = new Vector3();
  private Controller _controls = new Controller((int) Controls.Max);
  private Tween _tween = null;
  private Timer _rotateTimeout = null;
  private bool _canRotate = true;
  private uint _health = 0;
  private uint _shield = 0;

  [Signal]
  public delegate void Died();

  [Signal]
  public delegate void HealthDamaged(uint remaining);

  [Signal]
  public delegate void ShieldDamaged(uint remaining);

  [Export]
  public uint MaxHealth { get; set; } = 2;

  [Export]
  public uint MaxShield { get; set; } = 1;

  public void LockMotion() {
    _canRotate = !(AxisLockMotionX = AxisLockMotionY = AxisLockMotionZ = true);
  }

  public void UnlockMotion() {
    _canRotate = !(AxisLockMotionX = AxisLockMotionY = AxisLockMotionZ = false);
  }

  public void ToggleMotion() {
    _canRotate = !(AxisLockMotionX = AxisLockMotionY = AxisLockMotionZ = !AxisLockMotionX);
  }

  public void RestoreHealth(uint amount) {
    _health = Utility.Clamp(_health + amount, 0, MaxHealth);
    GD.Print($"Restored {amount} HP for a total of {_health} HP");
  }

  public void RestoreShield(uint amount) {
    _shield = Utility.Clamp(_shield + amount, 0, MaxShield);
    GD.Print($"Restored {amount} Shield for a total of {_shield} Shield");
  }

  public void ModifyVelocity(float x, float y, float z, float rotation) {
  }

  private void _OnTweenCompleted(object obj, string key) {
    switch (key)
    {
    case ":rotation_degrees":
      if (Mathf.IsEqualApprox(Mathf.PosMod(RotationDegrees.z, 360f), 0f))
      {
        RotationDegrees = new Vector3(RotationDegrees.x, RotationDegrees.y, 0f);
      }
      _rotateTimeout.Start();
      break;
    }
  }

  private void _OnRotateTimeout() {
    _canRotate = true;
  }

  protected override void _SetOrigin(Vector3 origin) {
    _origin = origin;
  }

  protected override Vector3 _GetOrigin() {
    return _origin;
  }

  public override void _Ready() {
    _UpdateMovementBounds(_GetOrigin(), MovementBounds);
    _tween = GetNode<Tween>("Tween");
    _rotateTimeout = GetNode<Timer>("RotateTimeout");

    _tween.Connect("tween_completed", this, "_OnTweenCompleted");
    _rotateTimeout.Connect("timeout", this, "_OnRotateTimeout");
    Initialize();
  }


  private void _SetControl(Controls control, bool state) {
    var idx = (int) control;
    _controls.SetControlIf(_controls.IsPressed(idx) != state, idx, state);
  }

  private Controls _FirstPressed(Controls a, Controls b) {
    return (Controls) (_controls.FirstPressed((int) a, (int) b));
  }

  private void _ReadControls() {
    _SetControl(Controls.Left, Input.IsActionPressed("play_move_left"));
    _SetControl(Controls.Right, Input.IsActionPressed("play_move_right"));
    _SetControl(Controls.Up, Input.IsActionPressed("play_move_up"));
    _SetControl(Controls.Down, Input.IsActionPressed("play_move_down"));
    _SetControl(Controls.RotateCW, Input.IsActionPressed("play_rotate_clockwise"));
    _SetControl(Controls.RotateCCW, Input.IsActionPressed("play_rotate_counter_clockwise"));
  }

  private void _HandleRotation() {
    if (!_canRotate)
    {
      return;
    }
    var angle = RotationDegrees;
    switch (_FirstPressed(Controls.RotateCW, Controls.RotateCCW))
    {
    case Controls.RotateCW:
      angle.z -= 90f;
      break;
    case Controls.RotateCCW:
      angle.z += 90f;
      break;
    default:
      return;
    }
    _tween.InterpolateProperty(this, "rotation_degrees", RotationDegrees, angle, 0.25f, Tween.TransitionType.Linear,
                               Tween.EaseType.InOut);
    _tween.Start();
    _canRotate = false;
  }

  private bool _IsEqualApprox(Vector3 a, Vector3 b, float tolerance) {
    return Mathf.IsEqualApprox(a.x, b.x, tolerance) && Mathf.IsEqualApprox(a.y, b.y, tolerance) &&
           Mathf.IsEqualApprox(a.z, b.z, tolerance);
  }

  private KinematicCollision _HandleMovement(float delta) {
    // Always travel forward.
    var direction = new Vector3(0f, 0f, -1f);
    switch (_FirstPressed(Controls.Left, Controls.Right))
    {
    case Controls.Left:
      direction.x = -1f;
      break;
    case Controls.Right:
      direction.x = 1f;
      break;
    }
    switch (_FirstPressed(Controls.Up, Controls.Down))
    {
    case Controls.Up:
      direction.y = 1f;
      break;
    case Controls.Down:
      direction.y = -1f;
      break;
    }
    // Directions are relative to Player rotation.
    direction = direction.Rotated(new Vector3(0f, 0f, 1f), Mathf.Deg2Rad(RotationDegrees.z));
    var transNoZ = Translation * new Vector3(1f, 1f, 0f);
    var bounds3d = new Vector3(MovementBounds.x, MovementBounds.y, 1f);
    var velocity = ((direction * bounds3d) - (transNoZ)).Normalized();
    // In place of _BoundTranslation to ensure good behavior.
    if (_IsEqualApprox(transNoZ, direction, 0.3f))
    {
      velocity *= new Vector3(0f, 0f, 1f);
      Translation = direction + (Translation * new Vector3(0f, 0f, 1f));
    }
    return MoveAndCollide(Speed * velocity * delta);
  }

  private void _HandleCollision(KinematicCollision collision) {
    if (collision == null)
    {
      return;
    }
    var other = collision.Collider as Node;
    if (other is Pickup)
    {
      var pickup = other as Pickup;
      pickup.ApplyEffect(this);
    }
    else
    {
      if (_shield != 0)
      {
        --_shield;
        EmitSignal("ShieldDamaged", _shield);
      }
      else
      {
        --_health;
        EmitSignal("HealthDamaged", _health);
      }
      if (_health == 0)
      {
        EmitSignal("Died");
      }
    }
    other.QueueFree();
  }

  public override void _PhysicsProcess(float delta) {
    _ReadControls();
    _HandleRotation();
    var collision = _HandleMovement(delta);
    _HandleCollision(collision);
  }

  public void Initialize() {
    _health = MaxHealth;
    _shield = 0;
    RotationDegrees = new Vector3();
  }

}
