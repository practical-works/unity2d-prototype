using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationPicker : MonoBehaviour
{
    public string AnimIntParamName = "ID";
    [Min(0)] public int AnimIntParamValue;
    public bool Randomize = true;
    public bool LiveUpdate;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        PickAnimation();
    }

    private void Update()
    {
        if (LiveUpdate) PickAnimation();
    }

    public void PickAnimation()
    {
        int lastAnimClipIndex = _animator.runtimeAnimatorController.animationClips.Length - 1;
        if (AnimIntParamValue < 0) AnimIntParamValue = 0;
        else if (AnimIntParamValue > lastAnimClipIndex) AnimIntParamValue = lastAnimClipIndex;
        _animator.SetInteger(AnimIntParamName, AnimIntParamValue = Randomize ? Random.Range(0, lastAnimClipIndex) : AnimIntParamValue);
    }

    [ContextMenu("Play Animation")]
    public void PlayAnimation() => _animator.Play(0);
}
