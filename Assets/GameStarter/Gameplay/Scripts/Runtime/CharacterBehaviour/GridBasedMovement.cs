using UnityEngine;
using UnityGameStarter.Gameplay.GridSystem;

namespace UnityGameStarter.Gameplay.CharacterMovement 
{
    public class GridBasedMovement
    {
        private SnappableGridManager _gridManager;

        private readonly GameObject _owner;
        private string _currentLayer = "Default";
        private string[] _blockLayers = { "Wall" };
        
        private Vector3Int _currentGridID = Vector3Int.zero;
        private Vector3Int _currentCoord = Vector3Int.zero;

        private bool _isMoving = false;
        private Vector2 _movingTarget = Vector2.zero;
        private bool _initiallySnapped = false;

        public SnappableGridManager GridManager => _gridManager;

        public string CurrentLayer => _currentLayer;
        public Vector3Int CurrentGridID => _currentGridID;
        public Vector3Int CurrentCoord => _currentCoord;

        public GridSystem.Grid CurrentGrid 
        {
            get 
            {
                if (!IsValidGrid() || string.IsNullOrEmpty(_currentLayer))
                    return null;

                if (!GridManager.Source.TryGetGridByLayerName(_currentLayer, out var grid))
                    return null;

                return grid;
            }
        }

        public GridSystem.Grid[] BlockGrids 
        {
            get 
            {
                if (!IsValidGrid()) return null;

                System.Collections.Generic.List<GridSystem.Grid> grids = new();
                
                foreach (string layer in _blockLayers) 
                {
                    if (string.IsNullOrWhiteSpace(layer)) continue;

                    if (!GridManager.Source.TryGetGridByLayerName(layer, out var grid))
                        continue;
                    
                    grids.Add(grid);
                }

                return grids.ToArray();
            }
        }

        public GridBasedMovement(GameObject owner, SnappableGridManager manager, string[] blockLayers) 
        {
            _owner = owner;

            SetToGrid(manager, blockLayers);
        }

        public void SetToGrid(SnappableGridManager manager, string[] blockLayers) 
        {
            _gridManager = manager;
            _blockLayers = blockLayers ?? System.Array.Empty<string>();
        }

        public void Move(Vector2 input, float maxDistanceDelta, bool isPaused = false,
            bool releaseCurrentCellOnMovingStart = false)
        {
            if (!IsValidGrid()) return;

            if (!_initiallySnapped) return;

            if (!_isMoving)
            {
                var direction = Direction(input);

                if (direction == Vector2.zero) return;

                _movingTarget = (Vector2)_owner.transform.position + direction * GridManager.Source.CellSize;

                if (!CanMoveToTarget())
                {
                    _isMoving = false;
                    return;
                }

                if (releaseCurrentCellOnMovingStart)
                    ReleaseCurrentCell();

                _isMoving = true;
            }

            if (isPaused) return;

            _owner.transform.position = Vector2.MoveTowards(_owner.transform.position, _movingTarget, maxDistanceDelta);

            if (Vector2.Distance(_owner.transform.position, _movingTarget) < 0.001f)
            {
                if (!releaseCurrentCellOnMovingStart)
                    ReleaseCurrentCell();

                SnapOwner();
                _isMoving = false;
            }
        }

        public void SnapOwner()
        {
            if (!IsValidGrid()) return;
            
            GridManager.Snapper.TrySnapToGrid(_owner, _currentLayer, out _, out _);
        }

        public void OnSnapped(string snappedLayer, Vector3Int gridID, Vector3Int coord)
        {
            _initiallySnapped = true;

            _currentLayer = snappedLayer;
            _currentGridID = gridID;
            _currentCoord = coord;
        }

        public Vector2 Direction(Vector2 input) 
        {
            if (input.x > 0) return Vector2.right;
            else if (input.x < 0) return Vector2.left;
            else if (input.y > 0) return Vector2.up;
            else if (input.y < 0) return Vector2.down;
            else return Vector2.zero;
        }

        public void ReleaseCurrentCell(GameObject deadEntity = null)
        {
            var grid = CurrentGrid;

            if (grid == null) return;

            if (deadEntity)
                grid.GridData[_currentGridID.x, _currentGridID.y, _currentGridID.z].entities.Add(deadEntity);

            grid.GridData[_currentGridID.x, _currentGridID.y, _currentGridID.z].entities.Remove(_owner);
            grid.GridData[_currentGridID.x, _currentGridID.y, _currentGridID.z].state = GridState.Passable;
        }

        public bool IsValidGrid() 
        {
            var manager = GridManager;

            return manager != null && manager.Source != null
                && manager.Source.IsGridGenerated && manager.Snapper != null;
        }

        private bool CanMoveToTarget()
        {
            var currentGrid = CurrentGrid;

            if (currentGrid == null) return false;

            Vector3Int targetCoord = currentGrid.WorldToLocalCell(_movingTarget);

            GridState currentLayerState = currentGrid.GridData[targetCoord.x, targetCoord.y, targetCoord.z].state;

            if (currentLayerState == GridState.Passable)
            {
                foreach (var grid in BlockGrids) 
                {
                    GridState blockLayerState = grid.GridData[targetCoord.x, targetCoord.y, targetCoord.z].state;

                    if (blockLayerState == GridState.Blocked)
                        return false;
                }

                return true;
            }

            return false;
        }
    }
}