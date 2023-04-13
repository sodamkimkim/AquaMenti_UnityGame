using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPaintManager : MonoBehaviour
{
    // DirtyMask�� �����Ͽ� �����ϴ� �뵵. DirtyMask�� ���� �� ��������� ����.
    private List<MeshPaintTarget> meshTargetList_ = null; // InGame MeshPaintTarget List
    private bool saveChecker_ = false;

    private void Awake()
    {
        MeshPaintTarget[] targets = FindObjectsOfType<MeshPaintTarget>();
        meshTargetList_ = new List<MeshPaintTarget>(targets);
    }
    private void Start()
    {
        //LoadTargetMask(0, 0);
    }

    private void LateUpdate()
    {
        if (saveChecker_ == false)
        {
            saveChecker_ = true;
            SaveTargetMask(0, 0);
        }
    }


    public void CopyInitFiles()
    {
        string sourceDir = FilePath.RESOURCES_MAP_PATH;
        string destinationDir = FilePath.SAVE_PATH;

        FilePath.CopyDirectory(sourceDir, destinationDir, "*.png");
    }

    public void SaveTargetMask(int _mapNum, int _sectionNum)
    {
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.IsDrawable())
            {
                string path = FilePath.GetPath(
                    FilePath.EPathType.RESOURCES,
                    (FilePath.EMapType)_mapNum,
                    (FilePath.ESection)_sectionNum
                    );

                target_.SaveMask(path);
            }
        }
    }

    public void LoadTargetMask(int _mapNum, int _sectionNum)
    {
        foreach (MeshPaintTarget target_ in meshTargetList_)
        {
            if (target_.IsDrawable())
            {
                string path = FilePath.GetPath(
                FilePath.EPathType.EXTERNAL,
                (FilePath.EMapType)_mapNum,
                (FilePath.ESection)_sectionNum
                );

                target_.LoadMask(path);
            }
        }
    }

}
