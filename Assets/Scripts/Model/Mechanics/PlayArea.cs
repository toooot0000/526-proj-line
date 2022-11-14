using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model.Mechanics.PlayableObjects;
using UnityEngine;

namespace Model.Mechanics{
    public class PlayArea: GameModel{
        public static readonly Vector2Int GridSize = new Vector2Int(8, 5);
        public const int ReservedNumber = 10;
        private readonly List<IPlayableObject> _objects = new();

        public PlayArea(GameModel parent) : base(parent){ }
        
        /// <summary>
        /// Return a non-overlapped RectInt in the grid
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public RectInt? RandomRectInt(Vector2Int size){
            if (RemainingPositionNumber() <= 0) return null;
            var candidates = new List<RectInt>();
            var list = _objects.Select(o => o.InitGridRectInt).ToList();
            // get all candidate position;
            for (var i = 0; i < GridSize.x - size.x + 1; i++){
                for (var j = 0; j < GridSize.y - size.y + 1; j++){
                    var cur = new RectInt(new Vector2Int(i, j), size);
                    if (list.Count > 0 && list.FindIndex(r => r.Overlaps(cur)) != -1){
                        continue;
                    }
                    candidates.Add(cur);
                }
            }
            if (candidates.Count == 0) return null;
            var ind = Random.Range(0, candidates.Count);
            return candidates[ind];
        }

        private bool IsOccupied(RectInt rect){
            return _objects.FindIndex(o => o.InitGridRectInt.Overlaps(rect)) != -1;
        }

        private int RemainingPositionNumber(){
            return GridSize.x * GridSize.y - ReservedNumber - _objects.Aggregate(0, (p, cur) => p + cur.InitGridRectInt.size.x * cur.InitGridRectInt.size.y);
        }

        public bool PlaceObject(IPlayableObject playableObject, RectInt? rectInt = null){
            if (RemainingPositionNumber() <= 0) return false;
            if (rectInt == null){
                if (IsOccupied(playableObject.InitGridRectInt)) return false;
            } else{
                if (IsOccupied(rectInt.Value)) return false;
                playableObject.InitGridRectInt = rectInt.Value;
            }
            ForceToPlaceObject(playableObject);
            return true;
        }

        public bool RemovePlayableObject(IPlayableObject playableObject){
            if (playableObject == null) return false;
            return _objects.Remove(playableObject);
        }

        public IPlayableObject[] GetPlayableObjects() => _objects.ToArray();

        public void RemoveAllPlayableObjects(){
            _objects.Clear();
        }

        private void ForceToPlaceObject(IPlayableObject playableObject){
            _objects.Add(playableObject);
        }

        private Block MakeBlock(BlockLevel level){
            var size = Block.GenerateGridSize(level);
            var rectInt = RandomRectInt(size);
            if (rectInt == null) return null;
            return new Block(this, rectInt.Value, level);
        }

        public Block MakeAndPlaceBlock(BlockLevel level){
            var ret = MakeBlock(level);
            ForceToPlaceObject(ret);
            return ret;
        }

        private Mine MakeMine(float speed = 2, float size = 1, MineEffect effect = null){
            var gridSize = new Vector2Int(1, 1);
            var rectInt = RandomRectInt(gridSize);
            if (rectInt == null) return null;
            return new Mine(this, rectInt.Value){
                speed = speed,
                size = size,
                effect = effect
            };
        }

        public Mine MakeAndPlaceMine(float speed = 2, float size = 1, MineEffect effect = null){
            var ret = MakeMine(speed, size, effect);
            ForceToPlaceObject(ret);
            return ret;
        }
        
        public BlackHole MakeAndPlaceBlackHole(float innerRange, float outerRange){
            var ret = new BlackHole(this, innerRange, outerRange);
            var rectInt = RandomRectInt(new Vector2Int(3, 3));
            if (rectInt == null) return null;
            ret.InitGridRectInt = rectInt.Value;
            ForceToPlaceObject(ret);
            return ret;
        }

        public Crystal MakeAndPlaceCrystal(CrystalType type, object arg){
            var ret = new Crystal(this, type, arg);
            var rectInt = RandomRectInt(new Vector2Int(1, 1));
            if (rectInt == null) return null;
            ret.InitGridRectInt = rectInt.Value;
            ForceToPlaceObject(ret);
            return ret;
        }

        public DirectionChanger MakeAndPlaceDirectionChanger(float angle){
            var ret = new DirectionChanger(this, angle);
            var rectInt = RandomRectInt(new Vector2Int(1, 1));
            if (rectInt == null) return null;
            ret.InitGridRectInt = rectInt.Value;
            ForceToPlaceObject(ret);
            return ret;
        }

        public Splitter MakeAndPlaceDirectionSplitter(float sizeLimit){
            var ret = new Splitter(this){
                sizeLimit = sizeLimit
            };
            var rectInt = RandomRectInt(new Vector2Int(1, 1));
            if (rectInt == null) return null;
            ret.InitGridRectInt = rectInt.Value;
            ForceToPlaceObject(ret);
            return ret;
        }

        public IEnumerable<Vector2Int> GetOccupiedGridPositions(){
            foreach (var obj in _objects){
                for (var i = 0; i < obj.InitGridRectInt.width; i++){
                    for (var j = 0; j < obj.InitGridRectInt.height; j++){
                        yield return obj.InitGridRectInt.position + new Vector2Int(i, j);
                    }
                }
            }
        }

        public IEnumerable<Vector2Int> GetEmptyGridPositions(){
            var occupied = GetOccupiedGridPositions().ToHashSet();
            for (var i = 0; i < GridSize.x; i++){
                for (var j = 0; j < GridSize.y; j++){
                    if(occupied.Contains(new Vector2Int(i, j))) continue;
                    yield return new Vector2Int(i, j);
                }
            }
        }
    }
}