using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract partial class Actor : MonoBehaviour
    {
        public void Jump(Vector3 direction, float height, OnDoAnything OnComplete = null, bool acceleration = true)
        {
            throw new System.NotImplementedException();
        }

        public void Helix(Vector3 direction, float height, float within, OnDoAnything OnComplete = null, bool revertBack = true)
        {
            throw new System.NotImplementedException();
        }

        public void TurnTo(Actor to, float within, OnDoAnything OnComplete = null, ActorForceEase ease = ActorForceEase.Linear)
        {
            throw new System.NotImplementedException();
        }

        public void MoveTo(Actor to, float within, OnDoAnything OnComplete = null, ActorForceEase ease = ActorForceEase.Linear)
        {
            throw new System.NotImplementedException();
        }

        public void TurnTo(Vector3 to, float within, OnDoAnything OnComplete = null, ActorForceEase ease = ActorForceEase.Linear)
        {
            throw new System.NotImplementedException();
        }

        public void MoveTo(Vector3 to, float within, OnDoAnything OnComplete = null, ActorForceEase ease = ActorForceEase.Linear)
        {
            throw new System.NotImplementedException();
        }

        public void Thurst(Vector3 direction, float initialBackwardTime, float restTime, OnDoAnything OnComplete = null)
        {
            throw new System.NotImplementedException();
        }

        public void Swirl(Vector3 singularityPosition, float centerTime, float upwardFactor, OnDoAnything OnComplete = null)
        {
            throw new System.NotImplementedException();
        }

        public void Pop(float popTime, float rubberness, OnDoAnything OnComplete = null)
        {
            throw new System.NotImplementedException();
        }

        public void SpringRelease(Vector3 direction, float force, OnDoAnything OnComplete = null)
        {
            throw new System.NotImplementedException();
        }

        public void Summon(Vector3 direction, Vector3 position, float force, OnDoAnything OnComplete = null)
        {
            throw new System.NotImplementedException();
        }

        public void Summon(Vector3 position, float force, OnDoAnything OnComplete = null)
        {
            throw new System.NotImplementedException();
        }

        public void Shake(float shakeDuration, float decreasePoint, OnDoAnything OnComplete = null, bool is2D = false)
        {
            if (isShaking)
            {
                return;
            }
            isShaking = true;
            StartCoroutine(shakeGameObjectCOR(_transform, shakeDuration, decreasePoint, is2D));

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
                isShaking = false; //So that we can call this function next time
                OnComplete?.Invoke();
            }
        }

        public void HangStill(Vector3 origin, float randomness, Vector3 direction)
        {
            throw new System.NotImplementedException();
        }

        public void HangStill(Vector3 origin, float randomness)
        {
            throw new System.NotImplementedException();
        }

        public void Swing(Vector3 originPosition, Vector3 centerPosition, Vector3 direction, float angleInDegree, OnDoAnything OnComplete = null)
        {
            throw new System.NotImplementedException();
        }

        public void Brownian(float force, float spread, Vector3 plane)
        {
            throw new System.NotImplementedException();
        }

        public void Brownian(float force, float spread)
        {
            throw new System.NotImplementedException();
        }

        public void RubberBandFollow(Actor to, float followSpeed, float rubberness)
        {
            throw new System.NotImplementedException();
        }
    }
}