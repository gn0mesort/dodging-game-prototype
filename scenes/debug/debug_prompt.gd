"""
  Debug console and related functionality.

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
extends LineEdit

var _command_pattern: RegEx = RegEx.new()
var _commands: Dictionary = { }

onready var _output: TextEdit = $output

func _init() -> void:
  var err: int = _command_pattern.compile("^\\s*(?<command>[A-Za-z][A-Za-z0-9_]+)(?<parameters>.*)$")
  assert(!err)
  err = register_command("clear", DebugCommand.new(self, "_builtin_clear_command", "", "Clear the debug panel."))
  assert(!err)
  err = register_command("help", DebugCommand.new(self, "_builtin_help_command", "<COMMAND>", "Display command help.", PassthroughParameterParser.new()))
  assert(!err)
  err = register_command("list", DebugCommand.new(self, "_builtin_list_command", "", "Display all registered commands."))
  assert(!err)
  err = register_command("echo", DebugCommand.new(self, "_builtin_echo_command", "[TEXT]", "Print the input text to the debug panel and engine stdout.", PassthroughParameterParser.new()))
  assert(!err)

"""
  Print a message to stdout and the debug output panel.

  message: The message to be output.
"""
func print_log(message: String) -> void:
  _output.text += "%s\n" % message
  _output.scroll_vertical = _output.get_line_count()
  print(message)

func _builtin_clear_command(_parameters: Array) -> void:
  _output.text = ""

func _builtin_help_command(parameters: Array) -> void:
  var cmd: String = parameters[0]
  print_log("%s\nUsage: %s %s\n%s\n" % [ cmd, cmd, _commands[cmd].usage(), _commands[cmd].help() ])

func _builtin_list_command(_parameters: Array) -> void:
  for cmd in _commands:
    print_log(cmd)

func _builtin_echo_command(parameters: Array) -> void:
  for parameter in parameters:
    print_log(parameter)

func _ready() -> void:
  var err: int = connect("text_entered", self, "_on_text_entered")
  assert(!err)
  print_log("Debug Prompt Initialized.")

"""
  Register a DebugCommand object with the console.

  name: The name to register the command under.
  command: The command to register.

  returns: OK on success. ERR_ALREADY_EXISTS if the name is already registered.
"""
func register_command(name: String, command: DebugCommand) -> int:
  if !_commands.has(name):
    _commands[name] = command
    return OK
  else:
    return ERR_ALREADY_EXISTS

"""
  Unregister a DebugCommand object removing it from the console.

  name: The name to unregister the command under.
  command: The command to register.

  returns: OK on success. ERR_DOES_NOT_EXIST if the name is not registered.
"""
func unregister_command(name: String) -> int:
  if _commands.erase(name):
    return OK
  else:
    return ERR_DOES_NOT_EXIST

"""
  Invoke a command with a specific parameter array.

  name: the name of the command to invoke.
  parameters: An array of parameters to pass to the command.
"""
func invoke_command(name: String, parameters: Array) -> void:
  _commands[name].invoke(parameters)

func _input(event: InputEvent) -> void:
  if event.is_action_pressed("ui_toggle_debug_prompt"):
    visible = !visible

func _on_text_entered(command_input: String) -> void:
  command_input = command_input.strip_edges()
  print_log("> %s" % command_input)
  text = ""
  var matches: RegExMatch = _command_pattern.search(command_input)
  if matches:
    var command: String = matches.get_string("command")
    if command && _commands.has(command):
      var params: String = matches.get_string("parameters")
      _commands[command].invoke_from_string(params.strip_edges() if params else "")
      return
  print_log("Invalid command \"%s\"" % command_input)
