using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.PlayArea.Block{
    public enum BlockLevel: int{
        Small = 2,
        Medium = 3,
        Large = 4,
        XLarge = 5
    }
    public class BlockManager: MonoBehaviour{
        
        public GameObject blockPrefab;
        public Vector2Int gridSize = new Vector2Int(6, 6);
        public float cellSizePaddingDelta = 0.2f;
        
        private readonly List<RectInt> _occupied = new();
        private readonly List<Block> _blocks = new();

        public Block PlaceBlock(BlockLevel level){
            var rectInt = GenerateGridRect(level, GenerateGridSize(level));
            if (rectInt == null) return null;
            var i = 0;
            for (; i < _blocks.Count; i++){
                if(_blocks[i].gameObject.activeSelf) continue;
                break;
            }
            if (i >= _blocks.Count){
                var newBlock = GenerateBlock();
                _blocks.Add(newBlock);
            }
            _occupied.Add(rectInt.Value);
            var rect = GridRectToRect(rectInt.Value);
            
            _blocks[i].gameObject.SetActive(true);
            _blocks[i].rectInt = rectInt.Value;
            _blocks[i].Rect = rect;
            
            return _blocks[i];
        }

        public void RemoveBlock(Block block){
            block.gameObject.SetActive(false);
            _occupied.Remove(block.rectInt);
        }

        public void RemoveAllBocks(){
            _blocks.ForEach(b => b.gameObject.SetActive(false));
            _occupied.Clear();
        }

        private Block GenerateBlock(){
            return Instantiate(blockPrefab, transform, false).GetComponent<Block>();
        }

        private Vector2Int GenerateGridSize(BlockLevel level){
            var width = Random.Range(1, (int)level);
            return new Vector2Int(width, (int)level - width);
        }

        private RectInt? GenerateGridRect(BlockLevel level, Vector2Int size){
            var candidates = new List<RectInt>();
            // get all candidate position;
            for (var i = 0; i < gridSize.x - size.x + 1; i++){
                for (var j = 0; j < gridSize.y - size.y + 1; j++){
                    var cur = new RectInt(new Vector2Int(i, j), size);
                    if (_occupied.Count > 0 && _occupied.FindIndex(r => r.Overlaps(cur)) != -1){
                        continue;
                    }
                    candidates.Add(cur);
                }
            }
            if (candidates.Count == 0) return null;
            var ind = Random.Range(0, candidates.Count);
            return candidates[ind];
        }

        private Rect GridRectToRect(RectInt gridRect){
            var gridCellSize = ((RectTransform)transform).rect.size / gridSize;
            var ret = new Rect(){
                position = gridRect.position * gridCellSize,
                size = gridRect.size * gridCellSize
            };
            ret.position += ret.size / 2;
            ret.size *= (1- 2*cellSizePaddingDelta);
            return ret;
        }

        // private Block _test;
        // private void Update(){
        //     var values = Enum.GetValues(typeof(BlockLevel));
        //     if (Input.GetKeyUp(KeyCode.A)){
        //         _test = PlaceBlock((BlockLevel)values.GetValue(Random.Range(0, values.Length)));
        //     }
        //
        //     if (Input.GetKeyUp(KeyCode.S)){
        //         RemoveBlock(_test);
        //     }
        // }
    }
}