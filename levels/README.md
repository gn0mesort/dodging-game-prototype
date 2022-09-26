# Levels

## Specifying Levels

Levels are specified in JSON files found under the `res://levels/` directory. Each level is made up of three
components: a metadata object, an entity array, and a level layout array. All of these components must be specified
(even if they are empty) for a functional level.

### Top-level Specification

Each level file must specify a valid object with the following properties: `metadata`, `entities`, and `level`. Valid
levels may also specify a `type` property. If specified `type` must always have the value `"curated"`. A minimal valid
top-level specification looks like this:

```json
{
  "metadata": {},
  "entities": [],
  "level": []
}
```

or

```json
{
  "type": "curated",
  "metadata": {},
  "entities": [],
  "level": []
}
```

Level objects may contain other properties but these will be ignored during loading.

### Metadata

Every level must contain a valid metadata object. A minimal metadata object is empty. However, the following recognized
properties may also be specified:

- `name`: A string containing the name of the level.
- `author`: A string containing the level author's name.
- `contact`: A string containing author contact information.
- `version`: A string indicating the current version of the contained level.

All recognized properties of the metadata object may have the value `null`.

A valid metadata object looks like this:

```json
{
  "name": "My Level",
  "author": "John Doe",
  "contact": "johndoe@email.tld",
  "version": "1.0.0"
}
```

### Entities

Every level must contain a valid entities array. A minimal array is empty (specifying 0 entities). However, it may 
contain an arbitrary number of entity objects. Each entity object should be unique. Valid entities are never `null`.
The entities array is considered to be 1-based (that is the first element has the index `1`) during level definition.

#### Entity

Entity objects specify a Godot scene and a configuration for that scene. A minimal entity contains a `path` property
specifying the Godot resource path of the referenced scene. The value of `path` must be a string and must not be an
empty string (`""`). Additionally, a valid entity may specify the following properties:

- `mode`: A string indicating the movement mode of the entity. The value must be one of `"stationary"`, `"translating"`, and `"scaling"`. If no `mode` is specified the value will default to `"stationary"`.
- `direction_x`: A string indicating the direction of movement along the x-axis. This value is only used when `mode` is specified and has the value `"translating"`. The value must be one of `"none"`, `"left"`, or `"right"`. If no `direction_x` is specified then the value will default to `"none"`.
- `direction_y`: A string indicating the direction of movement along the y-axis. This value is only used when `mode` is specified and has the value `"translating"`. The value must be one of `"none"`, `"up"`, or `"down"`. If no `direction_y` is specified then the value will default to `"none"`.
- `scale_x`: A boolean indicating whether or not the entity should scale along its x-axis. This value is only used when `mode` is specified and has the value `"scaling"`. If no `scale_x` is specified then the value will default to `false`.
- `scale_y`: A boolean indicating whether or not the entity should scale along its y-axis. This value is only used when `mode` is specified and has the value `"scaling"`. If no `scale_y` is specified then the value will default to `false`.

Entity scenes are allowed to interpret their configuration in any way they choose. Valid entity objects look like this:

```json
{ "path": "res://scenes/prefab/entities/Cube.tscn" }
```

```json
{ "path": "res://scenes/prefab/entities/Sphere.tscn", "mode": "translating", "direction_x": "left", "direction_y": "up" }
```

```json
{ "path": "res://scenes/prefab/entities/Letterbox.tscn", "mode": "scaling" }
```

### Level

Every level must contain a valid array describing its layout. This is specified as an array of 3x3 matrices. Matrix
values are positive integer indices into the `entities` array. Each matrix must contain exactly 9 values. To define 
empty space the value `0` is reserved. A valid matrix looks like this:

```json
[
 0, 0, 0
 0, 1, 0,
 2, 0, 3
]
```

In this case the matrix describes a single volume of 3D space containing: an empty top-row, the first entity listed in
the `entities` array in the center of the center row, and the second and third entities listed in the `entity` array 
in the bottom row. A valid complete level description looks like this:

```json
[
  [
    0, 0, 0
    0, 1, 0,
    2, 0, 3
  ]
]
```

### Complete Example Level

The following is a complete loadable example level:

```json
{
  "metadata": {
    "name": "Example",
    "author": "John Doe",
    "contact": "johndoe@email.tld",
    "version": "1.0.0"
  },
  "entities": [
    { "path": "res://scenes/prefab/entities/Cube.tscn" },
    { "path": "res://scenes/prefab/entities/Letterbox.tscn", "mode": "scaling" },
    { "path": "res://scenes/prefab/entities/VerticalLetterbox.tscn", "mode": "scaling" }
  ],
  "level": [
    [
      0, 0, 0,
      0, 0, 0,
      0, 0, 0
    ],
    [
      0, 0, 0,
      0, 0, 0,
      0, 0, 0
    ],
    [
      0, 0, 0,
      0, 0, 0,
      0, 0, 0
    ],
    [
      0, 0, 0,
      0, 2, 0,
      0, 0, 0
    ],
    [
      0, 1, 0,
      1, 0, 1,
      0, 1, 0
    ],
    [
      0, 1, 0,
      1, 0, 1,
      0, 1, 0
    ],
    [
      0, 1, 0,
      1, 0, 1,
      0, 1, 0
    ],
    [
      0, 1, 0,
      1, 0, 1,
      0, 1, 0
    ],
    [
      0, 1, 0,
      1, 0, 1,
      0, 1, 0
    ],
    [
      0, 1, 0,
      1, 0, 1,
      0, 1, 0
    ],
    [
      0, 0, 0,
      0, 3, 0,
      0, 0, 0
    ],
    [
      0, 0, 0,
      0, 0, 0,
      0, 0, 0
    ]
  ]
}
```

## Loading Levels

Currently, the level loaded by the title menu scene is hard coded. Therefore, the easiest way to load a level is to
place the level file in the `levels` directory and load it through the debug menu. To load a level through the debug
menu simply type `load_level` followed by the filename (without the `.json` extension). For example, 
`load_level tunnel` will load the hardcoded Tunnel demo level and `load_level example` will load the example level
provided above.
