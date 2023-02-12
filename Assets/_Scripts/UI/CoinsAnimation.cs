using System.Collections;
using _Scripts.Money_Logic;
using _Scripts.UI.Upgrade;
using DG.Tweening;
using QFSW.MOP2;
using UnityEngine;
using Zenject;

public class CoinsAnimation : MonoBehaviour
{
    #region Variables
    [SerializeField] private bool isDisabled;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform overlayCanvas;
    [SerializeField] private Transform screenCanvas;
    [SerializeField] private Transform coinsPosition;
    [SerializeField] private GameObject coinPrefab;
    [Space(10)]
    [SerializeField] private float motionDuration;
    [SerializeField] private float waitDuration = 0.3f;
    [SerializeField] private float scaleAnimationDuration;
    [SerializeField] private float startScale = 0.2f;

    private WaitForSecondsRealtime _motionWait;
    private WaitForSecondsRealtime _effectWait;

    private const float CoinsIconAnimationScale = 1.5f;
    private const float CoinsIconAnimationDuration = 0.1f;

    private Tween _iconTween;

    private Vector3 _targetPosition;
    
    [Inject] private UpgradeMenu _upgradeMenu;
    [Inject] private MoneyWallet _moneyWallet;
    #endregion

    #region Monobehaviour Callbaccks
    private void Awake()
    {
        _motionWait = new WaitForSecondsRealtime(waitDuration);
        _effectWait = new WaitForSecondsRealtime(motionDuration);
    }
    #endregion
    
    public void CollectCoins(Transform worldPosition, int amount)
    {
        if (isDisabled)
            return;
        
        var coin = Instantiate(coinPrefab, overlayCanvas);
        LocateCoin(coin.transform, worldPosition.position);
        StartCoroutine(CoinLife(coin.transform, amount));
    }
    
    private IEnumerator CoinLife(Transform coin, int amount)
    {
        coin.localScale = Vector3.one * startScale;
        yield return _motionWait;
        var scaleTween = coin.DOScale(Vector3.one, scaleAnimationDuration);
        var moveTween = coin.DOMove(coinsPosition.position, motionDuration);
        yield return _effectWait;

        scaleTween.Kill();
        moveTween.Kill();
        Destroy(coin.gameObject);
        //TryAnimateIcon();
        
        /*var reward = amount * _upgradeMenu.IncomeCoefficient;
        _moneyWallet.Add((int) reward);*/
    }

    private void LocateCoin(Transform coin, Vector3 worldPosition)
    {
        var targetPosition = mainCamera.WorldToScreenPoint(worldPosition);
        coin.position = targetPosition;
        var localPosition = coin.transform.localPosition;
        
        coin.SetParent(screenCanvas);
        coin.SetSiblingIndex(0);
        coin.localPosition = localPosition;
        coin.localScale = Vector3.one;
    }

    private void TryAnimateIcon()
    {
        if (_iconTween != null)
        {
            _iconTween.Rewind();
            _iconTween.Kill();
        }

        _iconTween = coinsPosition
                    .DOScale(coinsPosition.localScale * CoinsIconAnimationScale, CoinsIconAnimationDuration)
                    .SetLoops(2, LoopType.Yoyo);
    }
}
