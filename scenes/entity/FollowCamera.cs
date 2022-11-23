using Godot;

/**
 * @brief Behavior script for FollowCameras.
 *
 * This is the default game camera.
 */
public class FollowCamera : Camera {
  private Spatial _target = null;
  private Tween _tween = null;
  private ShakeTween _shake = null;

  /**
   * @brief The target Node that the camera should follow.
   */
  [Export]
  public NodePath Target { get; set; } = "";

  /**
   * @brief The depth from which the camera should follow its target.
   */
  [Export]
  public float DepthOffset { get; set; } = 0f;

  /**
   * @brief Whether or not the camera should rotate to follow the orientation of its target.
   */
  [Export]
  public bool FollowRotation { get; set; } = true;

  /**
   * @brief Whether or not the camera should follow the target as it translates on the X or Y axis.
   */
  [Export]
  public bool FollowTranslation { get; set; } = true;

  /**
   * @brief The duration of the camera shake triggered by targets receiving damage.
   *
   * This is only relevant for targets that emit "HealthDamaged" or "ShieldDamaged" signals.
   */
  [Export]
  public float ShakeDuration { get; set; } = 1f;

  /**
   * @brief The frequency of the camera shake triggered by targets receiving damage.
   *
   * This is only relevant for targets that emit "HealthDamaged" or "ShieldDamaged" signals.
   */
  [Export]
  public float ShakeFrequency { get; set; } = 1f;

  /**
   * @brief The amplitude of the camera shake triggered by targets receiving damage.
   *
   * This is only relevant for targets that emit "HealthDamaged" or "ShieldDamaged" signals.
   */
  [Export]
  public float ShakeAmplitude { get; set; } = 1f;

  private void _OnDamaged(uint _remaining) {
    _shake.Shake(ShakeDuration, ShakeFrequency, ShakeAmplitude);
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _target = GetNode<Spatial>(Target);
    _target.Connect("HealthDamaged", this, "_OnDamaged");
    _target.Connect("ShieldDamaged", this, "_OnDamaged");
    _tween = GetNode<Tween>("MoveTween");
    _shake = GetNode<ShakeTween>("ShakeTween");
  }

  /**
   * @brief Reset the FollowCamera to its initial state.
   */
  public void Initialize() {
    _shake.Stop();
  }

  /**
   * @brief Per-frame physics processing.
   *
   * @param delta The amount of time that has passed since the previous physics frame.
   */
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
