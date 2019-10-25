/*
 * @Author: fasthro
 * @Date: 2019-10-25 10:38:45
 * @Description: FastEngine 启动脚本
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FastEngine
{
    public class Launch : MonoBehaviour
    {
        void Awake()
        {
            App.Instance.AppRun();
        }
    }
}
