/**
 * Copyright (C) 2022 Alexander Rothman <gnomesort@megate.ch>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */
using Godot;

/**
 * @brief A tweened camera shake effect.
 *
 * This must have a Camera node as a parent.
 */
public class ShakeTween : Tween {
  /**
   * @brief The Tween transition type to use.
   */
  [Export]
  public Tween.TransitionType Transition { get; set; } = Tween.TransitionType.Sine;

  /**
   * @brief The Tween easing type to use.
   */
  [Export]
  public Tween.EaseType Easing { get; set; } = Tween.EaseType.InOut;

  private float _amplitude = 0f;
  private Camera _target = null;
  private Timer _frequency = null;
  private Timer _duration = null;

  /**
   * @brief Start shaking the parent camera.
   *
   * @param duration The duration (in seconds) to shake the camera for.
   * @param frequency The frequency (in Hz) of the shaking motion.
   * @param amplitude The amplitude (i.e., maximum and minimum values) of the shaking motion.
   */
  public void Shake(float duration, float frequency, float amplitude) {
    _duration.WaitTime = duration;
    _frequency.WaitTime = 1f / frequency;
    _amplitude = amplitude;
    _duration.Start();
    _frequency.Start();
    _StartShaking();
  }

  /**
   * @brief Immediately stop shaking the parent camera.
   */
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

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _target = GetParent<Camera>();
    _frequency = GetNode<Timer>("Frequency");
    _frequency.Connect("timeout", this, "_OnFrequencyTimeout");
    _duration = GetNode<Timer>("Duration");
    _duration.Connect("timeout", this, "_OnDurationTimeout");
  }
}
