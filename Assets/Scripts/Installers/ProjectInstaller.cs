using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<DefaultCancellation>()
            .FromNew()
            .AsSingle()
            .NonLazy();
    }
}
