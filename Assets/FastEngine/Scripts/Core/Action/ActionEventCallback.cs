/*
 * @Author: fasthro
 * @Date: 2019-09-18 15:42:48
 * @Description: Action Callback
 */

namespace FastEngine.Core
{
    // Action Callback
    public delegate void ActionEventCallback();
    public delegate void ActionEventCallback<T>(T arg);

    // Action Callback Type
    public enum ActionEvent
    {
        Completed,
        Disposed,
        Reset,
        Update,

        #region repeat action
        RepeatStepCompleted,
        #endregion

        #region http action
        HttpSucceed,
        HttpFailled,
        HttpProgress,
        #endregion
    }
}