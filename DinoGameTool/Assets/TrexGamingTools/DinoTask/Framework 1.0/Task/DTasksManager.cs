using Dino_Core.Task;
using System.Collections.Generic;

public class DTasksManager : DTasks<DTasksManager> {
    public List<TaskSet> CheckPoints;

    public TaskSet GetTasksByCheckPoints(int _id)
    {
        if (CheckPoints.Count > _id && _id >= 0)
        {
            return CheckPoints[_id];
        }

        return null;
    }
    public void ResetTasksByCheckPoints(int _id)
    {
        if (CheckPoints.Count > _id && _id >= 0)
        {
            CheckPoints[_id].ResetTasks();
        }
    }
    public void ReadyTasksByCheckPoints(int _id)
    {
        if (CheckPoints.Count > _id && _id >= 0)
        {
            CheckPoints[_id].ReadyTasks();
        }
    }
}

[System.Serializable]
public class TaskSet
{
    public List<BaseTask> Tasks;
    public void ResetTasks()
    {
        for (int i = 0; i < Tasks.Count; i++)
        {
            Tasks[i].ResetController();
        }
    }
    public void ReadyTasks()
    {
        for (int i = 0; i < Tasks.Count; i++)
        {
            Tasks[i].Ready();
        }
    }
}


