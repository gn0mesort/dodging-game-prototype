/** Main game entry point.
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

public class Main : Node {
  /**
   * A Scene to load immediately at program start.
   */
  [Export]
  public PackedScene InitialScene{ get; set; } = null;

  private readonly PackedScene _debugConsoleScene = GD.Load<PackedScene>("res://scenes/debug/DebugConsole.tscn");
  private SceneTree _tree = null;
  private DebugConsole _debugConsole = null;
  private Spatial _currentScene = null;

  /**
   * Whether or not the game is paused.
   */
  public bool Paused { get { return _tree.Paused; } set { _tree.Paused = value; } }

  private void OnDebugConsoleVisibilityChanged() {
    Paused = _debugConsole.Visible;
  }

  /**
   * Load a 3D scene from resource path.
   *
   * @param path The resource (res://) path of the scene to load.
   *
   * @return 0 on success. Otherwise 1.
   */
  private uint LoadSceneFromPath(string path) {
    Debug.Assert(path != null);
    Debug.Assert(path != "");
    var scene = GD.Load<PackedScene>(path);
    if (scene != null)
    {
      if (_currentScene != null)
      {
        CallDeferred("remove_child", _currentScene);
      }
      _currentScene = scene.InstanceOrNull<Spatial>();
      if (_currentScene != null)
      {
        CallDeferred("add_child", _currentScene);
        return 0;
      }
    }
    return 1;
  }


  /**
   * Load a 3D scene.
   *
   * @param scene a Spatial indicating the root of the scene to load.
   *
   * @return 0 on success. Otherwise 1.
   */
  private uint LoadScene(Spatial scene) {
    Debug.Assert(scene != null);
    if (_currentScene != null)
    {
      CallDeferred("remove_child", _currentScene);
    }
    _currentScene = scene;
    if (_currentScene != null)
    {
      CallDeferred("add_child", _currentScene);
      return 0;
    }
    return 1;
  }

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _tree = GetTree();
    if (OS.IsDebugBuild())
    {
      _debugConsole = _debugConsoleScene.Instance<DebugConsole>();
      _debugConsole.Connect("visibility_changed", this, "OnDebugConsoleVisibilityChanged");
      _debugConsole.RegisterCommand(new DebugCommand((output, parameters) => {
        var args = parameters.Trim().Split(" ");
        if (args.Length > 0 && args[0] != "")
        {
          var code = 0;
          if (Int32.TryParse(args[0].Trim(), out code))
          {
            OS.ExitCode = code;
            _tree.Notification(MainLoop.NotificationWmQuitRequest);
            return 0;
          }
          return 1;
        }
        OS.ExitCode = 0;
        _tree.Notification(MainLoop.NotificationWmQuitRequest);
        return 0;
      }, "exit", "[CODE]", "Exit with the given return code."));
      _debugConsole.RegisterCommand(new DebugCommand((output, parameters) => {
        var args = parameters.Trim().Split(" ");
        var path = args.Length > 0 ? args[0].Trim() : null;
        if (path != null && path != "")
        {
          return LoadSceneFromPath($"res://scenes/{path}.tscn");
        }
        return 1;
      }, "load_scene", "<SCENE>", "Instances a scene below the main node."));
      CallDeferred("add_child", _debugConsole);
    }
    if (InitialScene != null)
    {
      CallDeferred("LoadScene", InitialScene.InstanceOrNull<Spatial>());
    }
  }
}
