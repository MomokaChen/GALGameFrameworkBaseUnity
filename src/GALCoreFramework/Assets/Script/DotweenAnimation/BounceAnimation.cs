using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BounceAnimation : MonoBehaviour
{
    [Header("初始方向设置")]
    [Tooltip("勾选：角色初始朝右\n不勾选：角色初始朝左")] public bool startFaceRight = true;
    private int faceDirection;

    [Header("跳跃移动参数")]
    [Tooltip("左右跳跃最小距离")] public float horizontalRangeMin = 1f;
    [Tooltip("左右跳跃最大距离")] public float horizontalRangeMax = 2f;
    [Tooltip("跳跃高度")] public float jumpHeight = 1.5f;
    [Tooltip("跳跃一次花费的时间")] public float jumpTime = 0.7f;

    [Header("随机延迟参数")]
    [Tooltip("跳跃前最小等待时间")] public float waitMin = 1f;
    [Tooltip("跳跃前最大等待时间")] public float waitMax = 4f;

    [Header("落地三次弹跳")]
    [Tooltip("第一次弹跳高度")] public float bounce1 = 0.5f;
    [Tooltip("第二次弹跳高度")] public float bounce2 = 0.3f;
    [Tooltip("第三次弹跳高度")] public float bounce3 = 0.2f;
    [Tooltip("弹跳速度(越小越快)")] public float bounceDur = 0.1f;

    [Header("形变动画参数")]
    [Tooltip("落地挤压程度")][Range(0.7f, 0.9f)] public float landSquashY = 0.9f;
    [Tooltip("起跳拉伸程度")][Range(1.05f, 1.2f)] public float jumpStretchY = 1.05f;
    [Tooltip("形变动画速度")] public float deformTime = 0.12f;
    [Tooltip("落地弹性恢复速度")] public float bounceElastic = 1.2f;

    [Header("智能平衡随机（自动左右均衡）")]
    [Tooltip("倾向强度：越大越容易被拉回中间")]
    public float balanceStrength = 1.0f;
    private float _directionBalance = 0;

    private Transform _spriteTrans;
    private Vector3 _currentPos; //改成当前位置
    private Vector3 _originalImageScale;

    void Start()
    {
        _spriteTrans = transform;
        _currentPos = transform.position; //初始化当前位置
        _originalImageScale = _spriteTrans.localScale;
        faceDirection = startFaceRight ? 1 : -1;

        StartCoroutine(PlayBounceLoop());
    }

    IEnumerator PlayBounceLoop()
    {
        yield return new WaitForSeconds(RandomWaitTime());

        while (true)
        {
            int nextJumpDir = GetBalancedRandomDirection();

            //先翻转到正确方向
            if (nextJumpDir != faceDirection)
                ReverseDir();

            float distance = RandomRangeDistance();

            //从当前位置跳，不是从原点跳！
            float targetX = _currentPos.x + faceDirection * distance;

            JumpTo(targetX);
            yield return new WaitForSeconds(jumpTime);

            //跳完更新当前位置
            _currentPos.x = targetX;

            DoRealBounceThreeTimes();
            yield return new WaitForSeconds(bounceDur * 6);

            yield return new WaitForSeconds(RandomWaitTime());
        }
    }

    int GetBalancedRandomDirection()
    {
        float rightProb = 0.5f + (_directionBalance * balanceStrength);
        rightProb = Mathf.Clamp(rightProb, 0.1f, 0.9f);
        int dir = Random.value < rightProb ? 1 : -1;
        _directionBalance += (dir == 1) ? -1 : 1;
        return dir;
    }

    void ReverseDir()
    {
        Vector3 scale = _spriteTrans.localScale;
        scale.x *= -1;
        _spriteTrans.localScale = scale;
        faceDirection *= -1;
    }

    void JumpTo(float targetX)
    {
        DoJumpStretch();
        transform.DOMoveX(targetX, jumpTime).SetEase(Ease.InOutSine);
        Sequence jumpY = DOTween.Sequence();
        jumpY.Append(transform.DOMoveY(_currentPos.y + jumpHeight, jumpTime / 2).SetEase(Ease.OutQuad));
        jumpY.Append(transform.DOMoveY(_currentPos.y, jumpTime / 2).SetEase(Ease.InQuad));
    }

    void DoJumpStretch()
    {
        DOTween.Sequence()
            .Append(_spriteTrans.DOScaleY(jumpStretchY, deformTime))
            .Append(_spriteTrans.DOScaleY(_originalImageScale.y, deformTime));
    }

    void DoRealBounceThreeTimes()
    {
        Sequence s = DOTween.Sequence();
        s.Append(transform.DOMoveY(_currentPos.y + bounce1, bounceDur).SetEase(Ease.OutQuad));
        s.Append(transform.DOMoveY(_currentPos.y, bounceDur).SetEase(Ease.InQuad));
        s.Append(transform.DOMoveY(_currentPos.y + bounce2, bounceDur).SetEase(Ease.OutQuad));
        s.Append(transform.DOMoveY(_currentPos.y, bounceDur).SetEase(Ease.InQuad));
        s.Append(transform.DOMoveY(_currentPos.y + bounce3, bounceDur).SetEase(Ease.OutQuad));
        s.Append(transform.DOMoveY(_currentPos.y, bounceDur).SetEase(Ease.InQuad));
        DoLandSquash();
    }

    void DoLandSquash()
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(_spriteTrans.DOScaleY(landSquashY, deformTime));
        sq.Append(_spriteTrans.DOScaleY(_originalImageScale.y, deformTime * 1.2f).SetEase(Ease.OutElastic, bounceElastic));
    }

    float RandomWaitTime() => Random.Range(waitMin, waitMax);
    float RandomRangeDistance() => Random.Range(horizontalRangeMin, horizontalRangeMax);
}