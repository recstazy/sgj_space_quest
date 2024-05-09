using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public const string MainCameraId = nameof(MainCameraId);
        
    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private WireGameController _wireGameController;

    public override void InstallBindings()
    {
        Container.Bind<Camera>()
            .WithId(MainCameraId)
            .FromInstance(_mainCamera)
            .AsSingle();

        Container
            .Bind<PlayerInputController>()
            .AsSingle()
            .NonLazy();

        Container
            .BindInterfacesTo<WireGameController>()
            .FromInstance(_wireGameController)
            .AsSingle();
    }
}
