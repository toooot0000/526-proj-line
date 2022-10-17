using System;
using System.Collections.Generic;
using System.Linq;
using Tutorial.Common;
using Unity.VisualScripting;
using UnityEngine;

namespace Tutorial.Utility{

    public class StepConditionToContinue:  StepBase{
        protected StepCallbackDelegate SetUpProcedure{ get; set; } = null;
        protected StepCallbackDelegate ContinueConditionBind{ get; set; } = null;
        protected StepCallbackDelegate ContinueConditionUnbind{ get; set;} = null;
        protected StepCallbackDelegate CleanUpProcedure{ get; set;} = null;

        public StepConditionToContinue(
            StepCallbackDelegate setUpProcedure = null, 
            StepCallbackDelegate cleanUpProcedure = null,
            StepCallbackDelegate continueConditionBind = null,
            StepCallbackDelegate continueConditionUnbind = null
            ){
            SetUpProcedure = setUpProcedure;
            CleanUpProcedure = cleanUpProcedure;
            ContinueConditionBind = continueConditionBind;
            ContinueConditionUnbind = continueConditionUnbind;
        }

        public override void SetUp(TutorialBase tutorial){
            SetUpProcedure?.Invoke(tutorial, this);
            ContinueConditionBind?.Invoke(tutorial, this);
        }
        
        public override void Complete(TutorialBase tutorial){
            CleanUpProcedure?.Invoke(tutorial, this);
            ContinueConditionUnbind?.Invoke(tutorial, this);
        }
    }
}