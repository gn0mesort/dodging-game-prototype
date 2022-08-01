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
using System.Text.Json;
using System.Text.Json.Serialization;

public class Level {
  public class LevelMetadata {
    [JsonPropertyName("author")]
    public string AuthorName { get; set; }
    [JsonPropertyName("contact")]
    public string AuthorContact { get; set; }
    [JsonPropertyName("version")]
    public string Version { get; set; }

    /**
     * Convert LevelMetadata to a string.
     *
     * @returns A string representing the LevelMetadata.
     */
    public override string ToString() {
      return $"{{ AuthorName = {AuthorName}, AuthorContact = {AuthorContact}, Version = {Version} }}";
    }
  }

  public class LevelData {
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("top")]
    public int[] TopLayer { get; set; }
    [JsonPropertyName("middle")]
    public int[] MiddleLayer { get; set; }
    [JsonPropertyName("bottom")]
    public int[] BottomLayer { get; set; }

    /**
     * Convert LevelData to a string.
     *
     * @returns A string representing the LevelData.
     */
    public override string ToString() {
      return $"{{ Name = {Name}, TopLayer = {TopLayer}, MiddleLayer = {MiddleLayer}, BottomLayer = {BottomLayer} }}";
    }
  }

  public enum LevelType {
    Curated
  }

  [JsonPropertyName("type")]
  public LevelType Type { get; set; }
  [JsonPropertyName("metadata")]
  public LevelMetadata Metadata { get; set; }
  [JsonPropertyName("level")]
  public LevelData Data { get; set; }

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
    return $"{{ Type = {Type}, Metadata = {Metadata}, Data = {Data} }}";
  }
}
