using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static class ActorExt
    {
        public static T GetComInActor<T>(this Actor thisActor) where T : Component
        {
            var obj = thisActor.gameObject;
            var actObj = obj.GetComponent<Actor>();
            if (obj == null || (actObj != null && actObj != thisActor)) { return null; }
            T result = GetComInActorInternal<T>(thisActor, obj);
            return result;
        }

        static T GetComInActorInternal<T>(Actor thisActor, GameObject obj) where T : Component
        {
            var actObj = obj.GetComponent<Actor>();
            if (obj == null || (actObj != null && actObj != thisActor)) { return null; }
            T result = obj.GetComponent<T>();
            if (result == null)
            {
                if (obj.transform.childCount > 0)
                {
                    for (int i = 0; i < obj.transform.childCount; i++)
                    {
                        var gh = obj.transform.GetChild(i);
                        result = GetComInActorInternal<T>(thisActor, gh.gameObject);
                        if (result != null)
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }
    }
}