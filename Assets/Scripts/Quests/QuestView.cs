using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class QuestView : MonoBehaviour
{
    [SerializeField]
    private RectTransform _questRoot;

    [SerializeField]
    private TMP_Text _text;

    [SerializeField]
    private RectTransform _checkMark;

    [SerializeField]
    private Ease _showEase;

    [SerializeField]
    private Ease _hideEase;

    [SerializeField]
    private float _showHideTime;

    [SerializeField]
    private float _completeCheckMarkAnimTime;

    [SerializeField]
    private Ease _completeCheckMarkEase;

    [SerializeField]
    private float _completeCheckMarkStartScale;

    public string Text => _text.text;
    
    public void Setup(string text, bool isComplete)
    {
        _text.text = text;
        _checkMark.gameObject.SetActive(isComplete);
    }

    public UniTask Show(CancellationToken cancellation)
    {
        var parentRect = (RectTransform)transform.parent;
        _questRoot.anchoredPosition = new Vector2(-parentRect.rect.min.x - _questRoot.rect.width * 1.5f, _questRoot.anchoredPosition.y);
        var tween = _questRoot.DOAnchorPosX(0, _showHideTime).SetEase(_showEase);
        return tween.ToUniTask(cancellationToken: cancellation);
    }

    public UniTask ShowCompleteAnim(CancellationToken cancellation)
    {
        _checkMark.gameObject.SetActive(true);
        _checkMark.localScale = Vector3.one * _completeCheckMarkStartScale;
        var tween = _checkMark.DOScale(1, _completeCheckMarkAnimTime).SetEase(_completeCheckMarkEase);
        return tween.ToUniTask(cancellationToken: cancellation);
    }
    
    public async UniTask CompleteAndHide(CancellationToken cancellation)
    {
        await ShowCompleteAnim(cancellation);
        var parentRect = (RectTransform)transform.parent;
        _questRoot.anchoredPosition = new Vector2(0, _questRoot.anchoredPosition.y);
        var tween = _questRoot.DOAnchorPosX(-_questRoot.rect.width - parentRect.rect.x, _showHideTime).SetEase(_hideEase);
        await tween.ToUniTask(cancellationToken: cancellation);
    }
}
