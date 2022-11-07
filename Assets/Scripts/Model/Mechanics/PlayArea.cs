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
        private RectInt? RandomRectInt(Vector2Int size){
            if (RemainingPositionNumber() <= 0) return null;
            var candidates = new List<RectInt>();
            var list = _objects.Select(o => o.InitGridPosition).ToList();
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
            return _objects.FindIndex(o => o.InitGridPosition.Overlaps(rect)) != -1;
        }

        private int RemainingPositionNumber(){
            return GridSize.x * GridSize.y - ReservedNumber - _objects.Aggregate(0, (p, cur) => p + cur.InitGridPosition.size.x * cur.InitGridPosition.size.y);
        }

        private bool PlaceObject(IPlayableObject playableObject, RectInt? rectInt = null){
            if (RemainingPositionNumber() <= 0) return false;
            if (rectInt == null){
                if (IsOccupied(playableObject.InitGridPosition)) return false;
            } else{
                if (IsOccupied(rectInt.Value)) return false;
                playableObject.InitGridPosition = rectInt.Value;
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

        private Mine MakeMine(MineEffect effect = null){
            var size = new Vector2Int(1, 1);
            var rectInt = RandomRectInt(size);
            if (rectInt == null) return null;
            return new Mine(this, rectInt.Value){
                effect = effect
            };
        }

        public Mine MakeAndPlaceMine(MineEffect effect = null){
            var ret = MakeMine(effect);
            ForceToPlaceObject(ret);
            return ret;
        }
    }
}