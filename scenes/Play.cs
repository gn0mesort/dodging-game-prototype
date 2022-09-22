/** Gameplay node.
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
using Godot;
using System.Diagnostics;

public class Play : Spatial, IDependsOnMain, IRequiresConfiguration {
  public class Configuration {
    /**
     * The path to the level to load.
     */
    public string LevelPath { get; private set; } = "";

    /**
     * Construct a new Configuration.
     *
     * @param levelPath The path to the level that this configuration should load.
     */
    public Configuration(string levelPath) {
      Debug.Assert(levelPath != null);
      Debug.Assert(!levelPath.Empty());
      // TODO: verify that the input path is a level path.
      LevelPath = levelPath;
    }

    /**
     * Apply this configuration to the given node.
     *
     * @param target The node to configure.
     */
    public void Apply(Node target) {
      var actualTarget = target as Play;
      if (actualTarget == null)
      {
        return;
      }
      actualTarget.Levels.LoadLevel(LevelPath);
    }
  }

  private Main _main = null;
  private SceneManager.SceneConfigurationMethod _deferredConfig = null;
  private Player _player = null;
  private Timer _timer = null;
  private uint _timerTicks = 0;
  private bool _timerLeadIn = true;
  private PackedScene _pauseMenuScene = GD.Load<PackedScene>("res://scenes/prefab/Pause.tscn");

  /**
   * The gameplay level manager.
   */
  public LevelManager Levels { get; private set; } = null;

  /**
   * Set the main node.
   *
   * @param main The top level node of the scene.
   */
  public void SetMainNode(Main main) {
    _main = main;
    Debug.Assert(_main != null);
  }

  /**
   * Get the main node.
   *
   * @return The main node.
   */
  public Main GetMainNode() {
    return _main;
  }

  /**
   * Configure the implementing object.
   *
   * @param config A delegate defining the configuration process and any required data.
   */
  public void Configure(SceneManager.SceneConfigurationMethod config) {
    _deferredConfig = config;
  }

  private void _OnTimerTimeout() {
    if (_timerLeadIn && _timerTicks < 3)
    {
      ++_timerTicks;
    }
    else if (_timerLeadIn && _timerTicks >= 3)
    {
      _timer.Stop();
      _timerLeadIn = false;
      _timerTicks = 0;
      GD.Print("Start game");
      _player.SetDeferred("axis_lock_motion_z", false);
    }
    else if (!_timerLeadIn && _timerTicks < 3)
    {
      ++_timerTicks;
    }
    else if (!_timerLeadIn && _timerTicks >= 3)
    {
      _timer.Stop();
      _timerLeadIn = true;
      _timerTicks = 0;
      GD.Print("Stop game");
      _player.SetDeferred("axis_lock_motion_z", true);
      // In the future do something more intelligent.
      _main.Scenes.LoadScene("res://scenes/TitleMenu.tscn");
    }
  }

  private void _OnPlayerDied() {
    _main.Scenes.LoadScene("res://scenes/GameOver.tscn");
  }

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _player = GetNode<Player>("Player");
    _timer = GetNode<Timer>("Timer");
    Levels = new LevelManager(this, _player);
    _deferredConfig(this);
    _player.Connect("Died", this, "_OnPlayerDied");
    _timer.Connect("timeout", this, "_OnTimerTimeout");
    _timer.CallDeferred("start");
  }


  /**
   * Per physics frame processing.
   *
   * @param delta The delta time between the last physics frame and current physics frame.
   */
  public override void _PhysicsProcess(float delta) {
    if (!_timerLeadIn && (_player.Translation.z < Levels.ExitDepth.z) && _timer.IsStopped())
    {
      GD.Print($"Player Translation @ Exit: {_player.Translation}");
      _timer.CallDeferred("start");
    }
  }

  /**
   * Node-specific input handling.
   *
   * @param ev The input event to handle.
   */
  public override void _Input(InputEvent ev) {
    if (ev.IsActionPressed("pause"))
    {
      _main.Paused = true;
      var pauseMenu = _pauseMenuScene.Instance() as Pause;
      pauseMenu.SetMainNode(_main);
      CallDeferred("add_child", pauseMenu);
    }
  }
}
