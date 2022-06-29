"""
  Base ParameterParser type for DebugCommands.
  This class always returns an empty array.

  Copyright (C) 2022 Alexander Rothman <gnomesort@megate.ch>

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU Affero General Public License as published
  by the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Affero General Public License for more details.

  You should have received a copy of the GNU Affero General Public License
  along with this program.  If not, see <https://www.gnu.org/licenses/>.
"""
extends Reference
class_name BaseParameterParser

func _parse(_parameter_string: String) -> Array:
  return Array()

"""
  Parse a string into an array of command parameters.

  parameter_string: The input to parse.

  returns: An array of parameters based on the input string
"""
func parse(parameter_string: String) -> Array:
  return _parse(parameter_string)
