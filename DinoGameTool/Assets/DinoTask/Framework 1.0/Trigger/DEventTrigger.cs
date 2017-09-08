namespace Dino_Core.Task
{
    public class DEventTrigger : BaseTrigger
    {
        public BaseEvent MonitorEvent;

        protected override void OnMonitoring()
        {
            if (MonitorEvent == null)
            {
                RState = RunningState.END;
            }

            if (MonitorEvent.RState == RunningState.END)
            {
                Conditional();
            }
        }
    }
}