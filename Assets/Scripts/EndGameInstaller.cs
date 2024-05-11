using UnityEngine;
using Zenject;

public class EndGameInstaller : MonoInstaller
{
    [SerializeField] private EndGame _endGameScene;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<EndGame>().FromComponentInNewPrefab(_endGameScene).AsSingle().NonLazy();
    }
}
