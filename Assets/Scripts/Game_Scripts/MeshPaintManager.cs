using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeshPaintManager : MonoBehaviour
{
    public delegate void LoadingStartUICallback_(string _text);
    public delegate void LoadingEndUICallback_();

    private LoadingStartUICallback_ loadingStartCallback_;
    private LoadingEndUICallback_ loadingEndCallback_;

    [SerializeField] GameManager gameManager_;
    [SerializeField] PlayerKeyInput keyInput_;
    [SerializeField] PlayerFocusManager focusManager_;

    private List<MeshPaintTarget> meshTargetList_ = null;


    private void Awake()
    {
        // Scene�� �����ϴ� Object�� ������� MeshPaintTarget�� �����ɴϴ�.
        MeshPaintTarget[] targets = FindObjectsOfType<MeshPaintTarget>();
        meshTargetList_ = new List<MeshPaintTarget>(targets);
#if UNITY_EDITOR
        Debug.Log("[MeshPaintManager] target Count: " + meshTargetList_.Count);
#endif
    }

    private void Update()
    {
        if (!gameManager_.isInGame_) return;
        /*        // �ӽ� Save ��ư
                if (Input.GetKeyDown(KeyCode.O))
                {
                    SaveTargetMask();
                }*/
        // �ӽ� Reset ��ư
        if (Input.GetKeyDown(KeyCode.P))
        {
            ResetTargetMask();
        }
    }


    public void Init(LoadingStartUICallback_ _loadStartCallback, LoadingEndUICallback_ _loadEndCallback)
    {
        if (meshTargetList_.Count <= 0) return;

        loadingStartCallback_ = _loadStartCallback;
        loadingEndCallback_ = _loadEndCallback;

        InitTarget();
        LoadTargetMask();
    }


    private void InitTarget()
    {
        //foreach (MeshPaintTarget target_ in meshTargetList_)
        //{
        //    target_.Init();
        //}
        StopCoroutine("InitTargetCoroutine");
        StartCoroutine("InitTargetCoroutine");
    }


    public void SaveTargetMask()
    {
        // ���� Section�� �׸��� ����� ���� ����� ��
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.IsDrawable() && target_.IsClear() == false)
            {
                target_.SaveMask();
            }
        }
    }

    public void LoadTargetMask()
    {
        //// �׸��� ����� �ƴϾ �ٸ� ������ ���൵�� �� �� �ֵ��� �ϱ� ���� ���� ����
        //foreach (MeshPaintTarget target_ in meshTargetList_)
        //{
        //    if (target_.LoadMask() == false)
        //    {
        //        Debug.LogWarning("�Ϻ� ����� Mask�� �ҷ����µ� �����Ͽ����ϴ�." + target_.name);
        //    }
        //}
        StopCoroutine("LoadTargetMaskCoroutine");
        StartCoroutine("LoadTargetMaskCoroutine");
    }

    public void ResetTargetMask()
    {
        //foreach (MeshPaintTarget target_ in meshTargetList_)
        //{
        //    if (target_.IsDrawable() && target_.IsClear() == false && target_.GetProcessPercent() > 0.0001f)
        //    {
        //        if (target_.ResetMask() == false)
        //        {
        //            Debug.LogWarning("�Ϻ� ����� Mask�� �ʱ�ȭ�ϴµ� �����Ͽ����ϴ�.");
        //        }
        //    }
        //}
        StopCoroutine("ResetTargetMaskCoroutine");
        StartCoroutine("ResetTargetMaskCoroutine");
    }


    private void SetLoadScreen(string _type, int _cnt, int _total)
    {
        StringBuilder sb = new StringBuilder();
        int percent = Mathf.FloorToInt(_cnt / float.Parse(_total.ToString()) * 100);

        string text = string.Empty;
        switch (_type)
        {
            case "init":
                sb.Append("�ʱ�ȭ�ϴ� ��");
                sb.Append("...");
                break;
            case "load":
                sb.Append("�ҷ����� ��");
                sb.Append("...");
                break;
            case "reset":
                sb.Append("�����ϴ� ��");
                sb.Append("...");
                loadingStartCallback_?.Invoke(sb.ToString());
                return;
            default:
                sb.Append("...");
                loadingStartCallback_?.Invoke(sb.ToString());
                return;
        }
        //sb.Append(_cnt);
        //sb.Append("/");
        //sb.Append(_total);
        sb.Append("(");
        sb.Append(percent);
        sb.Append("%)");

        loadingStartCallback_?.Invoke(sb.ToString());
    }
    
    private void IsPlayerInputKeyLock(bool _lock)
    {
        if (_lock)
        {
            keyInput_.useKey = false; // ���� �߿��� Ű �Է� ����
            focusManager_.isFocusLock_ = true;
        }
        else
        {
            keyInput_.useKey = true;
            focusManager_.isFocusLock_ = false;
        }
    }

    private IEnumerator InitTargetCoroutine()
    {
        int count = 0;
        string type = "init";
        SetLoadScreen(type, count, meshTargetList_.Count);
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            target_.Init();
            ++count;
            SetLoadScreen(type, count, meshTargetList_.Count);
            yield return null;
        }
        loadingEndCallback_?.Invoke();
    }
    private IEnumerator LoadTargetMaskCoroutine()
    {
        int count = 0;
        string type = "load";
        SetLoadScreen(type, count, meshTargetList_.Count);
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.LoadMask() == false)
            {
                Debug.LogWarning("�Ϻ� ����� Mask�� �ҷ����µ� �����Ͽ����ϴ�." + target_.name);
            }
            else
            {
                ++count;
                SetLoadScreen(type, count, meshTargetList_.Count);
            }
            yield return null;
        }
        loadingEndCallback_?.Invoke();
        SoundManager.Instance.Play("BirdSound");
    }
    private IEnumerator ResetTargetMaskCoroutine()
    {
        int count = 0;
        string type = "reset";
        IsPlayerInputKeyLock(true);
        SetLoadScreen(type, count, meshTargetList_.Count);
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.IsDrawable() && target_.IsClear() == false && target_.GetProcessPercent() > 0.0001f)
            {
                if (target_.ResetMask() == false)
                {
                    Debug.LogWarning("�Ϻ� ����� Mask�� �ʱ�ȭ�ϴµ� �����Ͽ����ϴ�.");
                }
                SetLoadScreen(type, count, meshTargetList_.Count);
            }
            yield return null;
        }
        loadingEndCallback_?.Invoke();
        IsPlayerInputKeyLock(false);
    }
}
