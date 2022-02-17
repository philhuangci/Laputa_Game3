using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{

    public class GOHelper : MonoBehaviour
    {

        public static Transform FindChildNode(GameObject goParent, string childName)
        {
            Transform resultTrans = null;
            resultTrans = goParent.transform.Find(childName);
            if (resultTrans == null)
            {
                foreach (Transform trnas in goParent.transform)
                {
                    resultTrans = FindChildNode(trnas.gameObject, childName);
                    if (resultTrans != null) return resultTrans;
                }
            }
            return resultTrans;

        }

        public static T GetChildNodeComponentScript<T>(GameObject goParent, string childName) where T:Component
        {
            Transform resultTrans = null;
            resultTrans = FindChildNode(goParent, childName);
            if (resultTrans != null)
            {
                return resultTrans.gameObject.GetComponent<T>();
            }
            else
            {
                return null;
            }
        }

        public static T AddChildNodeComponent<T>(GameObject goParent, string childName) where T:Component
        {
            Transform resultTrans = null;
            resultTrans = FindChildNode(goParent, childName);
            if (resultTrans != null)
            {
                T[] componentScriptsArray = resultTrans.GetComponents<T>();
                foreach (T item in componentScriptsArray)
                {
                    if (item != null) Destroy(item);
                }
                return resultTrans.gameObject.AddComponent<T>();
            }
            else
            {
                return null;
            }
        }

        public static void AddChildNodeToParentNode(Transform parent,Transform child)
        {
            child.SetParent(parent, false);
            child.localPosition = Vector3.zero;
            child.localScale = Vector3.one;
            child.localEulerAngles = Vector3.zero;
        }
    }
}
