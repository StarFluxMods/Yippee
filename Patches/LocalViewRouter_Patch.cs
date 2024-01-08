using System.Reflection;
using HarmonyLib;
using Kitchen;
using Kitchen.Components;
using KitchenLib.Utils;
using UnityEngine;
using UnityEngine.AI;
using Yipee.Misc;

namespace Yipee.Patches
{
	[HarmonyPatch(typeof(LocalViewRouter), "GetPrefab")]
	public class LocalViewRouter_Patch
	{
		private static GameObject Prefab = null;
		private static FieldInfo assetDirectory = ReflectionUtils.GetField<LocalViewRouter>("AssetDirectory");
		
		static bool Prefix(LocalViewRouter __instance, ViewType view_type, ref GameObject __result)
		{
			if (view_type != (ViewType)VariousUtils.GetID("hoardingBugCustomer")) return true;
			
			if (Prefab == null)
			{
				AssetDirectory assetDirectoryValue = (AssetDirectory)assetDirectory.GetValue(__instance);

				if (assetDirectoryValue == null)
					return true;
					
				if (!assetDirectoryValue.ViewPrefabs.TryGetValue(ViewType.Customer, out GameObject customerPrefab))
					return true;
					
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
                    
				Prefab = hoardingBugCustomer;
			}
				
			__result = Prefab;
			return false;
		}
	}
}