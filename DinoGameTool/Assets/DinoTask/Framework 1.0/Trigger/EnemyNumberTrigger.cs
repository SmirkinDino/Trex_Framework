using Dino_Core.Task;

public class EnemyNumberTrigger : BaseTrigger {

    public TaskConst.Enemy_Type MonitorType;
    public int AtLeast;

    protected bool _canMonitor = false;

    protected override void OnMonitoring()
    {
        if (Owner.GetSets(MonitorType) == null)
        {
            return;
        }

        if (!_canMonitor && Owner.GetSets(MonitorType).Count > AtLeast)
        {
            _canMonitor = true;
        }

        if (_canMonitor && Owner.GetSets(MonitorType).Count <= AtLeast)
        {
            Conditional();
        }
    }
    protected override void TReady()
    {
        _canMonitor = false;
    }
    protected override void TEnd()
    {
        _canMonitor = false;
    }
    protected override void TReset()
    {
        _canMonitor = false;
    }

}
