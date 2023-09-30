using DDCore;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectBubble.Core.Combat
{
    public class Bubble : MonoBehaviour
    {
        private const float START_SCALE = 0.2f;
        private Collider2D _collider2D;
        private BubbleData _bubbleData;
        private float _travelTime;
        private Vector2 _startPosition;
        private Vector2 _targetPosition;
        private Type _type;
        [SerializeField] private float _movementSpeed;

        public event Action<BubbleData> OnCatch;
        public event Action<BubbleData> OnRelease;
        private void Start()
        {
            _travelTime = 0f;
            _startPosition = transform.position;
            _collider2D = GetComponent<Collider2D>();
            transform.localScale = new Vector3(START_SCALE, START_SCALE, START_SCALE);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            //This will make it slow down near the end of its goal.
            float movementMultiplier = Mathf.Lerp(1f, 0.01f, _travelTime);
            _travelTime += deltaTime * _movementSpeed * movementMultiplier;

            //Wanna also lerp to the max size.
            float scale = 0.2f;
            if(_travelTime > 0.75f)
            {
                float scaleTime = (_travelTime - 0.75f) / 0.25f;
                scale = Mathf.Lerp(scale, 1f, scaleTime);             
            }

            transform.localScale = new Vector3(scale, scale, scale);
            transform.position = Vector2.Lerp(_startPosition, _targetPosition, _travelTime);
            if (_travelTime >= 1f)
            {
                Death();
            }
        }

        public void Fire(BubbleData bubbleData, Type type, Vector3 targetPosition)
        {
            _bubbleData = bubbleData;
            _type = type;
            _targetPosition = targetPosition;
        }

        private void Death()
        {    
            //We need to send the bubble data somewhere though, should we just use a callback?
            //We can VFX later, there will be a thing where it returns to you with the screenshot or pops depending on the bubble type.
            switch (_type)
            {
                case Type.Catch:
                    CalculateBubbledObjects(_bubbleData.BubbledObjects);
                    DisableGameObjects(_bubbleData.BubbledObjects);

                    CalculateBubbledTiles(_bubbleData.BubbledTiles);
                    DestroyOriginalTiles(_bubbleData.BubbledTiles);
                    OnCatch?.Invoke(_bubbleData);
                    break;
                case Type.Release:
                    ReleaseBubbledTiles(_bubbleData.BubbledTiles);
                    ReleaseBubbledObjects(_bubbleData.BubbledObjects);
                    _bubbleData.ClearData();
                    OnRelease?.Invoke(_bubbleData);
                    break;
            }

            Destroy(gameObject);
        }

        public void CalculateScreenshot()
        {
            //TODO:, We'll add this cool little thing at the end.
        }

        public bool IsReleaseValid(List<BubbledTile> bubbledTiles, Vector3 targetPosition)
        {
            Tilemap tilemap = Ground.Map;
            Vector3Int position = new Vector3Int(Mathf.RoundToInt(targetPosition.x), Mathf.RoundToInt(targetPosition.y), 0);
            foreach (BubbledTile bubbledTile in bubbledTiles)
            {
                Vector3Int tilePosition = position + bubbledTile.offset;
                
            }

            //Uh i think we need pathfinding for this?
            return true;
        }

        private void ReleaseBubbledTiles(List<BubbledTile> bubbledTiles)
        {
            Tilemap tilemap = Ground.Map;
            Vector3Int position = new(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
            foreach(BubbledTile bubbledTile in bubbledTiles)
            {
                Vector3Int tilePosition = position + bubbledTile.offset;
                Vector3Int startTilePosition = tilePosition;

                //I'm, just gonna get the nearest tiles lol
                const int SEARCH_RANGE = 10;
                if (tilemap.HasTile(tilePosition))
                {
                    List<Vector3Int> availablePositions = new List<Vector3Int>();
                    for (int x = -SEARCH_RANGE; x < SEARCH_RANGE; x++)
                    {
                        for (int y = -SEARCH_RANGE; y < SEARCH_RANGE; y++)
                        {
                            Vector3Int offset = new Vector3Int(x, y, 0);
                            if(!tilemap.HasTile(tilePosition + offset))
                            {
                                availablePositions.Add(tilePosition + offset);
                            }
                        }
                    }

                    availablePositions = availablePositions.OrderBy(x => Vector3Int.Distance(tilePosition, x)).ToList();
                    tilePosition = availablePositions[0];
                }

                tilemap.SetTile(tilePosition, bubbledTile.tile);
            }

            bubbledTiles.Clear();
        }

        private void ReleaseBubbledObjects(List<BubbledObject> bubbledObjects)
        {
            foreach(BubbledObject bubbledObject in bubbledObjects)
            {
                Vector3 position = transform.position + bubbledObject.offset;
                bubbledObject.gameObject.transform.position = position;
                bubbledObject.gameObject.SetActive(true);
            }
            bubbledObjects.Clear();
        }

        private void CalculateBubbledTiles(List<BubbledTile> bubbledTiles)
        {
            Tilemap tilemap = Ground.Map;
            bubbledTiles.Clear();
            Vector3Int position = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
            foreach (Vector3Int tilePosition in tilemap.cellBounds.allPositionsWithin)
            {
                Vector2 worldPosition = new Vector2(tilePosition.x, tilePosition.y);
                Vector2 worldPositionCenter = worldPosition + new Vector2(0.5f, 0.5f);
                if (_collider2D.OverlapPoint(worldPosition) || _collider2D.OverlapPoint(worldPositionCenter))
                {
                    TileBase tile = tilemap.GetTile(tilePosition);
                    Vector3Int offset = tilePosition - position;
                    BubbledTile bubbledTile = new BubbledTile(tile, tilePosition, offset);
                    bubbledTiles.Add(bubbledTile);
                }
            }
        }

        private void CalculateBubbledObjects(List<BubbledObject> bubbledObjects)
        {
            bubbledObjects.Clear();
            List<Collider2D> collisions = new();
            ContactFilter2D contactFilter2D = new();
            contactFilter2D.NoFilter();

            _collider2D.OverlapCollider(contactFilter2D, collisions);
            foreach (Collider2D col in collisions)
            {
                //Unsure if I need to add this check but whatever.
                if (col.gameObject == gameObject)
                    continue;
                if (col.gameObject.layer == 6)
                    continue;
                if (col.gameObject.layer == 8)
                    continue;
                Vector3 offset = col.gameObject.transform.position - transform.position;
                BubbledObject bubble = new BubbledObject(col.gameObject, offset);
                bubbledObjects.Add(bubble);
            }
        }

        private void DestroyOriginalTiles(List<BubbledTile> bubbledTiles)
        {
            Tilemap tilemap = Ground.Map;
            foreach (var bubbledTile in bubbledTiles)
            {
                tilemap.SetTile(bubbledTile.originalPosition, null);
            }
        }

        private void DisableGameObjects(List<BubbledObject> bubbledObjects)
        {
            foreach(var bubbledObject in bubbledObjects)
            {
                bubbledObject.gameObject.SetActive(false);
            }
        }

        public enum Type
        {
            Catch = 0,
            Release = 1
        }
    }
}
