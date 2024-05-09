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

    [SerializeField]
    private QuestController _questController;
    
    /*[SerializeField]
    private WireGameController _wireGameController;*/

    [SerializeField]
    private VirtualCameraBehaviour _virtualCameras;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Player>()
            .FromInstance(_player)
            .AsSingle();

        Container.BindInterfacesAndSelfTo<QuestController>()
            .FromInstance(_questController)
            .AsSingle();
        
        Container.Bind<Camera>()
            .WithId(MainCameraId)
            .FromInstance(_mainCamera)
            .AsSingle();

        Container
            .Bind<PlayerInputController>()
            .AsSingle()
            .NonLazy();

        Container
           .BindInterfacesTo<VirtualCameraBehaviour>()
           .FromInstance(_virtualCameras)
           .AsSingle();
    }
}
