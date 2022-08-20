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
  private SceneTree _tree = null;
  private CanvasLayer _debugLayer = null;
  private CanvasLayer _viewLayer = null;

  public DebugConsole Debug { get; private set; } = null;
  public SceneManager Scenes { get; private set; } = null;

  /**
   * A Scene to load immediately at program start.
   */
  [Export]
  public String InitialScene { get; set; } = null;

  /**
   * Whether or not the game is paused.
   */
  public bool Paused { get { return _tree.Paused; } set { _tree.Paused = value; } }

  private void OnDebugConsoleVisibilityChanged() {
    Paused = Debug.Visible;
  }

//  private uint LoadSceneFromPath(string path) {
//    Debug.Assert(path != null);
//    Debug.Assert(path != "");
//    _currentScene = GD.Load<PackedScene>(path);
//    if (_currentScene != null)
//    {
//      if (_currentSceneRoot != null)
//      {
//        _currentSceneRoot.QueueFree();
//      }
//      _currentSceneRoot = _currentScene.Instance();
//      if (_currentSceneRoot != null)
//      {
//        _viewLayer.CallDeferred("add_child", _currentSceneRoot);
//        return 0;
//      }
//    }
//    return 1;
//  }
//
//  private uint LoadScene(PackedScene scene) {
//    Debug.Assert(scene != null);
//    _currentScene = scene;
//    if (_currentSceneRoot != null)
//    {
//      _currentSceneRoot.QueueFree();
//    }
//    _currentSceneRoot = _currentScene.Instance();
//    if (_currentSceneRoot != null)
//    {
//      _viewLayer.CallDeferred("add_child", _currentSceneRoot);
//      return 0;
//    }
//    return 1;
//  }

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _tree = GetTree();
    _debugLayer = GetNode<CanvasLayer>("Debug");
    _viewLayer = GetNode<CanvasLayer>("View");
    Scenes = new SceneManager(_viewLayer, GetNode<AnimationPlayer>("Overlay/Black/AnimationPlayer"));
    if (OS.IsDebugBuild())
    {
      var scene = GD.Load<PackedScene>("res://scenes/debug/DebugConsole.tscn");
      Debug = scene.Instance() as DebugConsole;
      Debug.Connect("visibility_changed", this, "OnDebugConsoleVisibilityChanged");
      Debug.RegisterCommand(new DebugCommand((output, parameters) => {
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
      Debug.RegisterCommand(new DebugCommand((output, parameters) => {
        var args = parameters.Trim().Split(" ");
        var path = args.Length > 0 ? args[0].Trim() : null;
        if (path != null && path != "")
        {
          Scenes.LoadScene($"res://scenes/{path}.tscn");
          return 0;
        }
        return 1;
      }, "load_scene", "<SCENE>", "Instances a scene below the main node."));
      Debug.RegisterCommand(new DebugCommand((output, parameters) => {
        OS.WindowFullscreen = !OS.WindowFullscreen;
        return 0;
      }, "fullscreen", "", "Toggles fullscreen rendering."));
      Debug.RegisterCommand(new DebugCommand((output, parameters) => {
        Scenes.ReloadScene();
        return 0;
      }, "reload_scene", "", "Realods the current scene."));
      _debugLayer.CallDeferred("add_child", Debug);
    }
    if (InitialScene != null)
    {
      Scenes.LoadScene(InitialScene);
    }
  }
}
