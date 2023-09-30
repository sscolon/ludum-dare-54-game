using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DDCore
{
    /// <summary>
    /// A utility class that contains a lot of helper functions for implementing game logic.
    /// </summary>
    public static class Util
    {
        public const float PIXELS_PER_UNIT = 1 / 16f;

        private static UnityEngine.Camera _mainCamera;

        /// <summary>
        /// Returns the main camera, which is the one following Skye.
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.Camera GetMainCamera()
        {
            if (_mainCamera == null)
                _mainCamera = UnityEngine.Camera.main;
            return _mainCamera;
        }

        /// <summary>
        /// Returns the world position of the camera, this will completely disregard the z-axis.
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMainCameraWorldPosition()
        {
            Vector3 position = GetMainCamera().transform.position;
            position.z = 0.0f;
            return position;
        }

        private static BindingFlags _flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        public static FieldInfo[] GetFields(object obj)
        {
            List<FieldInfo> fieldInfo = new List<FieldInfo>();
            Type type = obj.GetType();

            while (type != null)
            {
                fieldInfo.AddRange(type.GetFields(_flags));
                type = type.BaseType;
            }

            return fieldInfo.ToArray();
        }

        public static PropertyInfo[] GetProperties(object obj)
        {
            List<PropertyInfo> propertyInfo = new List<PropertyInfo>();
            Type type = obj.GetType();
            while (type != null)
            {
                propertyInfo.AddRange(type.GetProperties(_flags));
                type = type.BaseType;
            }

            return propertyInfo.ToArray();
        }

        /// <summary>
        /// Returns the world position of the mouse.
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 position = GetMainCamera().ScreenToWorldPoint(Input.mousePosition);
            position.z = 0.0f;
            return position;
        }

        /// <summary>
        /// Returns an array of tiles that would fit a bounds. Used by stuff like hallway rendering.
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="tile"></param>
        /// <returns></returns>
        public static TileBase[] Block(BoundsInt bounds, TileBase tile)
        {
            TileBase[] tiles = new TileBase[bounds.size.x * bounds.size.y * bounds.size.z];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = tile;
            }
            return tiles;
        }

        /// <summary>
        /// Repaints a sprite to a new texture, this will not be entirely accurate to the original sprite.
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public static Texture2D SpriteToTexture(Sprite sprite)
        {
            if (sprite == null)
                return null;
            int width = (int)sprite.textureRect.width;
            int height = (int)sprite.textureRect.height;

            int textureX = (int)sprite.textureRect.x;
            int textureY = (int)sprite.textureRect.y;

            Texture2D originalTexture = sprite.texture;
            RenderTexture renderTexture = RenderTexture.GetTemporary(originalTexture.width, originalTexture.height);
            UnityEngine.Graphics.Blit(originalTexture, renderTexture);


            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(new Vector2(textureX, textureY), new Vector2Int(width, height)), 0, 0);
            texture.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(renderTexture);
            return texture;
        }

        /// <summary>
        /// Resizes a texture by scaling it up.
        /// </summary>
        /// <param name="originalTexture"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        public static Texture2D ResizeTexture(Texture2D originalTexture, int newWidth, int newHeight)
        {
            RenderTexture renderTexture = RenderTexture.GetTemporary(newWidth, newHeight);
            UnityEngine.Graphics.Blit(originalTexture, renderTexture);

            Texture2D texture = new Texture2D(newWidth, newHeight, TextureFormat.ARGB32, false);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(Vector2.zero, new Vector2Int(newWidth, newHeight)), 0, 0);
            texture.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(renderTexture);
            return texture;
        }

        /// <summary>
        /// Returns the angle that an object must face to look at the target position. This is used mainly for projectiles.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Quaternion GetAngle(Vector3 start, Vector3 end)
        {
            Vector3 dir = end - start;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        /// <summary>
        /// Converts a direction to an angle. This is used mainly for projectiles.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Quaternion GetAngle(Vector3 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        /// <summary>
        /// Rotates a point around a specific pivot. This is mainly used for projectiles.
        /// </summary>
        /// <param name="point">The point to move.</param>
        /// <param name="pivot">The pivot to move the point around.</param>
        /// <param name="rot">The amount to rotate it.</param>
        /// <returns></returns>
        public static Vector3 RotateAroundPivot(Vector3 point, Vector3 pivot, Quaternion rot)
        {
            Vector3 dir = point - pivot;
            dir = rot * dir;
            point = dir + pivot;
            return point;
        }

        /// <summary>
        /// Rotates a point around a specific pivot.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pivot"></param>
        /// <param name="angles"></param>
        /// <returns></returns>
        public static Vector3 RotateAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            var dir = point - pivot;
            dir = Quaternion.Euler(angles) * dir;
            point = dir + pivot;
            return point;
        }

        /// <summary>
        /// Returns the transform closest to a reference position.
        /// </summary>
        /// <param name="transforms"></param>
        /// <param name="referencePosition"></param>
        /// <returns></returns>
        public static Transform NearestTransform(List<Transform> transforms, Vector3 referencePosition)
        {
            Transform closest = null;
            float distance = float.MaxValue;
            foreach (Transform transform in transforms)
            {
                if (transform == null)
                    continue;
                float dist = Vector3.Distance(referencePosition, transform.position);
                if (dist <= distance)
                {
                    closest = transform;
                    distance = dist;
                }
            }

            return closest;
        }

        /// <summary>
        /// Separates every capital letter in a string with a space.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="toLowercase"></param>
        /// <returns></returns>
        public static string SplitByCapital(this string s, bool toLowercase)
        {
            if (toLowercase)
            {
                var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

                return r.Replace(s, "_").ToLower();
            }
            else
            {
                return s;
            }
        }

        public static int GetDamerauLevenshteinDistance(string s, string t)
        {
            var bounds = new { Height = s.Length + 1, Width = t.Length + 1 };

            int[,] matrix = new int[bounds.Height, bounds.Width];

            for (int height = 0; height < bounds.Height; height++) { matrix[height, 0] = height; };
            for (int width = 0; width < bounds.Width; width++) { matrix[0, width] = width; };

            for (int height = 1; height < bounds.Height; height++)
            {
                for (int width = 1; width < bounds.Width; width++)
                {
                    int cost = (s[height - 1] == t[width - 1]) ? 0 : 1;
                    int insertion = matrix[height, width - 1] + 1;
                    int deletion = matrix[height - 1, width] + 1;
                    int substitution = matrix[height - 1, width - 1] + cost;

                    int distance = Math.Min(insertion, Math.Min(deletion, substitution));

                    if (height > 1 && width > 1 && s[height - 1] == t[width - 2] && s[height - 2] == t[width - 1])
                    {
                        distance = Math.Min(distance, matrix[height - 2, width - 2] + cost);
                    }

                    matrix[height, width] = distance;
                }
            }

            return matrix[bounds.Height - 1, bounds.Width - 1];
        }

        /// <summary>
        /// Creates empty instances of all classes that inherit the given type, using reflection. 
        /// <br>This will only work for classes that have an empty constructor.</br>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] InstantiateTypesOf<T>()
        {
            IEnumerable<Type> types = FindTypesOf<T>();
            List<T> instances = new List<T>();
            foreach (var type in types)
            {
                T instance = (T)Activator.CreateInstance(type);
                instances.Add(instance);
            }

            return instances.ToArray();
        }

        /// <summary>
        /// Uses LINQ to find every class that inherits the given type.
        /// <br>This ignores abstractions and interfaces.</br>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> FindTypesOf<T>()
        {
            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract && !p.IsInterface);
            return types;
        }

        /// <summary>
        /// Returns an up vector shifted by a number of pixels.
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static Vector3 PixelShiftUp(float pixels)
        {
            return Vector3.up * (pixels * PIXELS_PER_UNIT);
        }

        /// <summary>
        /// Returns a down vector shifted by a number of pixels.
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static Vector3 PixelShiftDown(float pixels)
        {
            return Vector3.down * (pixels * PIXELS_PER_UNIT);
        }

        /// <summary>
        /// Returns a left vector shifted by a number of pixels.
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static Vector3 PixelShiftLeft(float pixels)
        {
            return Vector3.left * (pixels * PIXELS_PER_UNIT);
        }

        /// <summary>
        /// Returns a right vector shifted by a number of pixels.
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static Vector3 PixelShiftRight(float pixels)
        {
            return Vector3.right * (pixels * PIXELS_PER_UNIT);
        }

        public static Vector3Int FloorToInt(Vector3 vector)
        {
            return new Vector3Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y), Mathf.FloorToInt(vector.z));
        }

        public static Vector2Int FloorToInt(Vector2 vector)
        {
            return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
        }

        public static Vector3Int RoundToInt(Vector3 vector)
        {
            return new Vector3Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
        }

        public static Vector2Int RoundToInt(Vector2 vector)
        {
            return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
        }


        /// <summary>
        /// Lerp a curve between an array of vectors.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="keyFrames"></param>
        /// <returns></returns>
        public static Vector3 Curve(float time, params Vector3[] keyFrames)
        {
            Vector3 curve = keyFrames[0];
            for (int i = 0; i < keyFrames.Length; i++)
            {
                Vector3 next = keyFrames[i];
                curve = Vector3.Lerp(curve, next, time);
            }

            return curve;
        }

        /// <summary>
        /// Smoothly step between two vectors.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 SmoothStep(Vector3 start, Vector3 end, float t)
        {
            float x = Mathf.SmoothStep(start.x, end.x, t);
            float y = Mathf.SmoothStep(start.y, end.y, t);
            float z = Mathf.SmoothStep(start.z, end.z, t);
            return new Vector3(x, y, z);
        }
        public static Vector3 InverseTransformPoint(Vector3 transforPos, Quaternion transformRotation, Vector3 transformScale, Vector3 pos)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(transforPos, transformRotation, transformScale);
            Matrix4x4 inverse = matrix.inverse;
            return inverse.MultiplyPoint3x4(pos);
        }

        #region Tiles & Tilemaps

        /// <summary>
        /// Rounds a vector to a tile by flooring it to an int and adding 0.5f.
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public static Vector3 RoundToTile(Vector3 vector3)
        {
            Vector3 roundedVector = new Vector3(Mathf.FloorToInt(vector3.x), Mathf.FloorToInt(vector3.y), 0);
            return roundedVector + new Vector3(0.5f, 0.5f);
        }

        /// <summary>
        /// Copies all tiles from a tilemap to a target tilemap.
        /// <br>This copy will ignore any tiles that already exist on the target tilemap, merging the two.</br>
        /// <br>This is mainly used for rendering rooms to the world.</br>
        /// </summary>
        /// <param name="start"></param>
        /// <param name="target"></param>
        /// <param name="worldPosition"></param>
        /// <param name="overrideTile"></param>
        /// <param name="color"></param>
        public static void CopyTilemap(Tilemap start, Tilemap target, Vector3Int worldPosition, TileBase overrideTile = null, Color color = default)
        {
            if (start == null || target == null)
                return;

            BoundsInt bounds = start.cellBounds;
            bounds.position += worldPosition;
            TileBase[] tiles = new TileBase[bounds.size.x * bounds.size.y * bounds.size.z];
            int index = 0;
            foreach (var position in start.cellBounds.allPositionsWithin)
            {
                Vector3Int worldCell = position + worldPosition;
                if (target.HasTile(worldCell))
                {
                    tiles[index] = target.GetTile(worldCell);
                }
                else
                {
                    TileBase tile = start.GetTile(position);
                    if (tile != null && overrideTile != null)
                    {
                        tiles[index] = overrideTile;
                    }
                    else
                    {
                        tiles[index] = tile;
                    }
                }
                index++;
            }

            target.SetTilesBlock(bounds, tiles);
            if (color != default)
            {
                foreach (var position in start.cellBounds.allPositionsWithin)
                {
                    Vector3Int worldCell = position + worldPosition;
                    target.SetTileFlags(worldCell, TileFlags.None);
                    target.SetColor(worldCell, color);
                }
            }
        }

        /// <summary>
        /// Clears all tiles from a tilemap to a target tilemap.
        /// <br>This will ONLY clear tiles that exist in positions on the map being used as a reference for clearing.</br>
        /// </summary>
        /// <param name="start"></param>
        /// <param name="target"></param>
        /// <param name="worldPosition"></param>
        /// <param name="overrideTile"></param>
        /// <param name="color"></param>
        public static void ClearTilemap(Tilemap start, Tilemap target, Vector3Int worldPosition)
        {
            if (start == null || target == null)
                return;

            BoundsInt bounds = start.cellBounds;
            bounds.position += worldPosition;
            TileBase[] tiles = new TileBase[bounds.size.x * bounds.size.y * bounds.size.z];
            int index = 0;

            //Basically what this is doing is only deleting the tiles if it matches what the room expects it to be.
            //This means the clear local tilemap should never delete tiles from other rooms.
            //So for example, if a tile is expected to be air but is not, it will assume that another room did that.

            //Not sure if this will fix the issue, but maybe it will.
            foreach (var position in start.cellBounds.allPositionsWithin)
            {
                Vector3Int worldCell = position + worldPosition;
                TileBase baseTile = start.GetTile(position);
                TileBase worldTile = target.GetTile(worldCell);
                tiles[index] = baseTile == worldTile ? null : worldTile;
                index++;
            }

            target.SetTilesBlock(bounds, tiles);
        }

        public static Vector3Int ClosestTile(List<Tilemap> tilemaps, Vector3 referencePosition)
        {
            Vector3Int referenceTile = FloorToInt(referencePosition);
            Vector3Int closestTile = Vector3Int.zero;
            float distance = float.MaxValue;

            foreach (Tilemap tilemap in tilemaps)
            {
                BoundsInt bounds = tilemap.cellBounds;
                foreach (Vector3Int tile in bounds.allPositionsWithin)
                {
                    if (!tilemap.HasTile(tile))
                        continue;

                    float dist = Vector3Int.Distance(referenceTile, tile);
                    if (dist <= distance)
                    {
                        closestTile = tile;
                        distance = dist;
                    }
                }
            }

            return closestTile;
        }

        #endregion

        #region Extensions
        /// <summary>
        /// Changes the alpha of a color to the given value.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Color ChangeAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        /// <summary>
        /// Replaces all empty spaces in a string with underscores.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string ToLowerUnderscore(this string content)
        {
            string underscore = content.ToLower();
            underscore = underscore.Replace(" ", "_");
            return underscore;
        }
        #endregion

        #region Circle Drawing

        private static Dictionary<int, Sprite> _radiusIndex = new Dictionary<int, Sprite>();
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ClearRadiusIndex()
        {
            _radiusIndex = new Dictionary<int, Sprite>();
        }
        public static Texture2D DrawCircle(this Texture2D tex, Color color, int x, int y, int radius = 3)
        {
            float rSquared = radius * radius;

            for (int u = x - radius; u < x + radius + 1; u++)
            {
                for (int v = y - radius; v < y + radius + 1; v++)
                {
                    if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared)
                    {
                        tex.SetPixel(u, v, color);
                    }
                    else
                    {
                        tex.SetPixel(u, v, Color.clear);
                    }
                }
            }

            return tex;
        }


        public static Texture2D DrawCircleClear(this Texture2D tex, Color color, int x, int y, int radius = 3)
        {
            float rSquared = radius * radius;

            for (int u = x - radius; u < x + radius + 1; u++)
            {
                for (int v = y - radius; v < y + radius + 1; v++)
                {
                    if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared)
                    {
                        tex.SetPixel(u, v, color);
                    }
                    else
                    {
                        tex.SetPixel(u, v, tex.GetPixel(u, v));
                    }
                }
            }

            return tex;
        }

        public static Sprite DrawCircle(int radius)
        {
            if (_radiusIndex.ContainsKey(radius))
            {
                return _radiusIndex[radius];
            }

            Texture2D circleTexture = new Texture2D(radius, radius);
            circleTexture = circleTexture.DrawCircle(Color.white, radius / 2, radius / 2, radius / 2);
            circleTexture.filterMode = FilterMode.Point;
            circleTexture.Apply();

            Sprite radiusSprite = Sprite.Create(circleTexture, new Rect(0, 0, circleTexture.width, circleTexture.height), new Vector2(0.5f, 0.5f), 16f);
            _radiusIndex.Add(radius, radiusSprite);
            return radiusSprite;
        }

        public static Sprite DrawCircle(int radius, Vector2 pivot)
        {
            if (_radiusIndex.ContainsKey(radius))
            {
                return _radiusIndex[radius];
            }

            Texture2D circleTexture = new Texture2D(radius, radius);
            circleTexture = circleTexture.DrawCircle(Color.white, radius / 2, radius / 2, radius / 2);
            circleTexture.filterMode = FilterMode.Point;
            circleTexture.Apply();

            Sprite radiusSprite = Sprite.Create(circleTexture, new Rect(0, 0, circleTexture.width, circleTexture.height), pivot, 16f);
            _radiusIndex.Add(radius, radiusSprite);
            return radiusSprite;
        }

        public static Sprite DrawCircleOutline(int radiusPixels, int innerRadiusPixels)
        {
            if (_radiusIndex.ContainsKey(radiusPixels))
            {
                return _radiusIndex[radiusPixels];
            }

            Texture2D circleTexture = new Texture2D(radiusPixels, radiusPixels);

            int centerPixel = radiusPixels / 2;
            circleTexture = circleTexture.DrawCircle(Color.white, centerPixel, centerPixel, radiusPixels / 2);
            circleTexture = circleTexture.DrawCircleClear(Color.clear, centerPixel, centerPixel, innerRadiusPixels / 2);
            circleTexture.filterMode = FilterMode.Point;
            circleTexture.Apply();

            Sprite radiusSprite = Sprite.Create(circleTexture, new Rect(0, 0, circleTexture.width, circleTexture.height), new Vector2(0.5f, 0.5f), 16f);
            _radiusIndex.Add(radiusPixels, radiusSprite);
            return radiusSprite;
        }
        #endregion
    }
}
