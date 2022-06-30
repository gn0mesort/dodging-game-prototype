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
using Godot;

public class Main : Node {
  private readonly PackedScene _debugConsoleScene = GD.Load<PackedScene>("res://scenes/debug/DebugConsole.tscn");
  private SceneTree _tree = null;
  private DebugConsole _debugConsole = null;

  public bool Paused { get { return _tree.Paused; } set { _tree.Paused = value; } }

  private void OnDebugConsoleVisibilityChanged() {
    Paused = _debugConsole.Visible;
  }

  public override void _Ready() {
    _tree = GetTree();
    if (OS.IsDebugBuild())
    {
      _debugConsole = _debugConsoleScene.Instance() as DebugConsole;
      _debugConsole.Connect("visibility_changed", this, "OnDebugConsoleVisibilityChanged");
      _debugConsole.RegisterCommand(new DebugCommand(_debugConsole.Output, (output, parameters) => {
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
      AddChild(_debugConsole);
    }
  }
}
