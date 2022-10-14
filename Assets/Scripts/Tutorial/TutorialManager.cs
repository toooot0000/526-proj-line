using System;
using System.Collections.Generic;
using Core.DisplayArea;
using Core.DisplayArea.Stage;
using Core.PlayArea.Balls;
using Core.PlayArea.TouchTracking;
using Tutorial.Tutorials.EnemyIntention;
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
        private const bool IsActive = true;

        public BallManager ballManager;
        public Shade shade;
        public TouchTracker tracker;
        public StageManager stageManager;
        public ActionDetailDisplayer actionDetailDisplayer;
        public GearDisplayer gearDisplayer;
        public TurnSignDisplayer turnSignDisplayer;
        public TutorialUIController uiController;

        private readonly HashSet<string> _completeTutorials = new();
        private float _nextDelay;
        private string _nextName;
        private string _curName;

        private void Update(){
            if (Input.GetKeyUp(KeyCode.A)){
                ForceLoadTutorial(TutorialEnemyIntention.PrefabName);
            }
        }

        public TutorialBase LoadTutorial(string tutorialName){
            if (!IsActive) return null;
            return ForceLoadTutorial(tutorialName);
        }

        private TutorialBase ForceLoadTutorial(string tutorialName){
            if (_completeTutorials.Contains(tutorialName)) return null;
            if (_curName != null){
                Debug.Log("Multiple Tutorial Invoke!");
                return null;
            }
            _curName = tutorialName;
            UIManager.shared.HideAllComponents();
            var prefab = Resources.Load<GameObject>($"{PrefabPathPrefix}{tutorialName}");
            var inst = Instantiate(prefab, transform);
            var tutorial = inst.GetComponent<TutorialBase>();

            if (tutorial is UITutorialBase){
                tutorial.transform.SetParent(uiController.transform);
            }
            
            tutorial.OnLoaded(this);
            tutorial.OnComplete += CompleteTutorial;
            shade.SetActive(true);
            return tutorial;
        }

        public void CompleteTutorial(TutorialBase tutorial){
            shade.SetActive(false);
            _completeTutorials.Add(_curName);
            _curName = null;
            if (!string.IsNullOrEmpty(tutorial.nextTutorialName)){
                _nextName = tutorial.nextTutorialName;
                _nextDelay = Math.Max(0.1f, tutorial.nextDelay);
                StartCoroutine(CoroutineUtility.Delayed(_nextDelay, () => { LoadTutorial(_nextName); }));
            }
            UIManager.shared.ShowAllComponents();
        }
    }
}