using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EndGame : MonoBehaviour
{
    [Inject] private SceneController controller;
    [SerializeField] private Button _startAgain;

    private void Start()
    {
        controller.StartNewScene();
        _startAgain.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        controller.LoarStartScene();
    }

}
