"""
  Main game entry point.

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
extends Node

var _debug_prompt_scene: PackedScene = preload("res://scenes/debug/debug_prompt.tscn")
var _debug_prompt: Control = null

onready var _tree: SceneTree = get_tree()

func _on_debug_prompt_visibility_changed() -> void:
  _set_paused(_debug_prompt.visible)

func _exit_command(_parameters: Array) -> void:
  _tree.quit(0)

func _ready() -> void:
  if OS.is_debug_build():
    _debug_prompt = _debug_prompt_scene.instance()
    var err: int = _debug_prompt.connect("visibility_changed", self, "_on_debug_prompt_visibility_changed")
    assert(!err)
    err = _debug_prompt.register_command("exit", DebugCommand.new(self, "_exit_command", "", "Exit the program."))
    assert(!err)
    add_child(_debug_prompt)

func _set_paused(paused: bool) -> void:
  _tree.paused = paused
  if _debug_prompt:
    _debug_prompt.print_log("Scene is %s." % ("paused" if _tree.paused else "running"))

