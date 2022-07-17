#define GAMEPLAY_SDK_DEBUG_MODE
#define GAMEPLAY_SUPPORT_NOSTANDARD_COLS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public abstract class FPhysicsShape : MonoBehaviour
    {
        Transform tr;
        public Transform _Transform { get { return tr; } }
        private void Awake()
        {
            tr = transform;
        }

        bool IsValid(ReactorActor rival, ReactorActor thisActor)
        {
            var valid = true;
#if GAMEPLAY_SDK_DEBUG_MODE
            if (thisActor == null) { valid = false; }
            if (rival == null) { valid = false; }
            if (rival != null && rival == thisActor) { valid = false; }
#else
            if (ReferenceEquals(rival, thisActor)) { valid = false; }
#endif
            return valid;
        }

#if GAMEPLAY_SDK_DEBUG_MODE
        void PreCheckPhysics(ActorLevelModule actorModule, Collider rivalCollider)
        {
            if (actorModule == null)
            {
                KLog.ThrowGameplaySDKException(GFType.Reaction, "Actor Level Module is null!");
            }
            else
            {
                if (actorModule.ReactorShapes.ContainsKey(this) == false)
                {
                    KLog.ThrowGameplaySDKException(GFType.Reaction, "Physical shape is used for gameobject: " + gameObject.name +
                        " but it has no owner reactor actor. Physical shapes are allowed to work only with reactor actors!");
                }

                if (actorModule.ReactorColliders.ContainsKey(rivalCollider) == false)
                {
                    KLog.ThrowGameplaySDKException(GFType.Reaction, "3D Collider is used for gameobject: " + gameObject.name +
                        " but it has no corresponding owner reactor actor. Colliders(2D/3D) are allowed to work only with reactor actors!");
                }

                if (actorModule.Shapes.ContainsKey(rivalCollider) == false)
                {
                    KLog.ThrowGameplaySDKException(GFType.Reaction, "3D Collider is used for gameobject: " + gameObject.name +
                        " but it has no corresponding physical shape. Colliders(2D/3D) are allowed to exist only with Physical shapes!");
                }

                if (actorModule.Shapes_RV.ContainsKey(this) == false)
                {
                    KLog.ThrowGameplaySDKException(GFType.Reaction, "Shape is used for gameobject: " + gameObject.name +
                        " but it has no corresponding 3D collider. Colliders(2D/3D) are allowed to exist only with Physical shapes!");
                }
            }
        }

        void PreCheckPhysics(ActorLevelModule actorModule, Collider2D rivalCollider)
        {
            if (actorModule == null)
            {
                KLog.ThrowGameplaySDKException(GFType.Reaction, "Actor Level Module is null!");
            }
            else
            {
                if (actorModule.ReactorShapes.ContainsKey(this) == false)
                {
                    KLog.ThrowGameplaySDKException(GFType.Reaction, "Physical shape is used for gameobject: " + gameObject.name +
                        " but it has no owner reactor actor. Physical shapes are allowed to work only with reactor actors!");
                }

                if (actorModule.ReactorColliders2D.ContainsKey(rivalCollider) == false)
                {
                    KLog.ThrowGameplaySDKException(GFType.Reaction, "2D Collider is used for gameobject: " + gameObject.name +
                        " but it has no corresponding owner reactor actor. Colliders(2D/3D) are allowed to work only with reactor actors!");
                }

                if (actorModule.Shapes2D.ContainsKey(rivalCollider) == false)
                {
                    KLog.ThrowGameplaySDKException(GFType.Reaction, "2D Collider is used for gameobject: " + gameObject.name +
                        " but it has no corresponding physical shape. Colliders(2D/3D) are allowed to exist only with Physical shapes!");
                }

                if (actorModule.Shapes2D_RV.ContainsKey(this) == false)
                {
                    KLog.ThrowGameplaySDKException(GFType.Reaction, "Shape is used for gameobject: " + gameObject.name +
                        " but it has no corresponding 2D collider. Colliders(2D/3D) are allowed to exist only with Physical shapes!");
                }
            }
        }
#endif

        void GetActors(Collider rivalCollider, out ReactorActor thisActor, out ReactorActor rivalActor,
            out FPhysicsShape rivalPhysicalShape)
        {
            var actLvMod = ActorLevelModule.instance;
#if GAMEPLAY_SDK_DEBUG_MODE
            PreCheckPhysics(actLvMod, rivalCollider);
#endif
            thisActor = actLvMod.ReactorShapes[this];
            rivalActor = actLvMod.ReactorColliders[rivalCollider];
            rivalPhysicalShape = actLvMod.Shapes[rivalCollider];

#if GAMEPLAY_SDK_DEBUG_MODE
            ReactorSanityCheck(thisActor, this);
            ReactorSanityCheck(rivalActor, rivalCollider);
            ShapeSanityCheck(rivalPhysicalShape, rivalCollider);

            var thisCol = actLvMod.Shapes_RV[this];
            ShapeSanityCheck(this, thisCol);
#endif
        }

        void GetActors(Collider2D rivalCollider, out ReactorActor thisActor, out ReactorActor rivalActor,
            out FPhysicsShape rivalPhysicalShape)
        {
            var actLvMod = ActorLevelModule.instance;
#if GAMEPLAY_SDK_DEBUG_MODE
            PreCheckPhysics(actLvMod, rivalCollider);
#endif
            thisActor = actLvMod.ReactorShapes[this];
            rivalActor = actLvMod.ReactorColliders2D[rivalCollider];
            rivalPhysicalShape = actLvMod.Shapes2D[rivalCollider];

#if GAMEPLAY_SDK_DEBUG_MODE
            ReactorSanityCheck(thisActor, this);
            ReactorSanityCheck(rivalActor, rivalCollider);
            ShapeSanityCheck(rivalPhysicalShape, rivalCollider);

            var thisCol = actLvMod.Shapes2D_RV[this];
            ShapeSanityCheck(this, thisCol);
#endif
        }

        void GetActors(Collision rivalCollision, out ReactorActor thisActor, out ReactorActor rivalActor,
            out FPhysicsShape rivalPhysicalShape)
        {
            var actLvMod = ActorLevelModule.instance;
#if GAMEPLAY_SDK_DEBUG_MODE
            PreCheckPhysics(actLvMod, rivalCollision.collider);
#endif
            thisActor = actLvMod.ReactorShapes[this];
            rivalActor = actLvMod.ReactorColliders[rivalCollision.collider];
            rivalPhysicalShape = actLvMod.Shapes[rivalCollision.collider];

#if GAMEPLAY_SDK_DEBUG_MODE
            ReactorSanityCheck(thisActor, this);
            ReactorSanityCheck(rivalActor, rivalCollision.collider);
            ShapeSanityCheck(rivalPhysicalShape, rivalCollision.collider);

            var thisCol = actLvMod.Shapes_RV[this];
            ShapeSanityCheck(this, thisCol);
#endif
        }

        void GetActors(Collision2D rivalCollision, out ReactorActor thisActor, out ReactorActor rivalActor,
            out FPhysicsShape rivalPhysicalShape)
        {
            var actLvMod = ActorLevelModule.instance;
#if GAMEPLAY_SDK_DEBUG_MODE
            PreCheckPhysics(actLvMod, rivalCollision.collider);
#endif
            thisActor = actLvMod.ReactorShapes[this];
            rivalActor = actLvMod.ReactorColliders2D[rivalCollision.collider];
            rivalPhysicalShape = actLvMod.Shapes2D[rivalCollision.collider];

#if GAMEPLAY_SDK_DEBUG_MODE
            ReactorSanityCheck(thisActor, this);
            ReactorSanityCheck(rivalActor, rivalCollision.collider);
            ShapeSanityCheck(rivalPhysicalShape, rivalCollision.collider);

            var thisCol = actLvMod.Shapes2D_RV[this];
            ShapeSanityCheck(this, thisCol);
#endif
        }

#if GAMEPLAY_SDK_DEBUG_MODE
        void ReactorSanityCheck(ReactorActor ractor, Collider2D col2D)
        {
            if (ractor == null)
            {
                KLog.ThrowGameplaySDKException(GFType.Reaction, "Corresponding reactor of collider2D: " + col2D.gameObject.name + " is null!." +
                    " This must never be possible since it is framework's job to keep dictionaries sane.");
            }
        }

        void ReactorSanityCheck(ReactorActor ractor, FPhysicsShape fShape2D)
        {
            if (ractor == null)
            {
                KLog.ThrowGameplaySDKException(GFType.Reaction, "Corresponding reactor of Physical Shape: " + fShape2D.gameObject.name + " is null!." +
                    " This must never be possible since it is framework's job to keep dictionaries sane.");
            }
        }

        void ReactorSanityCheck(ReactorActor ractor, Collider col)
        {
            if (ractor == null)
            {
                KLog.ThrowGameplaySDKException(GFType.Reaction, "Corresponding reactor of collider: " + col.gameObject.name + " is null!." +
                    " This must never be possible since it is framework's job to keep dictionaries sane.");
            }
        }

        void ShapeSanityCheck(FPhysicsShape shape, Collider col)
        {
            if (shape == null)
            {
                KLog.ThrowGameplaySDKException(GFType.Reaction, "Corresponding Physical Shape of collider: " + col.gameObject.name + " is null!." +
                    " This must never be possible since it is framework's job to keep Physical shape/Colliders sane.");
            }
        }

        void ShapeSanityCheck(FPhysicsShape shape, Collider2D col)
        {
            if (shape == null)
            {
                KLog.ThrowGameplaySDKException(GFType.Reaction, "Corresponding Physical Shape of 2D collider: " + col.gameObject.name + " is null!." +
                    " This must never be possible since it is framework's job to keep Physical shape/Colliders sane.");
            }
        }
#endif

        internal void OnVolumeEnter(Collider rival)
        {
#if GAMEPLAY_SUPPORT_NOSTANDARD_COLS
            var actLvMod = ActorLevelModule.instance;
            if (actLvMod.ReactorColliders.ContainsKey(rival) == false || actLvMod.Shapes.ContainsKey(rival) == false) { return; }
#endif
            ReactorActor thisActor; ReactorActor rivalActor; FPhysicsShape rivalShape;
            GetActors(rival, out thisActor, out rivalActor, out rivalShape);
            if (!IsValid(rivalActor, thisActor)) { return; }
            thisActor.ProcessVolume(rival, null, rivalActor, true, rivalShape, this);
        }

        internal void OnVolumeExit(Collider rival)
        {
#if GAMEPLAY_SUPPORT_NOSTANDARD_COLS
            var actLvMod = ActorLevelModule.instance;
            if (actLvMod.ReactorColliders.ContainsKey(rival) == false || actLvMod.Shapes.ContainsKey(rival) == false) { return; }
#endif
            ReactorActor thisActor; ReactorActor rivalActor; FPhysicsShape rivalShape;
            GetActors(rival, out thisActor, out rivalActor, out rivalShape);
            if (!IsValid(rivalActor, thisActor)) { return; }
            thisActor.ProcessVolume(rival, null, rivalActor, false, rivalShape, this);
        }

        internal void OnCollisionStart(Collision rival)
        {
#if GAMEPLAY_SUPPORT_NOSTANDARD_COLS
            var actLvMod = ActorLevelModule.instance;
            if (actLvMod.ReactorColliders.ContainsKey(rival.collider) == false || actLvMod.Shapes.ContainsKey(rival.collider) == false) { return; }
#endif
            ReactorActor thisActor; ReactorActor rivalActor; FPhysicsShape rivalShape;
            GetActors(rival, out thisActor, out rivalActor, out rivalShape);
            if (!IsValid(rivalActor, thisActor)) { return; }
            thisActor.ProcessSolid(rival, null, rivalActor, true, rivalShape, this);
        }

        internal void OnCollisionStop(Collision rival)
        {
#if GAMEPLAY_SUPPORT_NOSTANDARD_COLS
            var actLvMod = ActorLevelModule.instance;
            if (actLvMod.ReactorColliders.ContainsKey(rival.collider) == false || actLvMod.Shapes.ContainsKey(rival.collider) == false) { return; }
#endif
            ReactorActor thisActor; ReactorActor rivalActor; FPhysicsShape rivalShape;
            GetActors(rival, out thisActor, out rivalActor, out rivalShape);
            if (!IsValid(rivalActor, thisActor)) { return; }
            thisActor.ProcessSolid(rival, null, rivalActor, false, rivalShape, this);
        }

        internal void OnVolumeEnter2D(Collider2D rival)
        {
#if GAMEPLAY_SUPPORT_NOSTANDARD_COLS
            var actLvMod = ActorLevelModule.instance;
            if (actLvMod.ReactorColliders2D.ContainsKey(rival) == false || actLvMod.Shapes2D.ContainsKey(rival) == false) { return; }
#endif
            ReactorActor thisActor; ReactorActor rivalActor; FPhysicsShape rivalShape;
            GetActors(rival, out thisActor, out rivalActor, out rivalShape);
            if (!IsValid(rivalActor, thisActor)) { return; }
            thisActor.ProcessVolume(null, rival, rivalActor, true, rivalShape, this);
        }

        internal void OnVolumeExit2D(Collider2D rival)
        {
#if GAMEPLAY_SUPPORT_NOSTANDARD_COLS
            var actLvMod = ActorLevelModule.instance;
            if (actLvMod.ReactorColliders2D.ContainsKey(rival) == false || actLvMod.Shapes2D.ContainsKey(rival) == false) { return; }
#endif
            ReactorActor thisActor; ReactorActor rivalActor; FPhysicsShape rivalShape;
            GetActors(rival, out thisActor, out rivalActor, out rivalShape);
            if (!IsValid(rivalActor, thisActor)) { return; }
            thisActor.ProcessVolume(null, rival, rivalActor, false, rivalShape, this);
        }

        internal void OnCollisionStart2D(Collision2D rival)
        {
#if GAMEPLAY_SUPPORT_NOSTANDARD_COLS
            var actLvMod = ActorLevelModule.instance;
            if (actLvMod.ReactorColliders2D.ContainsKey(rival.collider) == false || actLvMod.Shapes2D.ContainsKey(rival.collider) == false) { return; }
#endif
            ReactorActor thisActor; ReactorActor rivalActor; FPhysicsShape rivalShape;
            GetActors(rival, out thisActor, out rivalActor, out rivalShape);
            if (!IsValid(rivalActor, thisActor)) { return; }
            thisActor.ProcessSolid(null, rival, rivalActor, true, rivalShape, this);
        }

        internal void OnCollisionStop2D(Collision2D rival)
        {
#if GAMEPLAY_SUPPORT_NOSTANDARD_COLS
            var actLvMod = ActorLevelModule.instance;
            if (actLvMod.ReactorColliders2D.ContainsKey(rival.collider) == false || actLvMod.Shapes2D.ContainsKey(rival.collider) == false) { return; }
#endif
            ReactorActor thisActor; ReactorActor rivalActor; FPhysicsShape rivalShape;
            GetActors(rival, out thisActor, out rivalActor, out rivalShape);
            if (!IsValid(rivalActor, thisActor)) { return; }
            thisActor.ProcessSolid(null, rival, rivalActor, false, rivalShape, this);
        }
    }
}