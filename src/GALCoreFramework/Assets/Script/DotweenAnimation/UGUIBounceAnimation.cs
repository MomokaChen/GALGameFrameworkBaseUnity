using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UGUIBounceAnimation : MonoBehaviour
{
    [Header("初始方向设置")]
    [Tooltip("勾选：角色初始朝右\n不勾选：角色初始朝左")]public bool startFaceRight = true;
    private int faceDirection;

    [Header("跳跃移动参数")]
    [Tooltip("左右跳跃最小距离")]public float horizontalRangeMin = 80f;
    [Tooltip("左右跳跃最大距离")]public float horizontalRangeMax = 120f;
    [Tooltip("跳跃高度")]public float jumpHeight = 80f;
    [Tooltip("跳跃一次花费的时间")]public float jumpTime = 0.7f;

    [Header("随机延迟参数")]
    [Tooltip("跳跃前最小等待时间")]public float waitMin = 1f;
    [Tooltip("跳跃前最大等待时间")]public float waitMax = 4f;


    [Header("落地三次弹跳")]
    [Tooltip("第一次弹跳高度")]public float bounce1 = 30;
    [Tooltip("第二次弹跳高度")]public float bounce2 = 16f;
    [Tooltip("第三次弹跳高度")]public float bounce3 = 6f;
    [Tooltip("弹跳速度(越小越快)")]public float bounceDur = 0.1f;

    [Header("形变动画参数")]
    [Tooltip("落地挤压程度")][Range(0.7f, 0.9f)]public float landSquashY = 0.85f;
    [Tooltip("起跳拉伸程度")][Range(1.05f, 1.2f)]public float jumpStretchY = 1.1f;
    [Tooltip("形变动画速度")]public float deformTime = 0.12f;
    [Tooltip("落地弹性恢复速度")]public float bounceElastic = 1.2f;


    private Transform _spriteTrans;
    private Vector3 _originPos;
    private Vector3 _originalImageScale;

    void Start()
    {
        _spriteTrans = transform.Find("Image");
        if (_spriteTrans == null) return;

        _originPos = transform.position;
        _originalImageScale = _spriteTrans.localScale; // 保存图片原生方向
        faceDirection = startFaceRight ? 1 : -1;

        StartCoroutine(PlayBounceLoop());
    }

    /// <summary>
    /// 玩家跳跃循环
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayBounceLoop()
    {
        yield return new WaitForSeconds(RandomWaitTime());

        while (true)
        {
            // 1. 先决定下一次跳哪边
            int nextJumpDir = -faceDirection;

            if (nextJumpDir != faceDirection)
            {
                ReverseDir();
            }

            float distance = RandomRangeDistance();
            float targetX = _originPos.x + faceDirection * distance;

            JumpTo(targetX);
            yield return new WaitForSeconds(jumpTime);

            DoRealBounceThreeTimes();
            yield return new WaitForSeconds(bounceDur * 6);

            yield return new WaitForSeconds(RandomWaitTime());
        }
    }

    /// <summary>
    /// 反转方向
    /// </summary>
    void ReverseDir()
    {
        Vector3 scale = _spriteTrans.localScale;
        scale.x *= -1;
        _spriteTrans.localScale = scale;
        faceDirection *= -1;
    }

    /// <summary>
    /// 执行跳跃移动
    /// </summary>
    void JumpTo(float targetX)
    {
        DoJumpStretch();
        transform.DOMoveX(targetX, jumpTime).SetEase(Ease.InOutSine);
        Sequence jumpY = DOTween.Sequence();
        jumpY.Append(transform.DOMoveY(_originPos.y + jumpHeight, jumpTime / 2).SetEase(Ease.OutQuad));
        jumpY.Append(transform.DOMoveY(_originPos.y, jumpTime / 2).SetEase(Ease.InQuad));
    }

    /// <summary>
    /// 落地挤压动画
    /// </summary>
    void DoJumpStretch()
    {
        DOTween.Sequence()
            .Append(_spriteTrans.DOScaleY(jumpStretchY, deformTime))
            .Append(_spriteTrans.DOScaleY(_originalImageScale.y, deformTime));
    }

    /// <summary>
    /// 落地弹跳
    /// </summary>
    void DoRealBounceThreeTimes()
    {
        Sequence s = DOTween.Sequence();
        s.Append(transform.DOMoveY(_originPos.y + bounce1, bounceDur).SetEase(Ease.OutQuad));
        s.Append(transform.DOMoveY(_originPos.y, bounceDur).SetEase(Ease.InQuad));
        s.Append(transform.DOMoveY(_originPos.y + bounce2, bounceDur).SetEase(Ease.OutQuad));
        s.Append(transform.DOMoveY(_originPos.y, bounceDur).SetEase(Ease.InQuad));
        s.Append(transform.DOMoveY(_originPos.y + bounce3, bounceDur).SetEase(Ease.OutQuad));
        s.Append(transform.DOMoveY(_originPos.y, bounceDur).SetEase(Ease.InQuad));
        DoLandSquash();
    }

    /// <summary>
    /// 获取随机等待时间
    /// </summary>
    /// <returns>随机等待时间</returns>
    void DoLandSquash()
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(_spriteTrans.DOScaleY(landSquashY, deformTime));
        sq.Append(_spriteTrans.DOScaleY(_originalImageScale.y, deformTime * 1.2f).SetEase(Ease.OutElastic, bounceElastic));
    }

    /// <summary>
    /// 获取随机等待时间
    /// </summary>
    /// <returns>随机等待时间</returns>
    float RandomWaitTime() => Random.Range(waitMin, waitMax);

    /// <summary>
    /// 获取随机跳跃距离
    /// </summary>
    /// <returns>随机跳跃距离</returns>
    float RandomRangeDistance() => Random.Range(horizontalRangeMin, horizontalRangeMax);
}
