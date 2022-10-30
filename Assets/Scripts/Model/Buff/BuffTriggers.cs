namespace Model.Buff{
    public interface IBuffTrigger{ }
    
    public interface IBuffEffect<in T> where T: IBuffModifiable{
        void Execute(T buffable);
    }

    public interface IBuffTriggerOnTurnBegin<in T> : IBuffTrigger
    where T: IBuffModifiable{
        IBuffEffect<T> OnTurnBegin();
    }

    public interface IBuffTriggerOnTurnEnd<in T> : IBuffTrigger
    where T: IBuffModifiable{
        IBuffEffect<T> OnTurnEnd();
    }

    public interface IBuffTriggerOnBallSliced<in T, in TBall>: IBuffTrigger 
        where TBall: Ball
        where T: IBuffModifiable{
        IBuffEffect<T> OnBallSliced(TBall ball);
    }

    public interface IBuffTriggerOnBallCircled<in T, in TBall>: IBuffTrigger 
        where TBall: Ball
        where T: IBuffModifiable{
        IBuffEffect<T> OnBallCircled(TBall ball);
    }

    public interface IBuffTriggerOnInputStart<in T> : IBuffTrigger
    where T: IBuffModifiable{
        IBuffEffect<T> OnInputStart();
    }

    public interface IBuffTriggerOnGetPlayerActionInfo : IBuffTrigger{
        IBuffEffect<StageActionInfoPlayerAction> OnGetPlayerActionInfo();
    }

    public interface IBuffTriggerOnGetEnemyActionInfo : IBuffTrigger{
        IBuffEffect<StageActionInfoEnemyAction> OnGetEnemyActionInfo();
    }

    public interface IBuffTriggerOnTakeDamage : IBuffTrigger{
        IBuffEffect<IBuffModifiable> OnTakeDamage();
    }
}