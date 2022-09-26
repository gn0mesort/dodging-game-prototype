/** Level Data Objects.
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
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;
using Godot;

public class Level {
  public class LevelMetadata {
    /**
     * The name of the level.
     */
    [JsonPropertyName("name")]
    public string LevelName { get; private set; }

    /**
     * The author of the level.
     */
    [JsonPropertyName("author")]
    public string AuthorName { get; private set; }

    /**
     * Contact information for the author. This should be an email address but it's an arbitrary string so it might
     * contain other information too.
     */
    [JsonPropertyName("contact")]
    public string AuthorContact { get; private set; }

    /**
     * The version of the level.
     */
    [JsonPropertyName("version")]
    public string Version { get; private set; }

    /**
     * Construct a new level object.
     *
     * @param levelName The name of the level.
     * @param authorName The author of the level.
     * @param authorContact Contact information for the level author.
     * @param version The version of the level.
     */
    [JsonConstructor]
    public LevelMetadata(string levelName, string authorName, string authorContact, string version) {
      LevelName = levelName;
      AuthorName = authorName;
      AuthorContact = authorContact;
      Version = version;
    }

    /**
     * Convert LevelMetadata to a string.
     *
     * @returns A string representing the LevelMetadata.
     */
    public override string ToString() {
      return $"{{ LevelName = {LevelName}, AuthorName = {AuthorName}, AuthorContact = {AuthorContact}, Version = {Version} }}";
    }
  }

  public class LevelEntity {
    /**
     * An enum representing different entity movement modes.
     */
    public enum EntityMode {
      Stationary,
      Translating,
      Scaling
    }

    /**
     * An enum representing different directions of movement for Translating entities.
     */
    public enum Direction {
      None,
      Up,
      Left,
      Down,
      Right
    }

    /**
     * The Godot resource path (e.g., res://scenes/scene.tscn) of the entity being described. This is the scene that
     * will actually be loaded into memory for the entity.
     */
    [JsonPropertyName("path")]
    public string ScenePath { get; private set; }

    /**
     * The movement mode of the entity.
     */
    [JsonPropertyName("mode")]
    public EntityMode Mode { get; private set; }

    /**
     * The x-axis direction of a translating entity.
     */
    [JsonPropertyName("direction_x")]
    public Direction DirectionX { get; private set; }

    /**
     * The y-axis direction of a translating entity.
     */
    [JsonPropertyName("direction_y")]
    public Direction DirectionY { get; private set; }

    /**
     * Whether or not to scale on the x-axis.
     */
    [JsonPropertyName("scale_x")]
    public bool ScaleX { get; private set; }

    /**
     * Whether or not to scale on the y-axis.
     */
    [JsonPropertyName("scale_y")]
    public bool ScaleY { get; private set; }

    /**
     * Constructs a new LevelEntity.
     *
     * @param scenePath The Godot resource path of the entity scene.
     * @param mode The movement mode of the entity.
     * @param directionX The x-axis direction for translating entities.
     * @param directionY The y-axis direction for translating entities.
     * @param scaleX Whether or not to scale on the x-axis.
     * @param scaleY Whether or not to scale on the y-axis.
     */
    [JsonConstructor]
    public LevelEntity(string scenePath, EntityMode mode, Direction directionX, Direction directionY, bool scaleX,
                       bool scaleY) {
      ScenePath = scenePath;
      Debug.Assert(ScenePath != null);
      Debug.Assert(!ScenePath.Empty());
      Mode = mode;
      DirectionX = directionX;
      Debug.Assert(DirectionX == Direction.None || DirectionX == Direction.Left || DirectionX == Direction.Right);
      DirectionY = directionY;
      Debug.Assert(DirectionY == Direction.None || DirectionY == Direction.Up || DirectionY == Direction.Down);
      ScaleX = scaleX;
      ScaleY = scaleY;
    }

    /**
     * Convert LevelData to a string.
     *
     * @returns A string representing the LevelData.
     */
    public override string ToString() {
      return $"{{ ScenePath = {ScenePath}, Mode = {Mode}, DirectionX = {DirectionX}, DirectionY = {DirectionY}, " +
             $"ScaleX = {ScaleX}, ScaleY = {ScaleY} }}";
    }
  }

  /**
   * An enum for types of levels.
   *
   * Currently only curated levels exist.
   */
  public enum LevelType {
    Curated
  }

  /**
   * The type of level.
   */
  [JsonPropertyName("type")]
  public LevelType Type { get; private set; }

  /**
   * Metadata describing the level.
   */
  [JsonPropertyName("metadata")]
  public LevelMetadata Metadata { get; private set; }

  /**
   * A dictionary containing loaded Godot scenes for each entity described by Entities.
   */
  [JsonIgnore]
  public Dictionary<string, PackedScene> Scenes { get; private set; } = new Dictionary<string, PackedScene>();

  /**
   * An array of entity descriptions.
   */
  [JsonPropertyName("entities")]
  public LevelEntity[] Entities { get; private set; }

  /**
   * A description of the level geometry.
   */
  [JsonPropertyName("level")]
  public IList<IList<int>> Data { get; private set; }

  [JsonConstructor]
  public Level(LevelType type, LevelMetadata metadata, LevelEntity[] entities, IList<IList<int>> data) {
    Type = type;
    Metadata = metadata;
    Debug.Assert(Metadata != null);
    Entities = entities;
    Debug.Assert(Entities != null);
    foreach (var entity in Entities)
    {
      if (!Scenes.ContainsKey(entity.ScenePath))
      {
        Scenes[entity.ScenePath] = GD.Load<PackedScene>(entity.ScenePath);
        Debug.Assert(Scenes[entity.ScenePath] != null);
      }
    }
    Data = data;
    Debug.Assert(Data != null);
  }

  /**
   * Convert a JSON string into a Level.
   *
   * JSON property names are case insensitive.
   *
   * @param json A string containing a valid JSON Level representation.
   * @retuns A corresponding Level object.
   */
  public static Level fromJson(string json) {
    Debug.Assert(json != null);
    Debug.Assert(!json.Empty());
    var serializerOptions = new JsonSerializerOptions();
    serializerOptions.PropertyNameCaseInsensitive = true;
    serializerOptions.Converters.Add(new JsonStringEnumConverter(new LowercaseJsonNamingPolicy()));
    return JsonSerializer.Deserialize<Level>(json, serializerOptions);
  }

  /**
   * Converts a Level to a string.
   *
   * @returns A string representing the Level.
   */
  public override string ToString() {
    var entities = "";
    foreach (var entity in Entities)
    {
      entities += $"{entity}, ";
    }
    var data = "";
    foreach (var val in Data)
    {
      data += "[";
      foreach (var innerVal in val)
      {
        data += $"{innerVal}, ";
      }
      data += "], ";
    }
    return $"{{ Type = {Type}, Metadata = {Metadata}, Entities = [{entities}], Data = [{data}] }}";
  }
}
