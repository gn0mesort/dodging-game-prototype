/** Debug console.
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
using System.Collections.Generic;
using System.Diagnostics;

public class DebugConsole : Control {
  private static readonly RegEx _command_pattern = new RegEx();

  private LineEdit _input = null;
  private readonly Dictionary<string, DebugCommand> _commands = new Dictionary<string, DebugCommand>();

  public DebugOutput Output { get; private set; } = null;

  private void OnTextEntered(string newText) {
    _input.Clear();
    Output.WriteLine($"> {newText}");
    var matches = _command_pattern.Search(newText);
    if (matches != null)
    {
      var command = matches.GetString("command");
      if (command != null && _commands.ContainsKey(command))
      {
        var parameters = matches.GetString("parameters");
        _commands[command].Invoke(Output, parameters);
        return;
      }
    }
    Output.WriteLine($"Invalid command \"{newText}\".");
  }

  public DebugConsole() {
    var res = _command_pattern.Compile(@"^\s*(?<command>[A-Za-z][A-Za-z0-9_]+)(?<parameters>.*)$");
    Debug.Assert(res == Error.Ok);
  }

  public void RegisterCommand(DebugCommand command) {
    _commands.Add(command.Name, command);
  }

  public override void _Ready() {
    _input = GetNode<LineEdit>("DebugInput");
    Output = GetNode<DebugOutput>("DebugOutput");
    _input.Connect("text_entered", this, "OnTextEntered");
    RegisterCommand(new DebugCommand((output, parameters) => {
      foreach (var kvp in _commands)
      {
        output.WriteLine(kvp.Value.Name);
      }
      return 0;
    }, "list", "", "List all registered commands."));
    RegisterCommand(new DebugCommand((output, parameters) => {
      var args = parameters.Trim().Split(" ");
      if (args.Length > 0 && args[0] != "")
      {
        var command = args[0].Trim();
        if (_commands.ContainsKey(command))
        {
          output.WriteLine(_commands[command].Help());
          return 0;
        }
      }
      return 1;
    }, "help", "<COMMAND>", "Display command help messages."));
    RegisterCommand(new DebugCommand((output, parameters) => {
          if (parameters != null)
          {
            output.WriteLine(parameters.Trim());
            return 0;
          }
          return 1;
    }, "echo", "<TEXT>", "Write the input text to standard output."));
    RegisterCommand(new DebugCommand((output, parameters) => {
      output.Clear();
      return 0;
    }, "clear", "", "Clear the output window."));
  }

  public override void _Input(InputEvent ev) {
    if (ev.IsActionPressed("ui_toggle_debug_prompt") && !ev.IsEcho())
    {
      Visible = !Visible;
    }
  }
}
