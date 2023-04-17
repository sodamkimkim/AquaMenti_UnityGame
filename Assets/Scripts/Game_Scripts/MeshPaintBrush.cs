using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPaintBrush : MonoBehaviour
{
    private Collider prevCollider_;
    private Vector2 uvPos_ = Vector2.zero;

    private MeshPaintTarget target_ = null;

    [SerializeField, Range(0.5f, 10f)]
    private float effectiveDistance_ = 1f; // ��ȿ ��Ÿ�
    [SerializeField, Range(1f, 16f)]
    private float maxDistance_ = 2f; // �ִ� ��Ÿ�
    private bool effective_ = false;

    // Option
    [SerializeField, Range(1, 20)]
    private int size_ = 10;
    private Vector4 color_ = new Vector4(0f, 0f, 0f, 1f);
    private bool isPainting_ = false;

    private float drawTiming_ = 0.01f;
    private float waitTime_ = 0.1f;
    private float saveTiming_ = 1f;

    private bool drawCoroutine_ = false;
    private bool runCoroutine_ = false;
    private bool saveCoroutine_ = false;

    private Ray ray_;

    // �ӽ�
    public enum EMagicType { Zero, One, Two }

    public struct DirtyLv // ����Ÿ��
    {
        public int rLv { get; set; } // ǥ��
        public int gLv { get; set; } // �ڵ���
        public int bLv { get; set; } // ���
    }

    public struct Stick
    {
        public DirtyLv cleanLv;
        public EMagicType magicType; // ���� => Nozzle
    }

    // �ӽ�
    // R: ǥ��, G: �ڵ���, B: ���
    //private int rDirtyLv = 5;
    //private int rCleanLv = 3;
    //private int magicType = 2; // Nozzle. Maybe Enum.
    public Stick stick;
    private DirtyLv dirty;

    //public void setStartPos(Vector3 _pos)
    //{
    //    startPos_ = _pos;
    //    this.gameObject.transform.localPosition = startPos_;
    //}
    //public void SetEndPos(Vector3 _pos)
    //{
    //    endPos_ = _pos;
    //}
    private void Start()
    {
        // ������ ���� ����
        stick = new Stick();
        DirtyLv cleanLv = new DirtyLv();
        cleanLv.rLv = 5;
        cleanLv.gLv = 3;
        cleanLv.bLv = 1;
        stick.cleanLv = cleanLv;
        stick.magicType = 0;

        // ��� ������ ����
        dirty = new DirtyLv();
        dirty.rLv = 10;
        dirty.gLv = 0;
        dirty.bLv = 1;
    }
    // End �ӽ� //
    public bool IsPainting()
    {
        return isPainting_;
    }
    public void IsPainting(bool _do)
    {
        isPainting_ = _do;
    }
    public MeshPaintTarget GetTarget()
    {
        if (target_ != null)
        {
            return target_;
        }
        else return null;
    }

    // �ӽ� ��Ī
    public void PaintToTarget(Ray _ray)
    {
#if UNITY_EDITOR
        //Debug.Log("Try");

#endif
        if (Physics.Raycast(_ray, out var hitInfo))
        {
#if UNITY_EDITOR
            //Debug.Log("In Raycast");
#endif
            if (prevCollider_ != hitInfo.collider)
            {
                prevCollider_ = hitInfo.collider;
                hitInfo.collider.TryGetComponent<MeshPaintTarget>(out target_);
            }
#if UNITY_EDITOR
            //Debug.Log("target? " + target_);
#endif
            if (target_ != null &&
                target_.IsDrawable() == true)
            {
#if UNITY_EDITOR
                //Debug.LogFormat("uvPos_: {0} | coord: {1}", uvPos_, hitInfo.textureCoord);
#endif
                if (uvPos_ != hitInfo.textureCoord)
                {
#if UNITY_EDITOR
                    //Debug.Log("Coord is not Same.");
#endif
                    uvPos_ = hitInfo.textureCoord;
                    if (maxDistance_ >= hitInfo.distance)
                        effective_ = true;
                    else if (effectiveDistance_ >= hitInfo.distance)
                        effective_ = true;
                    else
                        effective_ = false;

#if UNITY_EDITOR
                    //Debug.Log("Before Draw" + effective_);
#endif
                    // ��ȿ�� ��Ÿ���� DrawRender�� ����
                    if (effective_)
                    {
                        // target���� Drawó���ϰ� �ϴµ�
                        // uvPos�� Paint�� Color, Size, Distance, Drawable�� �ѱ� (Compute Shader���� ó���� �κ�)

                        // ��ȿ��Ÿ� ���� �ִٸ� ȿ�� 1, �ִ��Ÿ��� �����Ѵٸ� ȿ�� 1 ~ 0
                        float distance;
                        if (hitInfo.distance <= effectiveDistance_)
                            distance = 1f;
                        else
                            distance = 1 - ((hitInfo.distance - effectiveDistance_) / (maxDistance_ - effectiveDistance_));

                        // ��ô��
                        color_ = WashPower();
#if UNITY_EDITOR
                        //Debug.LogFormat("r: {0}, g: {1}, b: {2}", color_.x, color_.y, color_.z);
#endif
                        if (target_.IsClear() == false)
                            target_.DrawRender(isPainting_, uvPos_, color_, size_, distance);
                        target_.DrawWet(isPainting_, uvPos_, size_, distance);
                        CheckTargetProcess();
                        SaveTargetProcess();
                    }
                }
            }
        }
    }


    // ���� ��ô�� ����
    private Vector4 WashPower()
    {
        DirtyLv cleanLv = stick.cleanLv;

        float rPow = CalculatePower(cleanLv.rLv, dirty.rLv, stick.magicType);
        float gPow = CalculatePower(cleanLv.gLv, dirty.gLv, stick.magicType);
        float bPow = CalculatePower(cleanLv.bLv, dirty.bLv, stick.magicType);

        return new Vector4(rPow, gPow, bPow, 1);
    }

    // ��ô�� ���
    private float CalculatePower(float _cleanLv, float _dirtyLv, EMagicType _type)
    {
        float magicPw = GetMagicPower(_type);

        float cleanLvPw = (float)_cleanLv / _dirtyLv; // Ÿ���� ���� �Ϳ� ����

        float pw = cleanLvPw * magicPw; // �⺻: ������Pw(��ô��) * ����Pw(����)

        return pw;
    }

    // ���� ���翡 ���� ����ġ
    private float GetMagicPower(EMagicType _type)
    {
        switch ((int)_type)
        {
            default:
                return 1f;
            case 1: // 15��
                return 0.85f;
            case 2: // 25��
                return 0.7f;
            case 3: // 45��
                return 0.45f;
            case 4: // ��ô��
                return 1f;
        }
    }


    #region Paint Coroutine
    /// <summary>
    /// ///////////////////////////////
    /// </summary>
    /// <param name="_ray"></param>
    public void TimingDraw(Ray _ray)
    {

        ray_ = _ray;
        if (drawCoroutine_ == false)
        {
            drawCoroutine_ = true;
            IsPainting(true);

            StartCoroutine("TimingDrawCoroutine");
        }
    }
    public void SetRayDirection(Vector3 _dir)
    {
        ray_.direction = _dir;
    }
    public void StopTimingDraw()
    {
        if (drawCoroutine_ == true)
        {
            drawCoroutine_ = false;
            IsPainting(false);
            StopCoroutine("TimingDrawCoroutine");
        }
    }

    private void CheckTargetProcess()
    {
        if (runCoroutine_ == false)
        {
            runCoroutine_ = true;
            if (target_ != null && target_.IsDrawable() && target_.IsClear() == false)
                StartCoroutine("CheckTargetProcessCoroutine");
        }
    }
    public void StopCheckTargetProcess()
    {
        if (runCoroutine_ == true)
        {
            runCoroutine_ = false;
            StopCoroutine("CheckTargetProcessCoroutine");
        }
    }

    private void SaveTargetProcess()
    {
        if (saveCoroutine_ == false)
        {
            saveCoroutine_ = true;
            if (target_ != null && target_.IsDrawable() && target_.IsClear() == false)
                StartCoroutine("SaveTargetProcessCoroutine");
        }
    }
    public void StopSaveTargetProcess()
    {
        if (saveCoroutine_ == true)
        {
            saveCoroutine_ = false;
            StopCoroutine("SaveTargetProcessCoroutine");
        }
    }


    // Brush
    private IEnumerator TimingDrawCoroutine()
    {
        while (true)
        {
            // _ray�� �ٲ��ֱ�
            Ray Ray = ray_;


            Ray.origin = this.transform.position;

            //Ray.direction = _direction;
            PaintToTarget(Ray);
            // Debug.Log("in");
            Debug.DrawRay(Ray.origin, Ray.direction * effectiveDistance_, Color.green);
            yield return new WaitForSeconds(drawTiming_);
        }
    }

    private IEnumerator CheckTargetProcessCoroutine()
    {
#if UNITY_EDITOR
        //Debug.Log("[CheckTargetProcessCoroutine]");
#endif
        while (true)
        {
            if (target_ != null && target_.IsDrawable() && target_.IsClear())
                StopCheckTargetProcess();
            else if (target_ != null && target_.IsDrawable())
                target_.CheckAutoClear();
            yield return new WaitForSeconds(waitTime_);
        }
    }

    private IEnumerator SaveTargetProcessCoroutine()
    {
#if UNITY_EDITOR
        Debug.Log("[SaveTargetProcessCoroutine]");
#endif
        while (true)
        {
            // Ŭ���� ���°� �Ǿ��ٸ� ������ ���൵�� ���� �� Coroutine�� ����ϴ�.
            if (target_ != null && target_.IsDrawable() && target_.IsClear())
            {
                target_.SaveMask();
                StopSaveTargetProcess();
            }
            else if (target_ != null && target_.IsDrawable() && target_.IsClear() == false)
                target_.SaveMask();
            yield return new WaitForSeconds(saveTiming_);
        }
    }
    #endregion
}
