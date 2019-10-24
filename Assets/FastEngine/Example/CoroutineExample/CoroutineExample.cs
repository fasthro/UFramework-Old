/*
 * @Author: fasthro
 * @Date: 2019-10-24 17:56:46
 * @Description: Coroutine Example
 */

using UnityEngine;
using FastEngine.Core;
using System.Collections;

namespace FastEngine.Example
{
    public class CoroutineExample : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // create coroutine and start
            // CoroutineFactory.CreateAndStart(Test());

            // create coroutine and start complete callback
            // CoroutineFactory.CreateAndStart(Test(), (arg) =>
            // {
            //     Debug.Log("complate 2 second");
            // });
        }

        IEnumerator Test()
        {
            Debug.Log("start 2 second");
            yield return new WaitForSeconds(2f);
            Debug.Log("end 2 second");
        }
    }
}