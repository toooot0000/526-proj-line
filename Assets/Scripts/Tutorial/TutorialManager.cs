using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.DisplayArea;
using Core.DisplayArea.Stage;
using Core.PlayArea.Balls;
using Core.PlayArea.TouchTracking;
using Tutorial.Tutorials;
using Tutorial.Tutorials.BasicConcept;
using Tutorial.Tutorials.EnemyIntention;
using Tutorial.UI;
using Tutorials;
using UI;
using UI.Common.Shade;
using UI.GearDisplayer;
using UI.TurnSignDisplayer;
using UnityEngine;
using Utility;

namespace Tutorial{
    public class TutorialManager : MonoBehaviour{
        private const string PrefabPathPrefix = "Prefabs/Tutorials/";
        private const bool IsActive = false;

        public BallManager ballManager;
        public Shade shade;
        public TouchTracker tracker;
        public StageManager stageManager;
        public ActionDetailDisplayer actionDetailDisplayer;
        public GearDisplayer gearDisplayer;
        public TurnSignDisplayer turnSignDisplayer;
        public TutorialUIController uiController;

        private readonly HashSet<string> _completeTutorials = new();
        private readonly HashSet<Type> _completeTutorialTypes = new();
        private float _nextDelay;
        private string _nextName;
        private string _curName;
        private Type _curTutorialType;

        private void Update(){
            if (Input.GetKeyUp(KeyCode.A)){
                ForceLoadTutorial<UITutorialTest>();
            }
        }
        
        [Obsolete("Use the generic one")]
        public TutorialBase LoadTutorial(string tutorialName){
            if (!IsActive) return null;
            if (_completeTutorials.Contains(tutorialName)) return null;
            if (_curName != null){
                Debug.Log("Replicated Tutorial Invoke!");
                return null;
            }
            return ForceLoadTutorial(tutorialName);
        }

        [Obsolete("Use the generic one")]
        private TutorialBase ForceLoadTutorial(string tutorialName){
            _curName = tutorialName;
            var tutorial = InstanceTutorial(tutorialName);
            UIManager.shared.HideAllComponents();
            tutorial.OnLoaded(this);
            tutorial.OnComplete += CompleteTutorial;
            shade.SetActive(true);
            return tutorial;
        }

        public void CompleteTutorial(TutorialBase tutorial){
            // TODO: clean up;
            shade.SetActive(false);
            _completeTutorials.Add(_curName);
            _curName = null;
            uiController.shade.SetActive(false);
            _completeTutorialTypes.Add(_curTutorialType);
            _curTutorialType = null;
            if (!string.IsNullOrEmpty(tutorial.nextTutorialName)){
                _nextName = tutorial.nextTutorialName;
                _nextDelay = Math.Max(0.1f, tutorial.nextDelay);
                StartCoroutine(CoroutineUtility.Delayed(_nextDelay, () => { LoadTutorial(_nextName); }));
            }
            UIManager.shared.ShowAllComponents();
        }

        private TutorialBase InstanceTutorial(string tutorialName){
            var prefab = Resources.Load<GameObject>($"{PrefabPathPrefix}{tutorialName}");
            var inst = Instantiate(prefab, transform);
            return inst.GetComponent<TutorialBase>();
        }

        private string GetPrefabName<T>() where T : TutorialBase{
            var member = typeof(T).GetField("PrefabName", BindingFlags.Static | BindingFlags.Public);
            if (member == null){
                Debug.LogError($"Type {typeof(T).ToString()} has no PrefabName!");
                return null;
            }
            return (string)member.GetValue(null);
        }

        public T LoadTutorial<T>() where T : TutorialBase{
            if (!IsActive) return null;
            if (_completeTutorialTypes.Contains(typeof(T))) return null;
            if (_curTutorialType != null){
                Debug.Log("Replicated Tutorial Invoke!");
                return null;
            }
            return ForceLoadTutorial<T>();
        }

        public T ForceLoadTutorial<T>() where T : TutorialBase{
            _curTutorialType = typeof(T);
            var tutorial = InstanceTutorial<T>();
            tutorial.OnLoaded(this);
            tutorial.OnComplete += CompleteTutorial;
            if (typeof(T).IsSubclassOf(typeof(UITutorialBase))){
                uiController.shade.SetActive(true);
            } else{
                UIManager.shared.HideAllComponents();
                shade.SetActive(true);
            }
            return tutorial;
        }

        private T InstanceTutorial<T>() where T : TutorialBase{
            var prefab = Resources.Load<GameObject>($"{PrefabPathPrefix}{GetPrefabName<T>()}");
            var inst = Instantiate(prefab, typeof(T).IsSubclassOf(typeof(UITutorialBase)) ? uiController.transform : transform);
            return inst.GetComponent<T>();
        }
        
        
    }
}