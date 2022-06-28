extends LineEdit

export var interpreter: NodePath = ""

var _command_pattern: RegEx = RegEx.new()

onready var _output: TextEdit = $output
onready var _interpreter: Node = get_node(interpreter)

func _init() -> void:
  var err: int = _command_pattern.compile("^\\s*(?<command>[A-Za-z][A-Za-z0-9_]+)(?<parameters>.*)$")
  assert(!err)

func _ready() -> void:
  var err: int = connect("text_entered", self, "_on_text_entered")
  assert(!err)
  err = connect("visibility_changed", self, "_on_visibility_changed")
  assert(!err)
  print_message("Debug Prompt Initialized.")

func print_message(message: String) -> void:
  _output.text += "%s\n" % message
  _output.scroll_vertical = _output.get_line_count()
  print(message)

func clear_output() -> void:
  _output.text = ""

func _input(event: InputEvent) -> void:
  if event.is_action_pressed("ui_toggle_debug_prompt"):
    visible = !visible

func _on_visibility_changed() -> void:
  _interpreter.set_paused(visible)

func _on_text_entered(command_input: String) -> void:
  command_input = command_input.strip_edges()
  print_message("> %s" % command_input)
  text = ""
  var matches: RegExMatch = _command_pattern.search(command_input)
  if matches:
    var command: String = matches.get_string("command")
    if command && _interpreter.has_method(command):
      var params: String = matches.get_string("parameters")
      _interpreter.call_deferred(command, params.strip_edges() if params else "")
      return
  print_message("Invalid command \"%s\"" % command_input)
