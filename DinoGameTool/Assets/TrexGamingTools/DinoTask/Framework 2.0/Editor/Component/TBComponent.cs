namespace Dino_Core.Task
{
    public class ITBComponent
    {
        protected TBWindow ParentWindow;
        public ITBComponent(TBWindow _window)
        {
            ParentWindow = _window;
        }
        public virtual void PaintComponent()
        {
        }
    }
}