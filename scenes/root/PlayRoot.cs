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
using System.Diagnostics;

/**
 * @brief Behavior script for the root play scene.
 */
public class PlayRoot : Node {
  private PackedScene _pauseScreen = null;
  private Main _main = null;
  private Spatial _world = null;
  private Player _player = null;
  private FollowCamera _camera = null;
  private Level _level = null;
  private Timer _leadIn = null;
  private Timer _leadOut = null;
  private ulong _startTime = 0;
  private bool _exited = false;

  /**
   * @brief A Signal emitted to request a transition between root scenes.
   *
   * @param to The RootScene to transition to.
   */
  [Signal]
  public delegate void TransitionRoot(RootScenes to);

  /**
   * @brief A resource path indicating a level to load in place of the usual tutorial and array levels.
   */
  [Export]
  public string LevelOverride { get; set; } = "";

  /**
   * @brief A resource path indicating a level to load as the tutorial level.
   *
   * This is only considered if (Main.Player.Flags & PlayerData.TUTORIAL_COMPLETE_BIT) == 0
   */
  [Export]
  public string TutorialLevel { get; set; } = null;

  /**
   * @brief An array of resource paths indicating levels to progress through during play.
   */
  [Export]
  public string[] Levels { get; set; } = null;

  /**
   * @brief If this is set to true then PlayerData will be cleared completely (including flags) upon scene start.
   *
   * This is for debugging purposes.
   */
  [Export]
  public bool InitializePlayerFlagsOnStart { get; set; } = false;

  /**
   * @brief Initialization method.
   */
  public override void _EnterTree() {
    _main = GetNode<Main>("/root/Main");
    Connect("TransitionRoot", _main, "TransitionRoot");
    _pauseScreen = GD.Load<PackedScene>("res://scenes/play/PauseScreen.tscn");
  }

  private void _InitializeLevel(string path) {
    if (_level != null)
    {
      _level.QueueFree();
    }
    _exited = false;
    _level = GD.Load<PackedScene>(path).Instance() as Level;
    _world.AddChild(_level);
    _player.Initialize();
    _player.Translation = _level.Entrance();
    // Lock motion during lead
    _player.LockMotion();
    _camera.Initialize();
    _player.Visible = true;
    _camera.Visible = true;
    GD.Print($"Level \"{_level.LevelName}\" Start");
    _leadIn.Start();
  }

  private void _StartLevel() {
    if (LevelOverride != null && LevelOverride.Length > 0)
    {
      _InitializeLevel(LevelOverride);
    }
    else if (TutorialLevel != null && TutorialLevel.Length > 0 &&
             (_main.Player.Flags & PlayerData.TUTORIAL_COMPLETE_BIT) == 0)
    {
      _InitializeLevel(TutorialLevel);
    }
    else
    {
      Debug.Assert(Levels.Length > _main.Player.Progress);
      var levelPath = Levels[_main.Player.Progress];
      Debug.Assert(levelPath != null);
      Debug.Assert(levelPath.Length > 0);
      _InitializeLevel(levelPath);
    }
  }

  private void _OnPlayerDied() {
    ++_main.Player.Deaths;
    _UpdatePlayTime();
    GD.Print("Player died.");
    _StartLevel();
  }

  private void _OnPlayerDamaged(uint remaining) {
    ++_main.Player.Collisions;
    GD.Print("Player damaged");
  }

  private void _OnLeadInTimeout() {
    _startTime = OS.GetTicksMsec();
    _player.UnlockMotion();
  }

  private void _UpdatePlayTime() {
    var time = OS.GetTicksMsec() - _startTime;
    GD.Print($"Run time: {time}ms");
    _main.Player.PlayTime += time / 1000;
  }

  private void _OnLeadOutTimeout() {
    // We're not going to have 2^16 levels hopefully
    var oldProgress = _main.Player.Progress;
    if ((_main.Player.Flags & PlayerData.TUTORIAL_COMPLETE_BIT) == 0)
    {
      _main.Player.Flags |= PlayerData.TUTORIAL_COMPLETE_BIT;
      _main.StorePlayerData();
    }
    else
    {
      _main.Player.Progress = (ushort) Utility.Clamp(_main.Player.Progress + 1, 0, Levels.Length - 1);
      _main.StorePlayerData();
      if (LevelOverride != null && LevelOverride.Length > 0)
      {
        EmitSignal("TransitionRoot", RootScenes.Menu);
        return;
      }
      else if (oldProgress == _main.Player.Progress)
      {
        EmitSignal("TransitionRoot", RootScenes.GameComplete);
        return;
      }
    }
    _StartLevel();
  }

  /**
   * @brief Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _world = GetNode<Spatial>("World");
    _player = GetNode<Player>("World/Player");
    _player.Connect("Died", this, "_OnPlayerDied");
    _player.Connect("HealthDamaged", this, "_OnPlayerDamaged");
    _player.Connect("ShieldDamaged", this, "_OnPlayerDamaged");
    _camera = GetNode<FollowCamera>("World/FollowCamera");
    _leadIn = GetNode<Timer>("World/LeadIn");
    _leadIn.Connect("timeout", this, "_OnLeadInTimeout");
    _leadOut = GetNode<Timer>("World/LeadOut");
    _leadOut.Connect("timeout", this, "_OnLeadOutTimeout");
    if (InitializePlayerFlagsOnStart)
    {
      _main.InitializePlayerData(false);
      _main.StorePlayerData();
    }
    _StartLevel();
  }

  private void _EndLevel() {
    _exited = true;
    _player.LockMotion();
    GD.Print($"Level \"{_level.LevelName}\" End");
    _UpdatePlayTime();
    _leadOut.Start();
  }

  /**
   * @brief Per-frame physics processing.
   *
   * @param delta The amount of time that has passed since the previous physics frame.
   */
  public override void _PhysicsProcess(float delta) {
    // Less than is farther forward in this context
    if (_level != null && !_exited && _player.Translation.z < _level.Exit().z)
    {
      _EndLevel();
    }
  }

  private void _OnRequestResume() {
    GetNode("PauseScreen").QueueFree();
    _main.Resume();
  }

  private void _OnRequestRestartLevel() {
    _UpdatePlayTime();
    _StartLevel();
    _OnRequestResume();
  }

  /**
   * @brief Handling for otherwise unhandled InputEvents.
   *
   * @param ev The event that triggered handling.
   */
  public override void _UnhandledInput(InputEvent ev) {
    if (ev.IsActionPressed("play_pause"))
    {
      _main.TogglePaused();
      if (_main.IsPaused())
      {
        var pauseScreen = _pauseScreen.Instance();
        AddChild(pauseScreen);
        pauseScreen.Connect("RequestResume", this, "_OnRequestResume");
        pauseScreen.Connect("RequestRestartLevel", this, "_OnRequestRestartLevel");
      }
      else
      {
        GetNode("PauseScreen").QueueFree();
      }
      GetTree().SetInputAsHandled();
    }
  }
}
