"""
  An object representing commands for the debug console.

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
class_name DebugCommand

var _target: Object = null
var _action: String = ""
var _usage: String = ""
var _help: String = ""
var _parameter_parser: BaseParameterParser = null

"""
  Construct a DebugCommand that invokes the action method on the target object.

  target: The target object of this command. Must not be null.
  action: The action to invoke on the target object. Must name a method on target.
  usage: A usage message displayed which can be displayed by a help command. Must not be null.
  help: A help message which can be displayed by a help command. Must not be null.
  parameter_parser: A BaseParameterParser object which will be used in parsing parameters for the new command. Must not be null.
"""
func _init(target: Object, action: String, usage: String = "", help: String = "", parameter_parser: BaseParameterParser = BaseParameterParser.new()) -> void:
  assert(target != null)
  assert(target.has_method(action))
  assert(usage != null)
  assert(help != null)
  assert(parameter_parser != null)
  _target = target
  _action = action
  _usage = usage
  _help = help
  _parameter_parser = parameter_parser

"""
  Parse a parameter String into a parameter array.

  parameter_string: A String representing command parameters.

  returns: A parsed parameter array.
"""
func parse_parameters(parameter_string: String) -> Array:
  return _parameter_parser.parse(parameter_string)

"""
  Invoke a the command with the given array of parameters.

  parameters: The command parameters array.
"""
func invoke(parameters: Array) -> void:
  _target.call_deferred(_action, parameters)

"""
  Invoke a command directly from a string.

  This is equivalent to calling invoke(parse_parameters(parameter_string))

  parameter_string: A String representing command parameters.
"""
func invoke_from_string(parameter_string: String) -> void:
  invoke(parse_parameters(parameter_string))

"""
  Get the command usage message.

  returns: The command usage message.
"""
func usage() -> String:
  return _usage

"""
  Get the command help message.

  returns: The command help message.
"""
func help() -> String:
  return _help
