/** Player entity.
 *
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
using System;
using System.Diagnostics;
using Godot;

public class Player : KinematicBody, ICollidable {
  private enum Controls {
    None = -1,
    Left = 0,
    Up,
    Right,
    Down,
    Max
  }

  /**
   * A signal indicating a change in Player status (i.e., health or score).
   */
  [Signal]
  public delegate void StatusChanged(uint health, uint score);

  [Signal]
  public delegate void Died();

  private Vector3 _LEFT = new Vector3(-1f, 0f, 0f);
  private Vector3 _UP = new Vector3(0f, 1f, 0f);
  private Vector3 _RIGHT = new Vector3(1f, 0f, 0f);
  private Vector3 _DOWN = new Vector3(0f, -1f, 0f);
  private uint _score = 0;
  private uint _health = 2;
  private AnimationPlayer _animations = null;
  private Tween _tweens = null;
  private CSGBox _mesh = null;
  private Controller _controls = new Controller((int) Controls.Max);
  private static readonly Vector3 _VERTICAL_ORIENTATION = new Vector3(0f, 0f, 90f);
  private static readonly Vector3 _HORIZONTAL_ORIENTATION = new Vector3(0, 0, 0);
  private Vector3 _orientation = _HORIZONTAL_ORIENTATION;
  private bool _rotating = false;

  /**
   * The Player's basic speed in units/second.
   */
  [Export]
  public Vector3 BaseSpeed { get; set; } = new Vector3(36f, 36f, -36f);

  /**
   * The extents within which the Player can move.
   * Values are absolute.
   */
  [Export]
  public Vector3 MovementExtents { get; set; } = new Vector3(12f, 6f, 0f);

  /**
   * The Player's current score.
   * This must be between 0 and UInt32.MaxValue.
   */
  [Export]
  public uint Score {
    get {
      return _score;
    }
    set {
      EmitSignal("StatusChanged", _health, _score);
      _score = (uint) Mathf.Clamp((long) value, 0, UInt32.MaxValue);
    }
  }

  /**
   * The Player's current health.
   * This must be between 0 and UInt32.MaxValue.
   */
  [Export]
  public uint Health {
    get {
      return _health;
    }
    set {
      EmitSignal("StatusChanged", _health, _score);
      _health = (uint) Mathf.Clamp((long) value, 0, UInt32.MaxValue);
    }
  }

  private void _SetControl(Controls control, bool state) {
    var idx = (int) control;
    _controls.SetControlIf(_controls.IsPressed(idx) != state, idx, state);
  }

  private void _UpdateControls() {
    _SetControl(Controls.Left, Input.IsActionPressed("move_left"));
    _SetControl(Controls.Up, Input.IsActionPressed("move_up"));
    _SetControl(Controls.Right, Input.IsActionPressed("move_right"));
    _SetControl(Controls.Down, Input.IsActionPressed("move_down"));
  }

  private Vector3 _ChooseDirection() {
    var res = new Vector3(0f, 0f, 0f);
    switch ((Controls) _controls.FirstPressed((int) Controls.Left, (int) Controls.Right))
    {
    case Controls.Left:
      res += _LEFT;
      break;
    case Controls.Right:
      res += _RIGHT;
      break;
    }
    switch ((Controls) _controls.FirstPressed((int) Controls.Down, (int) Controls.Up))
    {
    case Controls.Down:
      res += _DOWN;
      break;
    case Controls.Up:
      res += _UP;
      break;
    }
    return res;
  }

  private void _OnTweenCompleted(object obj, NodePath key) {
    if (key == ":rotation_degrees")
    {
      RotationDegrees = _orientation;
      _rotating = false;
    }
  }

  private void _OnAnimationFinished(string animation) {
    if (animation == "CollideAndDie")
    {
      EmitSignal("Died");
    }
    GD.Print(animation);
  }

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
     _LEFT *= MovementExtents;
       _UP *= MovementExtents;
    _RIGHT *= MovementExtents;
     _DOWN *= MovementExtents;
    _animations = GetNode<AnimationPlayer>("Animations");
    _tweens = GetNode<Tween>("Tweens");
    _mesh = GetNode<CSGBox>("CSGBox");
    _tweens.Connect("tween_completed", this, "_OnTweenCompleted");
    _animations.Connect("animation_finished", this, "_OnAnimationFinished");
    _animations.Play("Reset");
  }

  private static bool _IsEqualApprox(Vector3 a, Vector3 b, float tolerance) {
    return Mathf.IsEqualApprox(a.x, b.x, tolerance) && Mathf.IsEqualApprox(a.y, b.y, tolerance) &&
           Mathf.IsEqualApprox(a.z, b.z, tolerance);
  }

  /**
   * Per physics frame processing.
   */
  public override void _PhysicsProcess(float delta) {
    _UpdateControls();
    var next = _ChooseDirection();
    if (!_rotating && Input.IsActionJustPressed("move_rotate"))
    {
      if (_IsEqualApprox(RotationDegrees, _HORIZONTAL_ORIENTATION, 0.2f))
      {
        _orientation = _VERTICAL_ORIENTATION;
      }
      else
      {
        _orientation = _HORIZONTAL_ORIENTATION;
      }
      _tweens.InterpolateProperty(this, "rotation_degrees", RotationDegrees, _orientation, 0.25f,
                                  Tween.TransitionType.Linear, Tween.EaseType.InOut);
      _tweens.Start();
      _rotating = true;
    }
    var transNoZ = Translation * new Vector3(1f, 1f, 0f);
    var velocity = (next - transNoZ).Normalized();
    velocity.z = 1f;
    velocity *= BaseSpeed;
    if (_IsEqualApprox(next, transNoZ, 0.3f))
    {
      velocity = new Vector3(0f, 0f, velocity.z);
      Translation = next + (Translation * new Vector3(0f, 0f, 1f));
    }
    var collision = MoveAndCollide(velocity * delta);
    HandleCollision(collision);
  }

  public void TakeDamage() {
      var animation = "CollideAndTakeDamage";
      if (--Health == 0)
      {
        EmitSignal("StatusChanged", _health, _score);
        animation = "CollideAndDie";
        CallDeferred("set_physics_process", false);
        GD.Print($"Player Translation @ Death: {Translation}");
      }
      if (_animations.IsPlaying())
      {
        _animations.Stop();
      }
      _animations.Play(animation);
  }

  public void HandleCollision(KinematicCollision collision) {
    if (collision != null)
    {
      (collision.Collider as Node).QueueFree();
      TakeDamage();
    }
  }
}
