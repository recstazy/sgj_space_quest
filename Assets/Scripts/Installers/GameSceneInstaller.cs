using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public const string MainCameraId = nameof(MainCameraId);
        
    [SerializeField]
    private Camera _mainCamera;
        
    public override void InstallBindings()
    {
        Container.Bind<Camera>()
            .WithId(MainCameraId)
            .FromInstance(_mainCamera)
            .AsSingle();
    }
}
