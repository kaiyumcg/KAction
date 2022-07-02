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


        #region Shake
        
        //swirl, rubber jump, spring arm etc effect? socket like attachement as we see in ue?
        //todo rigidbody counterpart?
        public static void Shake(this Actor actor, float shakeDuration, float decreasePoint, bool is2D = false)
        {
            if (actor.shaking)
            {
                return;
            }
            actor.shaking = true;
            actor.StartCoroutine(shakeGameObjectCOR(actor._Transform, shakeDuration, decreasePoint, is2D));

            IEnumerator shakeGameObjectCOR(Transform objTransform, float totalShakeDuration, float decreasePoint, bool objectIs2D = false)
            {
                if (decreasePoint >= totalShakeDuration)
                {
                    yield break; //Exit!
                }

                //Get Original Pos and rot
                Vector3 defaultPos = objTransform.position;
                Quaternion defaultRot = objTransform.rotation;

                float counter = 0f;

                //Shake Speed
                const float speed = 0.1f;

                //Angle Rotation(Optional)
                const float angleRot = 4;

                //Do the actual shaking
                while (counter < totalShakeDuration)
                {
                    counter += Time.deltaTime;
                    float decreaseSpeed = speed;
                    float decreaseAngle = angleRot;

                    //Shake GameObject
                    if (objectIs2D)
                    {
                        //Don't Translate the Z Axis if 2D Object
                        Vector3 tempPos = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                        tempPos.z = defaultPos.z;
                        objTransform.position = tempPos;

                        //Only Rotate the Z axis if 2D
                        objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-angleRot, angleRot), new Vector3(0f, 0f, 1f));
                    }
                    else
                    {
                        objTransform.position = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                        objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-angleRot, angleRot), new Vector3(1f, 1f, 1f));
                    }
                    yield return null;


                    //Check if we have reached the decreasePoint then start decreasing  decreaseSpeed value
                    if (counter >= decreasePoint)
                    {
                        //Reset counter to 0 
                        counter = 0f;
                        while (counter <= decreasePoint)
                        {
                            counter += Time.deltaTime;
                            decreaseSpeed = Mathf.Lerp(speed, 0, counter / decreasePoint);
                            decreaseAngle = Mathf.Lerp(angleRot, 0, counter / decreasePoint);
                            //Shake GameObject
                            if (objectIs2D)
                            {
                                //Don't Translate the Z Axis if 2D Object
                                Vector3 tempPos = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                                tempPos.z = defaultPos.z;
                                objTransform.position = tempPos;

                                //Only Rotate the Z axis if 2D
                                objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-decreaseAngle, decreaseAngle), new Vector3(0f, 0f, 1f));
                            }
                            else
                            {
                                objTransform.position = defaultPos + UnityEngine.Random.insideUnitSphere * decreaseSpeed;
                                objTransform.rotation = defaultRot * Quaternion.AngleAxis(UnityEngine.Random.Range(-decreaseAngle, decreaseAngle), new Vector3(1f, 1f, 1f));
                            }
                            yield return null;
                        }

                        //Break from the outer loop
                        break;
                    }
                }
                objTransform.position = defaultPos; //Reset to original postion
                objTransform.rotation = defaultRot;//Reset to original rotation

                actor.shaking = false; //So that we can call this function next time
            }
        }
        #endregion
    }
}