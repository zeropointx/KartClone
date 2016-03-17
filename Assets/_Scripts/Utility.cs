using UnityEngine;
using System.Reflection;
using System.Collections;

namespace kc
{
    public class Utility : MonoBehaviour
    {
        static void CopyComponent(GameObject target, Component original)
        {
            System.Type type = original.GetType();
            Component copy = target.AddComponent(type);
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    pinfo.SetValue(copy, pinfo.GetValue(original, null), null);
                }
            }
        }

        static void MoveComponent(GameObject target, Component original)
        {
            CopyComponent(target, original);
            Destroy(original);
        }
    }
}