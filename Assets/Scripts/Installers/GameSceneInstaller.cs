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
    private Player _player;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Player>()
            .FromInstance(_player)
            .AsSingle();
        
        Container.Bind<Camera>()
            .WithId(MainCameraId)
            .FromInstance(_mainCamera)
            .AsSingle();

        Container
            .Bind<PlayerInputController>()
            .AsSingle()
            .NonLazy();
    }
}
