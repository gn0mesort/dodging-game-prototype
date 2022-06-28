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

var _debug_prompt_scene: PackedScene = preload("res://scenes/debug_prompt.tscn")
var _debug_prompt: Control = null

func _ready() -> void:
  if OS.is_debug_build():
    _debug_prompt = _debug_prompt_scene.instance()
    _debug_prompt.interpreter = get_path()
    add_child(_debug_prompt)

func pause_scene() -> void:
  set_paused(true)

func resume_scene() -> void:
  set_paused(false)

func set_paused(paused: bool) -> void:
  var tree: SceneTree = get_tree()
  tree.paused = paused
  _debug_prompt.print_message("Scene is %s." % ("paused" if tree.paused else "running"))

func hide_node(args: String) -> void:
  get_tree().root.get_node(args).hide()

func show_node(args: String) -> void:
  get_tree().root.get_node(args).hide()

func echo(args: String) -> void:
  _debug_prompt.print_message(args)

func clear(_args: String) -> void:
  _debug_prompt.clear_output()
