using System.Collections;
using System.Collections.Generic;
using FastEngine.Core;
using UnityEngine;

public class ActionTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var a1 = new DelayAction(2f);
        var a2 = new DelayAction(5f);
        var a3 = new DelayAction(8f);

        a1.BindCallback(ACTION_CALLBACK_TYPE.COMPLETED, () =>
        {
            Debug.Log("a1 completed " + Time.realtimeSinceStartup);
        });
        a2.BindCallback(ACTION_CALLBACK_TYPE.COMPLETED, () =>
        {
            Debug.Log("a2 completed " + Time.realtimeSinceStartup);
        });
        a3.BindCallback(ACTION_CALLBACK_TYPE.COMPLETED, () =>
        {
            Debug.Log("a3 completed " + Time.realtimeSinceStartup);
        });

        var sq = new SequenceAction(a1, a2, a3);
        sq.BindCallback(ACTION_CALLBACK_TYPE.COMPLETED, () =>
        {
            Debug.Log("SequenceAction completed " + Time.realtimeSinceStartup);
        });
        sq.Start();
    }

    void Update()
    {

    }
}
