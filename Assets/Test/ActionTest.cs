using System.Collections;
using System.Collections.Generic;
using CI.HttpClient;
using DG.Tweening;
using FastEngine.Core;
using UnityEngine;

public class ActionTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // var a1 = new DelayAction(2f);
        // var a2 = new DelayAction(5f);
        // var a3 = new DelayAction(8f);

        // a1.BindCallback(ACTION_CALLBACK_TYPE.COMPLETED, () =>
        // {
        //     Debug.Log("a1 completed " + Time.realtimeSinceStartup);
        // });
        // a2.BindCallback(ACTION_CALLBACK_TYPE.COMPLETED, () =>
        // {
        //     Debug.Log("a2 completed " + Time.realtimeSinceStartup);
        // });
        // a3.BindCallback(ACTION_CALLBACK_TYPE.COMPLETED, () =>
        // {
        //     Debug.Log("a3 completed " + Time.realtimeSinceStartup);
        // });

        // var sq = new SequenceAction(a1, a2, a3);
        // sq.BindCallback(ACTION_CALLBACK_TYPE.COMPLETED, () =>
        // {
        //     Debug.Log("SequenceAction completed " + Time.realtimeSinceStartup);
        // });
        // sq.Start();

        // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // cube.transform.position = new Vector3(0, 0, 0);
        // cube.transform.DOMove(new Vector3(10, 0, 0), 2).SetAutoKill(false);
        // cube.transform.D

        // var DGA1 = new DGMoveAction(cube.transform, cube.transform.position, new Vector3(10, 0, 0), 2);
        // var DGA2 = new DGRotateAction(cube.transform, cube.transform.eulerAngles, new Vector3(0, 180, 0), 2);
        // var DGA3 = new DGScaleAction(cube.transform, cube.transform.localScale, new Vector3(3, 3, 3), 2);
        // new SequenceAction(DGA1, DGA2, DGA3).Start();

        // var DGA4 = new DGJumpAction(cube.transform, cube.transform.position, new Vector3(10, 0, 0), 10, 1, 1).SetRestoreValue(true);
        // new RepeatAction(DGA4, -1).Start();

        var http1 = new GetAction("http://192.168.1.253/platform/devConfig.json");
        http1.BindCallback<HttpResponseMessage>(ACTION_CALLBACK_TYPE.HTTP_SUCCEED, (rep) =>
        {
            Debug.Log("HTTP Get Succeed! ");
        });
        http1.BindCallback<HttpResponseMessage>(ACTION_CALLBACK_TYPE.HTTP_FAILLED, (rep) =>
        {
            Debug.Log("HTTP Get Failled! " + rep.StatusCode);
        });
        http1.SetTimeout(30);
        
        new RepeatAction(http1, -1).Start();
    }

    void Update()
    {

    }
}
