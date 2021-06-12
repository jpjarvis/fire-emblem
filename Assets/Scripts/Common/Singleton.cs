using UnityEngine;

namespace FireEmblem.Common
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<T>();
                }

                return _instance;
            }
        }
    }
}