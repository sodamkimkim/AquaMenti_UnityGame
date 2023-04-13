using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilePath
{
    public enum EPathType { EXTERNAL, RESOURCES, LEN }
    public enum EMapType { MAP_1, MAP_2, LEN }
    public enum ESection { SECTION_1, SECTION_2, SECTION_3, SECTION_4, SECTION_5, LEN }

    // PersistentDataPath //
    public static readonly string EXTERNAL_PATH = Application.persistentDataPath;

    public static readonly string SAVE_PATH = Application.persistentDataPath + "/Saves";

    public static readonly string MAP_1_PATH = Application.persistentDataPath + "/Saves/Map_1";
    public static readonly string MAP_1_SECTION_1_PATH = Application.persistentDataPath + "/Saves/Map_1/Section_1";
    public static readonly string MAP_1_SECTION_2_PATH = Application.persistentDataPath + "/Saves/Map_1/Section_2";
    public static readonly string MAP_1_SECTION_3_PATH = Application.persistentDataPath + "/Saves/Map_1/Section_3";
    public static readonly string MAP_1_SECTION_4_PATH = Application.persistentDataPath + "/Saves/Map_1/Section_4";
    public static readonly string MAP_1_SECTION_5_PATH = Application.persistentDataPath + "/Saves/Map_1/Section_5";
    
    public static readonly string MAP_2_PATH = Application.persistentDataPath + "/Saves/Map_2";
    public static readonly string MAP_2_SECTION_1_PATH = Application.persistentDataPath + "/Saves/Map_2/Section_1";
    public static readonly string MAP_2_SECTION_2_PATH = Application.persistentDataPath + "/Saves/Map_2/Section_2";
    public static readonly string MAP_2_SECTION_3_PATH = Application.persistentDataPath + "/Saves/Map_2/Section_3";
    //

    // DataPath //
    public static readonly string DATA_PATH = Application.dataPath; // Assets

    public static readonly string RESOURCES_PATH = Application.dataPath + "/Resources";
    public static readonly string RESOURCES_MAP_PATH = Application.dataPath + "/Resources/Maps";

    public static readonly string RESOURCES_MAP_1_PATH = Application.dataPath + "/Resources/Maps/Map_1";
    public static readonly string RESOURCES_MAP_1_SECTION_1_PATH = Application.persistentDataPath + "/Resources/Maps/Map_1/Section_1";
    public static readonly string RESOURCES_MAP_1_SECTION_2_PATH = Application.persistentDataPath + "/Resources/Maps/Map_1/Section_2";
    public static readonly string RESOURCES_MAP_1_SECTION_3_PATH = Application.persistentDataPath + "/Resources/Maps/Map_1/Section_3";
    public static readonly string RESOURCES_MAP_1_SECTION_4_PATH = Application.persistentDataPath + "/Resources/Maps/Map_1/Section_4";
    public static readonly string RESOURCES_MAP_1_SECTION_5_PATH = Application.persistentDataPath + "/Resources/Maps/Map_1/Section_5";

    public static readonly string RESOURCES_MAP_2_PATH = Application.dataPath + "/Resources/Maps/Map_2";
    public static readonly string RESOURCES_MAP_2_SECTION_1_PATH = Application.persistentDataPath + "/Resources/Maps/Map_2/Section_1";
    public static readonly string RESOURCES_MAP_2_SECTION_2_PATH = Application.persistentDataPath + "/Resources/Maps/Map_2/Section_2";
    public static readonly string RESOURCES_MAP_2_SECTION_3_PATH = Application.persistentDataPath + "/Resources/Maps/Map_2/Section_3";
    //

    public static readonly string SAVE_PATH_NAME = "Saves";
    public static readonly string RESOURCES_MAP_PATH_NAME = "Maps";

    public static readonly string MAP_1_PATH_NAME = "Map_1";
    public static readonly string MAP_2_PATH_NAME = "Map_2";
    public static readonly string SECTION_1_PATH_NAME = "Section_1";
    public static readonly string SECTION_2_PATH_NAME = "Section_2";
    public static readonly string SECTION_3_PATH_NAME = "Section_3";
    public static readonly string SECTION_4_PATH_NAME = "Section_4";
    public static readonly string SECTION_5_PATH_NAME = "Section_5";


    /// <summary>
    /// �⺻ Directory ��� ����
    /// </summary>
    public static void Init()
    {
        // External ������ �⺻ Directory ����
        CreateDirectory(SAVE_PATH);

        CreateDirectory(MAP_1_PATH);
        CreateDirectory(MAP_1_SECTION_1_PATH);
        CreateDirectory(MAP_1_SECTION_2_PATH);
        CreateDirectory(MAP_1_SECTION_3_PATH);
        CreateDirectory(MAP_1_SECTION_4_PATH);
        CreateDirectory(MAP_1_SECTION_5_PATH);

        CreateDirectory(MAP_2_PATH);
        CreateDirectory(MAP_2_SECTION_1_PATH);
        CreateDirectory(MAP_2_SECTION_2_PATH);
    }


    /// <summary>
    /// <paramref name="_sourceDir"/>�� �ִ� <paramref name="_fileType"/>������ ���ϵ��� <paramref name="_destinationDir"/>�� �����մϴ�.<br/>
    /// <paramref name="_recursive"/>�� ���� ���� Directory���� ������ ������ ���� �� �ֽ��ϴ�.
    /// </summary>
    /// <param name="_sourceDir">������ Directory</param>
    /// <param name="_destinationDir">����� Directory</param>
    /// <param name="_fileType">�����ϰ��� �ϴ� �������� | default: "*"</param>
    /// <param name="_recursive">���� Directory���� ������ ������ ���� | default: false</param>
    public static void CopyDirectory(string _sourceDir, string _destinationDir, string _fileType = "*", bool _recursive = false)
    {
        if (Directory.Exists(_destinationDir) == false)
            CreateDirectory(_destinationDir);

        var dir = new DirectoryInfo(_sourceDir);

        foreach (FileInfo file in dir.GetFiles(_fileType))
        {
            string path = Path.Combine(_destinationDir, file.Name);
            file.CopyTo(path);
        }

        if (_recursive)
        {
            DirectoryInfo[] dirs = dir.GetDirectories(_sourceDir);
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(_destinationDir, subDir.Name);
                CopyDirectory(_destinationDir, newDestinationDir, _fileType, true);
            }
        }
    }

    /// <summary>
    /// External ��ο� Resources ��θ� Ȯ���Ͽ� �սǵ� ���� ���ٸ� true�� ��ȯ�ϰ� �սǵ� ���� �ִٸ� false�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="_missList">�սǵ� Directory�� Name�� ����ϴ�.</param>
    /// <returns>true: ��� ������ ���� | false: �սǵ� ������ ����</returns>
    public static bool CheckFileState(out List<string> _missList)
    {
        bool checker = true;
        // System.Linq ���
        var externalFiles = from file in Directory.EnumerateFiles(SAVE_PATH, "*", SearchOption.AllDirectories) select file;
        var resourceFiles = from file in Directory.EnumerateFiles(RESOURCES_MAP_PATH, "*.png", SearchOption.AllDirectories) select file;

        List<string> missDirList = new List<string>();
        int i = 0;
        foreach (var file in resourceFiles)
        {
            if (externalFiles.Any(f => f.Contains(file.ToString()) == false))
            {
#if UNITY_EDITOR
                Debug.LogFormat("[FilePath] {0}- Missing File Name: {1}", i, file.ToString());
#endif
                missDirList.Add(file.ToString());
                checker = false;
            }
            ++i;
        }

        _missList = missDirList;


        return checker;
    }


    /// <summary>
    /// �ش��ϴ� ��θ� ��ȯ�մϴ�. �ش��ϴ� ���� ���ٸ� string.Empty�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="_pathType"></param>
    /// <returns>�ش� ���� ���ٸ� string.Empty ��ȯ</returns>
    public static string GetPath(EPathType _pathType)
    {
        switch (_pathType)
        {
            case EPathType.EXTERNAL:
                return EXTERNAL_PATH;
            case EPathType.RESOURCES:
                return RESOURCES_PATH;
        }

        return string.Empty;
    }
    /// <summary>
    /// �ش��ϴ� ��θ� ��ȯ�մϴ�. �ش��ϴ� ���� ���ٸ� string.Empty�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="_pathType"></param>
    /// <param name="_mapType"></param>
    /// <returns>�ش� ���� ���ٸ� string.Empty ��ȯ</returns>
    public static string GetPath(EPathType _pathType, EMapType _mapType)
    {
        if (_pathType == EPathType.EXTERNAL)
        {
            switch (_mapType)
            {
                case EMapType.MAP_1:
                    return MAP_1_PATH;
                case EMapType.MAP_2:
                    return MAP_2_PATH;
            }
        }
        else if (_pathType == EPathType.RESOURCES)
        {
            switch (_mapType)
            {
                case EMapType.MAP_1:
                    return RESOURCES_MAP_1_PATH;
                case EMapType.MAP_2:
                    return RESOURCES_MAP_2_PATH;
            }
        }

        return string.Empty;
    }
    /// <summary>
    /// �ش��ϴ� ��θ� ��ȯ�մϴ�. �ش��ϴ� ���� ���ٸ� string.Empty�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="_pathType"></param>
    /// <param name="_mapType"></param>
    /// <param name="_sectionType"></param>
    /// <returns>�ش� ���� ���ٸ� string.Empty ��ȯ</returns>
    public static string GetPath(EPathType _pathType, EMapType _mapType, ESection _sectionType)
    {
        if (_pathType == EPathType.EXTERNAL)
        {
            if (_mapType == EMapType.MAP_1)
            {
                switch (_sectionType)
                {
                    case ESection.SECTION_1:
                        return MAP_1_SECTION_1_PATH;
                    case ESection.SECTION_2:
                        return MAP_1_SECTION_2_PATH;
                    case ESection.SECTION_3:
                        return MAP_1_SECTION_3_PATH;
                    case ESection.SECTION_4:
                        return MAP_1_SECTION_4_PATH;
                    case ESection.SECTION_5:
                        return MAP_1_SECTION_5_PATH;
                }
            }
            else if (_mapType == EMapType.MAP_2)
            {
                switch (_sectionType)
                {
                    case ESection.SECTION_1:
                        return MAP_2_SECTION_1_PATH;
                    case ESection.SECTION_2:
                        return MAP_2_SECTION_2_PATH;
                    case ESection.SECTION_3:
                        return MAP_2_SECTION_3_PATH;
                }
            }
        }
        else if (_pathType == EPathType.RESOURCES)
        {
            if (_mapType == EMapType.MAP_1)
            {
                switch (_sectionType)
                {
                    case ESection.SECTION_1:
                        return RESOURCES_MAP_1_SECTION_1_PATH;
                    case ESection.SECTION_2:
                        return RESOURCES_MAP_1_SECTION_2_PATH;
                    case ESection.SECTION_3:
                        return RESOURCES_MAP_1_SECTION_3_PATH;
                    case ESection.SECTION_4:
                        return RESOURCES_MAP_1_SECTION_4_PATH;
                    case ESection.SECTION_5:
                        return RESOURCES_MAP_1_SECTION_5_PATH;
                }
            }
            else if (_mapType == EMapType.MAP_2)
            {
                switch (_sectionType)
                {
                    case ESection.SECTION_1:
                        return RESOURCES_MAP_2_SECTION_1_PATH;
                    case ESection.SECTION_2:
                        return RESOURCES_MAP_2_SECTION_2_PATH;
                    case ESection.SECTION_3:
                        return RESOURCES_MAP_2_SECTION_3_PATH;
                }
            }
        }

        return string.Empty;
    }

    private static void CreateDirectory(string _path)
    {
        if (Directory.Exists(_path) == false)
            Directory.CreateDirectory(_path);
    }
}
