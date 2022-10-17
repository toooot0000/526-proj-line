using System;
using System.Collections.Generic;
using System.Linq;
using Tutorial.Common;
using Unity.VisualScripting;
using UnityEngine;

namespace Tutorial.Utility{

    public class StepConditionToContinue<T>:  StepBase
    where T: TutorialBase {
        protected Action<T, StepConditionToContinue<T>> SetUpProcedure{ get; set; } = null;
        protected Action<T, StepConditionToContinue<T>> ContinueConditionBind{ get; set; } = null;
        protected Action<T, StepConditionToContinue<T>> ContinueConditionUnbind{ get; set;} = null;
        protected Action<T, StepConditionToContinue<T>> CleanUpProcedure{ get; set;} = null;

        public StepConditionToContinue(
            Action<T, StepConditionToContinue<T>> setUpProcedure = null, 
            Action<T, StepConditionToContinue<T>> cleanUpProcedure = null,
            Action<T, StepConditionToContinue<T>> continueConditionBind = null,
            Action<T, StepConditionToContinue<T>> continueConditionUnbind = null
            ){
            SetUpProcedure = setUpProcedure;
            CleanUpProcedure = cleanUpProcedure;
            ContinueConditionBind = continueConditionBind;
            ContinueConditionUnbind = continueConditionUnbind;
        }

        public override void SetUp(TutorialBase tutorial){
            SetUpProcedure?.Invoke((T)tutorial, this);
            ContinueConditionBind?.Invoke((T)tutorial, this);
        }
        
        public override void Complete(TutorialBase tutorial){
            CleanUpProcedure?.Invoke((T)tutorial, this);
            ContinueConditionUnbind?.Invoke((T)tutorial, this);
        }
    }
}