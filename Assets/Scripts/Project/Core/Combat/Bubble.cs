using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectBubble.Core.Combat
{
    public class Bubble : MonoBehaviour
    {
        private const float START_SCALE = 0.4f;
        private const float END_SCALE = 1f;

        private bool _hasCompletedAction;
        private float _travelTime;

        private Type _type;

        private Vector2 _startPosition;

        private Collider2D _collider2D;
        private List<BubbledObject> _bubbledObjects;
        private List<BubbledTile> _bubbledTiles;

        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _bubbleReleaseDamage;
        [SerializeField] private float _bubbleBurstDamage;
        [SerializeField] private SpriteRenderer _screenshotRenderer;

        [Header("VFX")]
        [SerializeField] private VFX _catchVFX;
        [SerializeField] private VFX _releaseVFX;
        public Movement MoveType { get; set; }
        public Vector2 TargetPosition { get; set; }

        public event Action<Bubble> OnCatch;
        public event Action<Bubble> OnRelease;
        private void Start()
        {
            _bubbledObjects = new();
            _bubbledTiles = new();
            _collider2D = GetComponent<Collider2D>();

            _travelTime = 0f;
            _startPosition = transform.position;
            transform.localScale = new Vector3(START_SCALE, START_SCALE, START_SCALE);
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            switch (MoveType)
            {
                case Movement.Send:
                    Send();
                    break;
                case Movement.Return:
                    Return();
                    break;
            }
        }

        private void Send()
        {
            //This will make it slow down near the end of its goal.

            float scale;
            switch (_type)
            {
                case Type.Catch:
                    float movementMultiplier = Mathf.Lerp(1f, 0.01f, _travelTime);
                    _travelTime += Time.deltaTime * _movementSpeed * movementMultiplier;

                    scale = START_SCALE;
                    if (_travelTime > 0.75f)
                    {
                        float scaleTime = (_travelTime - 0.75f) / 0.25f;
                        scale = Mathf.Lerp(scale, END_SCALE, scaleTime);
                    }

                    transform.localScale = new Vector3(scale, scale, scale);
                    transform.position = Vector2.Lerp(_startPosition, TargetPosition, _travelTime);
                    if (_travelTime >= 1f && !_hasCompletedAction)
                    {
                        _hasCompletedAction = true;
                        DoAction();
                    }
                    break;
                case Type.Release:
                    const float Release_Movement_Multiplier = 1f;
                    const float Release_Scale_Threshold = 0.95f;
                    float travelMultiplier = Release_Movement_Multiplier + (Vector3.Distance(_startPosition, TargetPosition) / 16);
                    _travelTime += Time.deltaTime * _movementSpeed * travelMultiplier;
                   
                    scale = START_SCALE;
                 
                    if (_travelTime > Release_Scale_Threshold)
                    {
                        float scaleTime = (_travelTime - Release_Scale_Threshold) / (1 - Release_Scale_Threshold);
                        scale = Mathf.Lerp(scale, END_SCALE, scaleTime);
                    }
                    transform.localScale = new Vector3(scale, scale, scale);
                    transform.position = Vector2.Lerp(_startPosition, TargetPosition, _travelTime);
                    if (_travelTime >= 1f && !_hasCompletedAction)
                    {
                        _hasCompletedAction = true;
                        DoAction();
                    }
                    break;
            }
        }

        private void Return()
        {
            float movementMultiplier = Mathf.Lerp(1f, 0.01f, _travelTime);
            _travelTime += Time.deltaTime * _movementSpeed * movementMultiplier;

            float scale = END_SCALE;
            if (_travelTime > 0.75f)
            {
                float scaleTime = (_travelTime - 0.75f) / 0.25f;
                scale = Mathf.Lerp(scale, START_SCALE, scaleTime);
            }

            transform.localScale = new Vector3(scale, scale, scale);
            transform.position = Vector2.Lerp(transform.position, TargetPosition, Time.deltaTime * 6f);
        }

        public void Fire(Type type, Vector3 targetPosition)
        {
            _hasCompletedAction = false;
            _type = type;
            _travelTime = 0f;
            _startPosition = transform.position;
            TargetPosition = targetPosition;
            MoveType = Movement.Send;
        }

        private void DoAction()
        {
            //We need to send the bubble data somewhere though, should we just use a callback?
            //We can VFX later, there will be a thing where it returns to you with the screenshot or pops depending on the bubble type.
            switch (_type)
            {
                case Type.Catch:
                    StartCoroutine(CatchRoutine());
                    break;
                case Type.Release:
                    StartCoroutine(ReleaseRoutine());
                    break;
            }
        }

        private IEnumerator CatchRoutine()
        {
            BubbleCamera.MovePosition(transform.position);
            yield return new WaitForEndOfFrame();
            _screenshotRenderer.sprite = BubbleCamera.TakeSnapshot();
            _travelTime = 0f;
            MoveType = Movement.Return;

            CalculateBubbledObjects();
            CalculateBubbledTiles();
            CatchBubbledObjects();
            CatchBubbledTiles();

            _catchVFX?.Play(transform.position);
            OnCatch?.Invoke(this);
        }

        private IEnumerator ReleaseRoutine()
        {
            yield return new WaitForEndOfFrame();
            _screenshotRenderer.sprite = null;
            _travelTime = 0f;
            MoveType = Movement.Return;

            ReleaseBurst();         
            ReleaseBubbledTiles();
            ReleaseBubbledObjects();

            _releaseVFX?.Play(transform.position);
            OnRelease?.Invoke(this);
        }

        public bool HasCatch()
        {
            return _bubbledTiles.Count > 0 || _bubbledObjects.Count > 0;
        }

        private void ReleaseBurst()
        {
            List<Collider2D> collisions = new();
            ContactFilter2D contactFilter2D = new();
            contactFilter2D.NoFilter();

            _collider2D.OverlapCollider(contactFilter2D, collisions);
            foreach (Collider2D col in collisions)
            {
                //Unsure if I need to add this check but whatever.
                if (col.gameObject == gameObject)
                    continue;
                if (col.gameObject.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_bubbleBurstDamage);
                }
            }
        }

        private void ReleaseTile(Vector3Int tilePosition, TileBase tile)
        {
            World.Ground.SetTile(tilePosition, tile);
            World.Undercliff.SetTile(tilePosition + Vector3Int.down, World.UndercliffTile);
            TileBurstManager.FreeTile(tilePosition);
        }

        private void ReleaseBubbledTiles()
        {
            Tilemap tilemap = World.Ground;
            Vector3Int center = tilemap.WorldToCell(transform.position);

            List<Vector3Int> availablePositions = new();
            List<Vector3Int> tileBurstPositions = new();
            foreach (BubbledTile bubbledTile in _bubbledTiles)
            {
                Vector3Int tilePosition = center + bubbledTile.offset;
                Vector3Int startTilePosition = tilePosition;

                //I'm, just gonna get the nearest tiles lol
                const int SEARCH_RANGE = 13;
                if (tilemap.HasTile(tilePosition) || TileBurstManager.IsUsed(tilePosition))
                {
                    availablePositions.Clear();
                    for (int x = -SEARCH_RANGE; x < SEARCH_RANGE; x++)
                    {
                        for (int y = -SEARCH_RANGE; y < SEARCH_RANGE; y++)
                        {
                            Vector3Int offset = new Vector3Int(x, y, 0);
                            Vector3Int proposedTilePosition = startTilePosition + offset;
                            if (TileBurstManager.IsUsed(proposedTilePosition))
                                continue;
                            if (tilemap.HasTile(proposedTilePosition) || tilemap.GetTile(proposedTilePosition) != null)
                                continue;

                            availablePositions.Add(proposedTilePosition);
                        }
                    }

                    availablePositions = availablePositions.OrderBy(x => Vector3Int.Distance(tilePosition, x)).ToList();
                    tilePosition = availablePositions[0]; 
                }

                if (tileBurstPositions.Contains(tilePosition))
                {
                    DebugWrapper.LogError($"Tile Burst Positions already has that! {tilePosition}");
                }
              
                TileBurstManager.UseTile(tilePosition);
                tileBurstPositions.Add(tilePosition);
            }


            for(int i = 0; i < tileBurstPositions.Count; i++)
            {
                Vector3Int tileBurstPosition = tileBurstPositions[i];
                TileBase tile = _bubbledTiles[i].tile;
                TileBurstManager.Burst(transform.position, new Vector3(tileBurstPosition.x + 0.5f, tileBurstPosition.y + 0.5f, 0),
                    () => ReleaseTile(tileBurstPosition, tile));
            }


            tileBurstPositions.Clear();
            _bubbledTiles.Clear();
        }

        private void ReleaseBubbledObjects()
        {
            foreach (BubbledObject bubbledObject in _bubbledObjects)
            {
                Vector3 position = transform.position + bubbledObject.offset;
                bubbledObject.gameObject.transform.position = position;
                if (bubbledObject.gameObject.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_bubbleReleaseDamage);
                }

                bubbledObject.gameObject.SetActive(true);
            }
            _bubbledObjects.Clear();
        }

        private void CalculateBubbledTiles()
        {
            Tilemap tilemap = World.Ground;
            _bubbledTiles.Clear();
            Vector3Int position = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
            foreach (Vector3Int tilePosition in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(tilePosition))
                    continue;
                TileBase tile = tilemap.GetTile(tilePosition);
                if (tile == null)
                    continue;
                Vector2 worldPosition = new Vector2(tilePosition.x, tilePosition.y);
                Vector2 worldPositionCenter = worldPosition + new Vector2(0.5f, 0.5f);
                if (_collider2D.OverlapPoint(worldPosition) || _collider2D.OverlapPoint(worldPositionCenter))
                {
                    Vector3Int offset = tilePosition - position;
                    BubbledTile bubbledTile = new BubbledTile(tile, tilePosition, offset);
                    _bubbledTiles.Add(bubbledTile);
                }
            }
        }

        private void CalculateBubbledObjects()
        {
            _bubbledObjects.Clear();
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
                _bubbledObjects.Add(bubble);
            }
        }

        private void CatchBubbledTiles()
        {
            Tilemap tilemap = World.Ground;
            Tilemap undercliff = World.Undercliff;
            foreach (var bubbledTile in _bubbledTiles)
            {
                tilemap.SetTile(bubbledTile.originalPosition, null);
                undercliff.SetTile(bubbledTile.originalPosition + Vector3Int.down, null);
            }
        }

        private void CatchBubbledObjects()
        {
            foreach (var bubbledObject in _bubbledObjects)
            {
                if (bubbledObject.gameObject.TryGetComponent(out IBubbleReceiver bubbleReceiver))
                {
                    bubbleReceiver.OnBubble(this);
                }
                bubbledObject.gameObject.SetActive(false);

            }
        }

        public enum Type
        {
            Catch = 0,
            Release = 1
        }

        public enum Movement
        {
            Send = 0,
            Return = 1
        }
    }
}
