/*
 * @Author: fasthro
 * @Date: 2019-09-18 15:42:48
 * @Description: Action Callback
 */

namespace FastEngine.Core
{
    // Action Callback
    public delegate void ActionCallback();

    // Action Callback Type
    public enum ACTION_CALLBACK_TYPE
    {
        COMPLETED,
        DISPOSED,
        
        #region MoveAction
        MOVE_UPDATE,
        #endregion
    }
}
