using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField]
    private SceneController _sceneController;

    [SerializeField]
    private MixerSettings _mixerSettings;
 
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<DefaultCancellation>()
            .FromNew()
            .AsSingle()
            .NonLazy();

        Container
            .BindInterfacesAndSelfTo<SceneController>()
            .FromComponentInNewPrefab(_sceneController)
            .AsSingle()
        .NonLazy();

        Container
            .BindInterfacesAndSelfTo<MixerSettings>()
            .FromComponentInNewPrefab(_mixerSettings)
            .AsSingle()
            .NonLazy();
    }
}
