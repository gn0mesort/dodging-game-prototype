using Godot;

public class Player : KinematicBody, IVelocityModifiable {
  public enum Controls {
    None = -1,
    Left = 0,
    Right,
    Up,
    Down,
    RotateCW,
    RotateCCW,
    Pause,
    Max
  }

  private Controller _controls = new Controller((int) Controls.Max);
  private Tween _tween = null;
  private Timer _rotateTimeout = null;
  private bool _canRotate = true;

  [Export]
  public Vector3 Speed { get; set; } = new Vector3(1f, 1f, 1f);

  [Export]
  public Vector3 MovementBounds { get; set; } = new Vector3(3f, 3f, 0f);

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

  public override void _Ready() {
    _tween = GetNode<Tween>("Tween");
    _rotateTimeout = GetNode<Timer>("RotateTimeout");

    _tween.Connect("tween_completed", this, "_OnTweenCompleted");
    _rotateTimeout.Connect("timeout", this, "_OnRotateTimeout");
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
    _SetControl(Controls.Pause, Input.IsActionPressed("play_pause"));
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
    _tween.InterpolateProperty(this, "rotation_degrees", RotationDegrees, angle, 0.25f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
    _tween.Start();
    _canRotate = false;
  }

  public override void _PhysicsProcess(float delta) {
    _ReadControls();
    _HandleRotation();
  }

}
