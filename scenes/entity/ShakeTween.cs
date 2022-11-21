using Godot;

public class ShakeTween : Tween {
  [Export]
  public Tween.TransitionType Transition { get; set; } = Tween.TransitionType.Sine;

  [Export]
  public Tween.EaseType Easing { get; set; } = Tween.EaseType.InOut;

  private float _amplitude = 0f;
  private Camera _target = null;
  private Timer _frequency = null;
  private Timer _duration = null;

  public void Shake(float duration, float frequency, float amplitude) {
    _duration.WaitTime = duration;
    _frequency.WaitTime = 1f / frequency;
    _amplitude = amplitude;
    _duration.Start();
    _frequency.Start();
    _StartShaking();
  }

  public void Stop() {
    _frequency.Stop();
    _duration.Stop();
    StopAll();
    _target.HOffset = 0f;
    _target.VOffset = 0f;
  }

  private void _StartShaking() {
    var x = (float) GD.RandRange(-_amplitude, _amplitude);
    var y = (float) GD.RandRange(-_amplitude, _amplitude);
    InterpolateProperty(_target, "h_offset", _target.HOffset, x, _frequency.WaitTime, Transition, Easing);
    InterpolateProperty(_target, "v_offset", _target.VOffset, y, _frequency.WaitTime, Transition, Easing);
    Start();
  }

  private void _ResetShaking() {
    InterpolateProperty(_target, "h_offset", _target.HOffset, 0f, _frequency.WaitTime, Transition, Easing);
    InterpolateProperty(_target, "v_offset", _target.VOffset, 0f, _frequency.WaitTime, Transition, Easing);
    Start();
  }

  private void _OnFrequencyTimeout() {
    _StartShaking();
  }

  private void _OnDurationTimeout() {
    _ResetShaking();
    _frequency.Stop();
  }

  public override void _Ready() {
    _target = GetParent<Camera>();
    _frequency = GetNode<Timer>("Frequency");
    _frequency.Connect("timeout", this, "_OnFrequencyTimeout");
    _duration = GetNode<Timer>("Duration");
    _duration.Connect("timeout", this, "_OnDurationTimeout");
  }
}
