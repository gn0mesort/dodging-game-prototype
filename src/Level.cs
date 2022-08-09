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
using Godot;

public class Level {
  public class LevelMetadata {
    [JsonPropertyName("name")]
    public string LevelName { get; private set; }
    [JsonPropertyName("author")]
    public string AuthorName { get; private set; }
    [JsonPropertyName("contact")]
    public string AuthorContact { get; private set; }
    [JsonPropertyName("version")]
    public string Version { get; private set; }

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
    public enum EntityMode {
      Stationary,
      Translating,
      Scaling
    }
    public enum Direction {
      None,
      Up,
      Left,
      Down,
      Right
    }
    [JsonPropertyName("path")]
    public string ScenePath { get; private set; }
    [JsonIgnore]
    public PackedScene Scene { get; private set; }
    [JsonPropertyName("mode")]
    public EntityMode Mode { get; private set; }
    [JsonPropertyName("direction_x")]
    public Direction DirectionX { get; private set; }
    [JsonPropertyName("direction_y")]
    public Direction DirectionY { get; private set; }

    [JsonConstructor]
    public LevelEntity(string scenePath, EntityMode mode, Direction directionX, Direction directionY) {
      ScenePath = scenePath;
      Scene = GD.Load<PackedScene>(ScenePath);
      Mode = mode;
      DirectionX = directionX;
      DirectionY = directionY;
    }
    /**
     * Convert LevelData to a string.
     *
     * @returns A string representing the LevelData.
     */
    public override string ToString() {
      return $"{{ ScenePath = {ScenePath}, Scene = {Scene}, Mode = {Mode}, DirectionX = {DirectionX}, DirectionY = {DirectionY} }}";
    }
  }

  public enum LevelType {
    Curated
  }

  [JsonPropertyName("type")]
  public LevelType Type { get; private set; }
  [JsonPropertyName("metadata")]
  public LevelMetadata Metadata { get; private set; }
  [JsonPropertyName("entities")]
  public LevelEntity[] Entities { get; private set; }
  [JsonPropertyName("level")]
  public IList<IList<int>> Data { get; private set; }

  [JsonConstructor]
  public Level(LevelType type, LevelMetadata metadata, LevelEntity[] entities, IList<IList<int>> data) {
    Type = type;
    Metadata = metadata;
    Entities = entities;
    Data = data;
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
