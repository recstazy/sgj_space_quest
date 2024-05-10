using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    [SerializeField] private MenuScene _startScene;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MenuScene>().FromComponentInNewPrefab(_startScene).AsSingle().NonLazy();
    }
}
