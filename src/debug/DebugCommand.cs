/** Debug console command type.
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

public class DebugCommand {
  /**
   * Command procedure to be executed during calls to Invoke.
   */
  public delegate uint Command(DebugOutput output, string parameters);

  private Command _command = null;

  /**
   * A unique DebugCommand name.
   */
  public string Name { get; private set; } = "";

  /**
   * A message describing the usage of the DebugCommand.
   */
  public string Usage { get; private set; } = "";

  /**
   * A message describing the functionality of the DebugCommand.
   */
  public string Description { get; private set; } = "";

  public DebugCommand(Command cmd, string name, string usage, string description) {
    _command = cmd;
    Debug.Assert(_command != null);
    Name = name;
    Debug.Assert(Name != null);
    Debug.Assert(!Name.Empty());
    Usage = usage;
    Debug.Assert(Usage != null);
    Description = description;
    Debug.Assert(Description != null);
  }

  /**
   * Get a help message.
   *
   * @return A help message based on the value of Name, Usage, and Description.
   */
  public string Help() {
    return $"{Name}{System.Environment.NewLine}Usage: {Name} {Usage}{System.Environment.NewLine}{Description}";
  }

  /**
   * Invoke the DebugCommand.
   *
   * @param output The DebugOutput to write into.
   * @param parameters a string containing any parameters the command expects during processing.
   */
  public void Invoke(DebugOutput output, string parameters) {
    Debug.Assert(output != null);
    Debug.Assert(parameters != null);
    var res = _command(output, parameters);
    if (res != 0)
    {
      output.WriteLine($"Command \"{Name}\" failed with code 0x{res:x8}.");
      output.WriteLine(Help());
    }
  }
}
