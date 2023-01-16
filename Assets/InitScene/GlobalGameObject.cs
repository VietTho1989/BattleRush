using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GlobalGameObject : MonoBehaviour
{

    // public AssetReference appLoginAssetReference;

    void Start()
    {
        /*if (appLoginAssetReference != null)
        {
            // Addressables.InstantiateAsync(appLoginAssetReference, this.transform);
            Addressables.Instantiate(appLoginAssetReference, this.transform);
        }
        else
        {
            Logger.LogError("appLoginAssetReference null");
        }*/
        // StartCoroutine(InitAppLoginUI());
    }

    /*IEnumerator InitAppLoginUI()
    {
        AsyncOperationHandle<GameObject> loadOp = Addressables.LoadAssetAsync<GameObject>(appLoginAssetReference);
        yield return loadOp;
        if (loadOp.Status == AsyncOperationStatus.Succeeded)
        {
            var op = Addressables.InstantiateAsync(appLoginAssetReference);
            if (op.IsDone) // <--- this will always be true.  A preloaded asset will instantiate synchronously. 
            {
                //...
            }
            //...
        }
    }*/

}