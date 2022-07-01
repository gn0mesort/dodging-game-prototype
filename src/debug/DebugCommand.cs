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

public class DebugCommand : Reference {
  public delegate uint Command(DebugOutput output, string parameters);

  private Command _command = null;

  public string Name { get; private set; } = "";
  public string Usage { get; private set; } = "";
  public string Description { get; private set; } = "";

  public DebugCommand(Command cmd, string name, string usage, string description) {
    _command = cmd;
    Name = name;
    Usage = usage;
    Description = description;
  }

  public string Help() {
    return $"{Name}{System.Environment.NewLine}Usage: {Name} {Usage}{System.Environment.NewLine}{Description}";
  }

  public void Invoke(DebugOutput output, string parameters) {
    var res = _command(output, parameters);
    if (res != 0)
    {
      output.WriteLine($"Command \"{Name}\" failed with code 0x{res:x8}.");
      output.WriteLine(Help());
    }
  }
}
