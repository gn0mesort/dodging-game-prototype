/** Debug console output type.
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

public class DebugOutput : Control {
  private TextEdit _text_area = null;

  /**
   * Post-_EnterTree initialization.
   */
  public override void _Ready() {
    _text_area = GetNode<TextEdit>("Container/TextArea");
  }

  /**
   * Clear the DebugOutput.
   */
  public void Clear() {
    _text_area.Text = "";
  }

  /**
   * Write a string followed by a newline to the DebugOutput.
   *
   * @param message The string to write.
   */
  public void WriteLine(string message) {
    _text_area.Text += $"{message}{System.Environment.NewLine}";
    _text_area.ScrollVertical = _text_area.GetLineCount();
    GD.Print($"[DebugConsole] \"{message}\"");
  }
}
