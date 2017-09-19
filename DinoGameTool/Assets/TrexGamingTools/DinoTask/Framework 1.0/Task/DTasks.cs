namespace Dino_Core.Task
{
    public abstract class DTasks<T> : MonoSingleton<T> where T : MonoSingleton<T>
    {
        protected BaseTask[] Tasks
        {
            get; set;
        }
        public V GetTask<V>(string _id) where V : BaseTask
        {
            for (int i = 0; i < Tasks.Length; i++)
            {
                if (Tasks[i].ID == _id)
                {
                    return Tasks[i] as V;
                }
            }

            return null;
        }
        public virtual void Init()
        {
            Tasks = GetComponentsInChildren<BaseTask>();

            for (int i = 0; i < Tasks.Length; i++)
            {
                Tasks[i].InitController();
            }
        }
        public void ResetTasks()
        {
            for (int i = 0; i < Tasks.Length; i++)
            {
                Tasks[i].ResetController();
            }
        }

        public void ReadyTasks()
        {
            for (int i = 0; i < Tasks.Length; i++)
            {
                Tasks[i].Ready();
            }
        }
    }
}