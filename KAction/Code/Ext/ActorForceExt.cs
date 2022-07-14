using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public static class ActorForceExt
    {
        public static void Jump(this Actor actor, Vector3 direction, float height, OnCompleteForceLibFunc OnComplete = null, bool acceleration = true)
        {
            throw new System.NotImplementedException();
        }

        public static void Helix(this Actor actor, Vector3 direction, float height, float within, OnCompleteForceLibFunc OnComplete = null, bool revertBack = true)
        {
            throw new System.NotImplementedException();
        }

        public static void TurnTo(this Actor actor, Actor to, float within, OnCompleteForceLibFunc OnComplete = null, ActorForceEase ease = ActorForceEase.Linear)
        {
            throw new System.NotImplementedException();
        }

        public static void MoveTo(this Actor actor, Actor to, float within, OnCompleteForceLibFunc OnComplete = null, ActorForceEase ease = ActorForceEase.Linear)
        {
            throw new System.NotImplementedException();
        }

        public static void TurnTo(this Actor actor, Vector3 to, float within, OnCompleteForceLibFunc OnComplete = null, ActorForceEase ease = ActorForceEase.Linear)
        {
            throw new System.NotImplementedException();
        }

        public static void MoveTo(this Actor actor, Vector3 to, float within, OnCompleteForceLibFunc OnComplete = null, ActorForceEase ease = ActorForceEase.Linear)
        {
            throw new System.NotImplementedException();
        }

        public static void Thurst(this Actor actor, Vector3 direction, float initialBackwardTime, float restTime, OnCompleteForceLibFunc OnComplete = null)
        {
            throw new System.NotImplementedException();
        }

        public static void Swirl(this Actor actor, Vector3 singularityPosition, float centerTime, float upwardFactor, OnCompleteForceLibFunc OnComplete = null)
        {
            throw new System.NotImplementedException();
        }

        public static void Pop(this Actor actor, float popTime, float rubberness, OnCompleteForceLibFunc OnComplete = null)
        {
            throw new System.NotImplementedException();
        }

        public static void SpringRelease(this Actor actor, Vector3 direction, float force, OnCompleteForceLibFunc OnComplete = null)
        {
            throw new System.NotImplementedException();
        }

        public static void Summon(this Actor actor, Vector3 direction, Vector3 position, float force, OnCompleteForceLibFunc OnComplete = null)
        {
            throw new System.NotImplementedException();
        }

        public static void Summon(this Actor actor, Vector3 position, float force, OnCompleteForceLibFunc OnComplete = null)
        {
            throw new System.NotImplementedException();
        }

        public static void Shake(this Actor actor, float shakeDuration, float decreasePoint, OnCompleteForceLibFunc OnComplete = null, bool is2D = false)
        {
            if (actor.isShaking)
            {
                return;
            }
            actor.isShaking = true;
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
                actor.isShaking = false; //So that we can call this function next time
                OnComplete?.Invoke();
            }
        }

        public static void HangStill(this Actor actor, Vector3 origin, float randomness, Vector3 direction)
        {
            throw new System.NotImplementedException();
        }

        public static void HangStill(this Actor actor, Vector3 origin, float randomness)
        {
            throw new System.NotImplementedException();
        }

        public static void Swing(this Actor actor, Vector3 originPosition, Vector3 centerPosition, Vector3 direction, float angleInDegree, OnCompleteForceLibFunc OnComplete = null)
        {
            throw new System.NotImplementedException();
        }

        public static void Brownian(this Actor actor, float force, float spread, Vector3 plane)
        {
            throw new System.NotImplementedException();
        }

        public static void Brownian(this Actor actor, float force, float spread)
        {
            throw new System.NotImplementedException();
        }

        public static void RubberBandFollow(this Actor actor, Actor to, float followSpeed, float rubberness)
        {
            throw new System.NotImplementedException();
        }
    }
}