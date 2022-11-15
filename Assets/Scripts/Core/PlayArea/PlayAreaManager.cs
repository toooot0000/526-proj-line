using System.Collections.Generic;
using System.Linq;
using Core.PlayArea.Balls;
using Core.PlayArea.BlackHoles;
using Core.PlayArea.Blocks;
using Core.PlayArea.Crystals;
using Core.PlayArea.DirectionChangers;
using Core.PlayArea.Mines;
using Model;
using Model.Mechanics;
using Model.Mechanics.PlayableObjects;
using Model.Mechanics.PlayableObjects.MineEffects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.PlayArea{
    public class PlayAreaManager: MonoBehaviour{
        public Model.Mechanics.PlayArea model;
        public BallManager ballManager;
        public BlockManager blockManager;
        public MineManager mineManager;
        public BlackHoleManager blackHoleManager;
        public Vector2 padding = new Vector2(.3f, .3f);

        private readonly List<PlayableObjectViewBase> _views = new();
        private readonly List<IPlayableViewManager> _managers = new();

        private readonly List<IResetable> _resetables = new();
        
        public IEnumerable<IPlayableViewManager> AllManagers => _managers;

        public T GetManager<T>() where T : IPlayableViewManager => (T)AllManagers.First(m => m is T);

        public void RegisterManager(IPlayableViewManager manager){
            _managers.Add(manager);
        }

        private void Start(){
            model = GameManager.shared.game.playArea;
            model.view = this;
        }
        
        public Rect GridRectToRect(RectInt gridRect){
            var gridCellSize = (((RectTransform)transform).rect.size - padding * 2) / Model.Mechanics.PlayArea.GridSize;
            var ret = new Rect(){
                position = gridRect.position * gridCellSize + padding,
                size = gridRect.size * gridCellSize
            };
            ret.position += ret.size / 2;
            return ret;
        }

        public void SetPlayableViewPosition<T>(T view, IPlayableObject objectModel) 
        where T: PlayableObjectViewBase{
            var rect = GridRectToRect(objectModel.InitGridRectInt);
            var rectTrans = view.transform;
            ((RectTransform)rectTrans).anchoredPosition = rect.position;
            ((RectTransform)rectTrans).sizeDelta = rect.size;
        }

        public void SetPlayableViewPosition<T>(PlayableObjectViewWithModel<T> view)
        where T:GameModel, IPlayableObject {
            SetPlayableViewPosition(view, view.Model);
        }


        public BlockView AddBlock(BlockLevel level){
            var block = model.MakeAndPlaceBlock(level);
            return blockManager.PlaceBlock(block);
        }

        private BlockView _test;
        private void Update(){
            if (Input.GetKeyUp(KeyCode.A)){
                var mine = model.MakeAndPlaceMine(2, 1, new MineEffectLoseLife(1));
                mineManager.Place(mine);
            }

            if (Input.GetKeyUp(KeyCode.S)){
                var blackHole = model.MakeAndPlaceBlackHole(0.2f, 3f);
                if(blackHole != null) blackHoleManager.Place(blackHole);
            }

            if (Input.GetKeyUp(KeyCode.D)){
                var crystal = new Crystal(GameManager.shared.game, CrystalType.FreezeMovable, 0.1f);
                var rect = model.RandomRectInt(Vector2Int.one)!;
                crystal.InitGridRectInt = rect.Value;
                model.PlaceObject(crystal);
                GetManager<CrystalManager>().Place(crystal);
            }
            
            if (Input.GetKeyUp(KeyCode.F)){
                var crystal = new Crystal(GameManager.shared.game, CrystalType.LengthenLine, 10f);
                var rect = model.RandomRectInt(Vector2Int.one)!;
                crystal.InitGridRectInt = rect.Value;
                model.PlaceObject(crystal);
                GetManager<CrystalManager>().Place(crystal);
            }

            if (Input.GetKeyUp(KeyCode.G)){
                var dc = new DirectionChanger(model, Random.Range(0, 361)){
                    InitGridRectInt = model.RandomRectInt(Vector2Int.one)!.Value
                };
                model.PlaceObject(dc);
                GetManager<DirectionChangerManager>().Place(dc);
            }
        }

        public void RegisterResetEffect(IResetable resetable){
            _resetables.Add(resetable);
        }

        public IEnumerable<T> GetAllViewsOfProperty<T>() where T: IPlayableObjectViewProperty {
            foreach (var playableViewManager in AllManagers) {
                foreach (var playableObjectViewBase in playableViewManager.GetAllViews()) {
                    if (playableObjectViewBase is T typed) yield return typed;
                }
            }
        }

        public IEnumerable<T> GetAllViewsOfType<T>() where T : PlayableObjectViewBase{
            foreach (var playableObjectViewBase in GetAllViews()){
                if (playableObjectViewBase is T typed) yield return typed;
            }
        }

        public IEnumerable<PlayableObjectViewBase> GetAllViews(){
            return AllManagers.SelectMany(playableViewManager => playableViewManager.GetAllViews());
        }

        public void OnPlayerFinishDrawing(){
            foreach (var view in GetAllViewsOfProperty<IOnPlayerFinishDrawing>()){
                view.OnPlayerFinishDrawing();
            }
            foreach (var resetable in _resetables){
                resetable.Reset();
            }
            _resetables.Clear();
        }

        public void ClearAllObjects(){
            foreach (var playableViewManager in AllManagers){
                foreach (var playableObjectViewBase in playableViewManager.GetAllViews()){
                    playableObjectViewBase.gameObject.SetActive(false);
                }
            }
            model.ClearAllObjects();
        }
    }
}