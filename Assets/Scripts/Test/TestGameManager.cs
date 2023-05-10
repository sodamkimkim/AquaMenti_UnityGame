using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    [SerializeField]
    private UI_Manager uI_Manager_ = null;
    [SerializeField]
    private MeshPaintManager meshPaintManager_ = null;

    private List<string> missFileList_ = null;
    //private int mapNum_ = 0;
    //private int sectionNum_ = 0;


    private void Awake()
    {
        // 1) Save ��θ� �����մϴ�. (�̹� �ִٸ� ������ ���� �ʽ��ϴ�. - ���ǹ� �����̱� ����)
        FilePath.Init();

        // 2) Resources ����� ���ϰ� ���Ͽ� Save ����� ������ ���� �Ǿ����� Ȯ���� �մϴ�.
        // 2-1) �ϳ��� ������ �Ǿ��ٸ� false�� ��ȯ�ϰ� List�� ����ϴ�.
        bool isOk = FileIO.CheckFileState(out missFileList_, "*.png");
        // 3) �����Ǿ��ٸ�
        if (isOk == false)
        {
            // 3-1)�սǵ� ������ �����մϴ�.
            foreach (string file in missFileList_)
            {
                string[] split = file.Split("/");
                string fileName = split[split.Length - 1];
                split[split.Length - 1] = null; // FileName�� ��ο� ���Խ�Ű�� �ʱ� ���� null ó���մϴ�.
                string dir = string.Join("/", split); // Split�� ���ڿ����� �ٽ� ���ս�ŵ�ϴ�.

#if UNITY_EDITOR
                //Debug.Log("[TestLog] dir: " + dir);
#endif
                // 3-2) Resources ��ηκ��� Save��η� ������ �����մϴ�.
                FileIO.CopyFile(
                    FilePath.ASSETS_MAP_PATH + dir,
                    FilePath.SAVE_PATH + dir,
                    fileName);
            }
        }
    }

    private void Start()
    {
#if UNITY_EDITOR
        // ������ ���ϵ� Ȯ�ο�
        PrintList(missFileList_);
#endif

        // 4) �����ǰų� �սǵ� ������ �����Ͽ��ٸ� ���� �غ� ���� ���·� �Ǵ��մϴ�.
        // Scene�� �ִ� ��� MeshPaintTarget�� ���� Object�� ���� Mask Texture�� �ҷ��ɴϴ�.
        meshPaintManager_.Init(uI_Manager_.LoadingStart, uI_Manager_.LoadingEnd);
    }


    private void PrintList(List<string> _list)
    {
        for(int i = 0; i < _list.Count; ++i)
        {
            Debug.Log("[TestLog] " + i + "- Missing File Name: " + _list[i]);
        }
    }
}
