using Godot;

/**
 * @brief Behavior script for the main Player entity.
 */
public class Player : BoundedKinematicBody, IVelocityModifiable {
  /**
   * @brief An enumeration representing different Player inputs.
   */
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
  private Timer _collideTimeout = null;
  private bool _canRotate = true;
  private bool _invulnerable = false;
  private uint _health = 0;
  private uint _shield = 0;
  private float _rotationModifier = 1f;
  private float _depthSpeedModifier = 1f;
  private Vector3 _speedModifier = new Vector3(1f, 1f, 1f);

  /**
   * @brief A Signal emitted when the Player dies (i.e., health == 0).
   */
  [Signal]
  public delegate void Died();

  /**
   * @brief A Signal emitted when the Player takes health damage.
   *
   * This is emitted even if the Player dies in the same frame.
   *
   * @param remaining The Player's remaining health.
   */
  [Signal]
  public delegate void HealthDamaged(uint remaining);

  /**
   * @brief A Signal emitted when the Player takes shield damage.
   *
   * This is emitted even if the Player dies in the same frame.
   *
   * @param remaining The Player's remaining shield.
   */
  [Signal]
  public delegate void ShieldDamaged(uint remaining);

  /**
   * @brief The Player's maximum health value.
   */
  [Export]
  public uint MaxHealth { get; set; } = 2;

  /**
   * @brief The Player's maximum shield value.
   */
  [Export]
  public uint MaxShield { get; set; } = 1;

  /**
   * @brief Lock Player motion and rotation on all axes.
   */
  public void LockMotion() {
    _canRotate = !(AxisLockMotionX = AxisLockMotionY = AxisLockMotionZ = true);
  }

  /**
   * @brief Unlock Player motion and rotation on all axes.
   */
  public void UnlockMotion() {
    _canRotate = !(AxisLockMotionX = AxisLockMotionY = AxisLockMotionZ = false);
  }

  /**
   * @brief Toggle Player motion and rotation on all axes.
   */
  public void ToggleMotion() {
    _canRotate = !(AxisLockMotionX = AxisLockMotionY = AxisLockMotionZ = !AxisLockMotionX);
  }

  /**
   * @brief Restore Player health.
   *
   * @param amount The amount of health to restore.
   */
  public void RestoreHealth(uint amount) {
    _health = Utility.Clamp(_health + amount, 0, MaxHealth);
    GD.Print($"Restored {amount} HP for a total of {_health} HP");
  }

  /**
   * @brief Restore Player shield.
   *
   * @param amount The amount of shield to restore.
   */
  public void RestoreShield(uint amount) {
    _shield = Utility.Clamp(_shield + amount, 0, MaxShield);
    GD.Print($"Restored {amount} Shield for a total of {_shield} Shield");
  }

  /**
   * @brief Modify the velocity of the target object by the given factor.
   *
   * Each factor is applied as-if by multiplication. That is to say, ModifyVelocity(0.5, 0.5, 0.5, 0.5) will halve all
   * the velocity of the target object along all three axes and halve the speed at which it can rotate. Similarly,
   * ModifyVelocity(1, 1, 1, 1) will restore the default behavior.
   *
   * @param x The X-axis velocity modifier.
   * @param y The Y-axis velocity modifier.
   * @param z The Z-axis velocity modifier.
   * @param rotation The rotational velocity modifier.
   */
  public void ModifyVelocity(float x, float y, float z, float rotation) {
    _speedModifier = new Vector3(x, y, z);
    _rotationModifier = 1f / rotation;
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

  private void _OnCollideTimeout() {
    _invulnerable = false;
    _depthSpeedModifier *= 2f;
  }

  protected override void _SetOrigin(Vector3 origin) {
    _origin = origin;
  }

  protected override Vector3 _GetOrigin() {
    return _origin;
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _UpdateMovementBounds(_GetOrigin(), MovementBounds);
    _tween = GetNode<Tween>("Tween");
    _rotateTimeout = GetNode<Timer>("RotateTimeout");
    _collideTimeout = GetNode<Timer>("CollideTimeout");

    _tween.Connect("tween_completed", this, "_OnTweenCompleted");
    _rotateTimeout.Connect("timeout", this, "_OnRotateTimeout");
    _collideTimeout.Connect("timeout", this, "_OnCollideTimeout");
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
    _tween.InterpolateProperty(this, "rotation_degrees", RotationDegrees, angle, 0.25f * _rotationModifier,
                               Tween.TransitionType.Linear, Tween.EaseType.InOut);
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
    var computed = Speed * _speedModifier * velocity;
    computed.z *= _depthSpeedModifier;
    return MoveAndCollide(computed * delta);
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
    else if (!_invulnerable)
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
      _invulnerable = true;
      _depthSpeedModifier /= 2f;
      _collideTimeout.Start();
    }
    other.QueueFree();
  }

  /**
   * @brief Per-frame physics processing.
   *
   * @param delta The amount of time that has passed since the previous physics frame.
   */
  public override void _PhysicsProcess(float delta) {
    _ReadControls();
    _HandleRotation();
    var collision = _HandleMovement(delta);
    _HandleCollision(collision);
    if (_health == 0)
    {
      EmitSignal("Died");
    }
  }

  /**
   * @brief Reset the Player to its initial state.
   *
   * This resets health to MaxHealth and shield to 0.
   */
  public void Initialize() {
    _health = MaxHealth;
    _shield = 0;
    RotationDegrees = new Vector3();
  }

}
