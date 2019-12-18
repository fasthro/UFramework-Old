namespace FastEngine.Core
{
    /// <summary>
    /// hotfix事件(便于外部设置UI显示)
    /// </summary>
    public enum HotfixEvent
    {
        InitVersion,
        CheckBase,
        UnpackBase,
        InitUpdate,
        CheckUpdate,
        DownloadUpdate,
        UnpackUpdate,
        HotfixFinished,
    }

    /// <summary>
    /// hotfix回调(便于外部设置UI显示)
    /// </summary>
    public abstract class HotfixEventCallback
    {
        public HotfixEvent hotfixEvent { get; private set; }

        public void Callback(HotfixEvent e)
        {
            hotfixEvent = e;
            OnCallback();
        }
        protected virtual void OnCallback() { }

        public void Progress(float value) { OnProgress(value); }
        protected virtual void OnProgress(float value) { }
    }
}
