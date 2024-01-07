using System.Reflection;
using Kitchen;
using Kitchen.Components;
using KitchenLib.Utils;
using KitchenMods;
using UnityEngine;
using UnityEngine.AI;
using Yipee.Misc;

namespace Yipee.Systems
{
    public class CreateViewType : GameSystemBase, IModSystem
    {
        private bool done = false;
        protected override void OnUpdate()
        {
            if (!HasSingleton<SAssetDirectory>() || done) return;
            if (!AssetDirectory.ViewPrefabs.TryGetValue(ViewType.Customer, out GameObject customerPrefab)) return;
            
            GameObject hoardingBugCustomer = Mod.Bundle.LoadAsset<GameObject>("HoardingBugCustomer").AssignMaterialsByNames();
            Animator customerAnimator = customerPrefab.GetComponentInChildren<Animator>();
                    
            NavMeshAgent navMeshAgent = hoardingBugCustomer.GetComponent<NavMeshAgent>();
            Rigidbody rigidbody = hoardingBugCustomer.GetComponent<Rigidbody>();
            Animator animator = hoardingBugCustomer.GetComponentInChildren<Animator>();
            CustomerView customerView = hoardingBugCustomer.AddComponent<CustomerView>();

            FieldInfo customerViewAgent = ReflectionUtils.GetField<CustomerView>("Agent");
            FieldInfo customerViewRigidbody = ReflectionUtils.GetField<CustomerView>("Rigidbody");
            FieldInfo customerViewAnimator = ReflectionUtils.GetField<CustomerView>("Animator");
                    
            animator.avatar = customerAnimator.avatar;
            animator.runtimeAnimatorController = customerAnimator.runtimeAnimatorController;
                    
            customerViewAgent.SetValue(customerView, navMeshAgent);
            customerViewRigidbody.SetValue(customerView, rigidbody);
            customerViewAnimator.SetValue(customerView, animator);
                    
            hoardingBugCustomer.layer = customerPrefab.layer;
                    
            SoundSource soundSource = hoardingBugCustomer.AddComponent<SoundSource>();
            soundSource.Configure(SoundCategory.Effects, Mod.Bundle.LoadAsset<AudioClip>("yippee-tbh"));
            soundSource.ShouldLoop = false;
                    
            ControlAudio controlAudio = hoardingBugCustomer.AddComponent<ControlAudio>();
            controlAudio.animator = animator;
            controlAudio.soundSource = soundSource;
                    
            AssetDirectory.ViewPrefabs.Add((ViewType)VariousUtils.GetID("hoardingBugCustomer"), hoardingBugCustomer);
            
            done = true;
        }
    }
}