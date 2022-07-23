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
  private PackedScene _currentScene = null;
  private Node _currentSceneRoot = null;

  /**
   * Whether or not the game is paused.
   */
  public bool Paused { get { return _tree.Paused; } set { _tree.Paused = value; } }

  private void OnDebugConsoleVisibilityChanged() {
    Paused = _debugConsole.Visible;
  }

  private uint LoadSceneFromPath(string path) {
    Debug.Assert(path != null);
    Debug.Assert(path != "");
    _currentScene = GD.Load<PackedScene>(path);
    if (_currentScene != null)
    {
      if (_currentSceneRoot != null)
      {
        _currentSceneRoot.QueueFree();
      }
      _currentSceneRoot = _currentScene.Instance();
      if (_currentSceneRoot != null)
      {
        CallDeferred("add_child", _currentSceneRoot);
        return 0;
      }
    }
    return 1;
  }

  private uint LoadScene(PackedScene scene) {
    Debug.Assert(scene != null);
    _currentScene = scene;
    if (_currentSceneRoot != null)
    {
      _currentSceneRoot.QueueFree();
    }
    _currentSceneRoot = _currentScene.Instance();
    if (_currentSceneRoot != null)
    {
      CallDeferred("add_child", _currentSceneRoot);
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
      _debugConsole.RegisterCommand(new DebugCommand((output, parameters) => {
        OS.WindowFullscreen = !OS.WindowFullscreen;
        return 0;
      }, "fullscreen", "", "Toggles fullscreen rendering."));
//      _debugConsole.RegisterCommand(new DebugCommand((output, parameters) => {
//        return LoadScene(_currentScene);
//      }, "reload_scene", "", "Realods the current scene."));
//      _debugConsole.RegisterCommand(new DebugCommand((output, parameters) => {
//        var CoordsPattern = new RegEx();
//        CoordsPattern.Compile(@"^\((?<x>\d*\.?\d+f?),\s*(?<y>\d*\.?\d+f?),\s*(?<z>\d*\.?\d+f?)\)$");
//        var mainNode = GetNode<Node>("/root/Main");
//        GridMap map = null;
//        for (int i = 0; i < mainNode.GetChildCount() && map == null; ++i)
//        {
//          map = mainNode.GetChild(i) as GridMap;
//        }
//        if (map == null)
//        {
//          return 1;
//        }
//        var matches = CoordsPattern.Search(parameters.Trim());
//        if (matches != null)
//        {
//          var coords = new Vector3();
//          var success = Single.TryParse(matches.GetString("x"), out coords.x);
//          success = success && Single.TryParse(matches.GetString("y"), out coords.y);
//          success = success && Single.TryParse(matches.GetString("z"), out coords.z);
//          if (success)
//          {
//            output.WriteLine($"On GridMap \"{map.Name}\": World = {coords}, Grid = {map.WorldToMap(coords)}");
//            return 0;
//          }
//          return 2;
//        }
//        return 3;
//      }, "wtg", "<WORLD_COORDS>", "Print GridMap position equivalent to the input global coordinates."));
//      _debugConsole.RegisterCommand(new DebugCommand((output, parameters) => {
//        var CoordsPattern = new RegEx();
//        CoordsPattern.Compile(@"^\((?<x>\d+),\s*(?<y>\d+),\s*(?<z>\d+)\)$");
//        var mainNode = GetNode<Node>("/root/Main");
//        GridMap map = null;
//        for (int i = 0; i < mainNode.GetChildCount() && map == null; ++i)
//        {
//          map = mainNode.GetChild(i) as GridMap;
//        }
//        if (map == null)
//        {
//          return 1;
//        }
//        var matches = CoordsPattern.Search(parameters.Trim());
//        if (matches != null)
//        {
//          var coords = new int[3];
//          var success = Int32.TryParse(matches.GetString("x"), out coords[0]);
//          success = success && Int32.TryParse(matches.GetString("y"), out coords[1]);
//          success = success && Int32.TryParse(matches.GetString("z"), out coords[2]);
//          if (success)
//          {
//            output.WriteLine($"On GridMap \"{map.Name}\": Grid = ({coords[0]}, {coords[1]}, {coords[2]}), World = {map.MapToWorld(coords[0], coords[1], coords[2])}");
//            return 0;
//          }
//          return 2;
//        }
//        return 3;
//      }, "gtw", "<GRID_COORDS>", "Print global position equivalent to the input GridMap coordinates."));
//      CallDeferred("add_child", _debugConsole);
//    }
    if (InitialScene != null)
    {
      LoadScene(InitialScene);
    }
  }
}
