using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponPose : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public Transform gunTransform; // Reference to the gun's transform
    public Transform leftHandIKTarget; // Reference to the left hand IK target
    public Transform rightHandIKTarget; // Reference to the right hand IK target
    public Transform[] leftFingerIKTargets; // Array of left finger IK targets
    public Transform[] rightFingerIKTargets; // Array of right finger IK targets

    private Transform leftHandBone;
    private Transform rightHandBone;
    private Transform[] leftFingerBones;
    private Transform[] rightFingerBones;

    private void Start()
    {
        leftHandBone = animator.GetBoneTransform(HumanBodyBones.LeftHand);
        rightHandBone = animator.GetBoneTransform(HumanBodyBones.RightHand);

        leftFingerBones = new Transform[leftFingerIKTargets.Length];
        rightFingerBones = new Transform[rightFingerIKTargets.Length];

        for (int i = 0; i < leftFingerIKTargets.Length; i++)
        {
            leftFingerBones[i] = animator.GetBoneTransform(HumanBodyBones.LeftThumbProximal + i);
        }

        for (int i = 0; i < rightFingerIKTargets.Length; i++)
        {
            rightFingerBones[i] = animator.GetBoneTransform(HumanBodyBones.RightThumbProximal + i);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            if (gunTransform)
            {
                // Set the position and rotation of the left hand IK target
                if (leftHandIKTarget)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTarget.rotation);
                }

                // Set the position and rotation of the right hand IK target
                if (rightHandIKTarget)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKTarget.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandIKTarget.rotation);
                }

                // Set the position and rotation of the left finger IK targets
                if (leftFingerIKTargets != null)
                {
                    for (int i = 0; i < leftFingerIKTargets.Length; i++)
                    {
                        if (leftFingerIKTargets[i])
                        {
                            int fingerBoneIndex = (int)HumanBodyBones.LeftHand;
                            fingerBoneIndex += GetFingerJointOffset(i);

                            animator.SetIKPositionWeight((AvatarIKGoal)fingerBoneIndex, 1f);
                            animator.SetIKRotationWeight((AvatarIKGoal)fingerBoneIndex, 1f);
                            animator.SetIKPosition((AvatarIKGoal)fingerBoneIndex, leftFingerIKTargets[i].position);
                            animator.SetIKRotation((AvatarIKGoal)fingerBoneIndex, leftFingerIKTargets[i].rotation);
                        }
                    }
                }

                // Set the position and rotation of the right finger IK targets
                if (rightFingerIKTargets != null)
                {
                    for (int i = 0; i < rightFingerIKTargets.Length; i++)
                    {
                        if (rightFingerIKTargets[i])
                        {
                            int fingerBoneIndex = (int)HumanBodyBones.RightHand;
                            fingerBoneIndex += GetFingerJointOffset(i);

                            animator.SetIKPositionWeight((AvatarIKGoal)fingerBoneIndex, 1f);
                            animator.SetIKRotationWeight((AvatarIKGoal)fingerBoneIndex, 1f);
                            animator.SetIKPosition((AvatarIKGoal)fingerBoneIndex, rightFingerIKTargets[i].position);
                            animator.SetIKRotation((AvatarIKGoal)fingerBoneIndex, rightFingerIKTargets[i].rotation);
                        }
                    }
                }
            }
            else
            {
                ResetIKWeights();
            }
        }
    }

    private int GetFingerJointOffset(int jointIndex)
    {
        if (jointIndex == 0) // Thumb
            return (int)HumanBodyBones.LeftThumbProximal - (int)AvatarIKGoal.LeftHand;
        else if (jointIndex < 4) // Index, Middle, Ring fingers
            return (int)HumanBodyBones.LeftIndexProximal - (int)AvatarIKGoal.LeftHand + jointIndex;
        else // Pinky finger
            return (int)HumanBodyBones.LeftLittleProximal - (int)AvatarIKGoal.LeftHand + jointIndex;
    }

    private void ResetIKWeights()
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);

        for (int i = 0; i < leftFingerIKTargets.Length; i++)
        {
            int fingerBoneIndex = (int)HumanBodyBones.LeftHand;
            fingerBoneIndex += GetFingerJointOffset(i);
            animator.SetIKPositionWeight((AvatarIKGoal)fingerBoneIndex, 0f);
            animator.SetIKRotationWeight((AvatarIKGoal)fingerBoneIndex, 0f);
        }

        for (int i = 0; i < rightFingerIKTargets.Length; i++)
        {
            int fingerBoneIndex = (int)HumanBodyBones.RightHand;
            fingerBoneIndex += GetFingerJointOffset(i);
            animator.SetIKPositionWeight((AvatarIKGoal)fingerBoneIndex, 0f);
            animator.SetIKRotationWeight((AvatarIKGoal)fingerBoneIndex, 0f);
        }
    }
}