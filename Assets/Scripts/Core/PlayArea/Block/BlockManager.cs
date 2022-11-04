using System;
using System.Collections.Generic;
using Model.Obstacles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.PlayArea.Block{
    public class BlockManager: MonoBehaviour{
        
        public GameObject blockPrefab;
        public float cellSizePaddingDelta = 0.1f;
        public PlayAreaManager playAreaManager;

        private readonly List<BlockView> _blockViews = new();

        public BlockView PlaceBlock(Model.Obstacles.Block block){
            var i = 0;
            for (; i < _blockViews.Count; i++){
                if(_blockViews[i].gameObject.activeSelf) continue;
                break;
            }
            if (i >= _blockViews.Count){
                var newBlock = GenerateBlock();
                _blockViews.Add(newBlock);
            }
            var rect = GridRectToRect(block.RectInt);
            
            _blockViews[i].gameObject.SetActive(true);
            _blockViews[i].Rect = rect;
            _blockViews[i].Model = block;
            return _blockViews[i];
        }

        public void RemoveBlock(BlockView blockView){
            if (blockView == null) return;
            blockView.gameObject.SetActive(false);
            playAreaManager.model.RemovePlayableObject(blockView.Model);
        }
        
        public void RemoveAllBocks(){
            _blockViews.ForEach(b => b.gameObject.SetActive(false));
            foreach (var view in _blockViews){
                playAreaManager.model.RemovePlayableObject(view.Model);
            }
        }

        private BlockView GenerateBlock(){
            return Instantiate(blockPrefab, transform, false).GetComponent<BlockView>();
        }

        private Rect GridRectToRect(RectInt gridRect){
            var ret = playAreaManager.GridRectToRect(gridRect);
            ret.size *= (1- 2*cellSizePaddingDelta);
            return ret;
        }


    }
}