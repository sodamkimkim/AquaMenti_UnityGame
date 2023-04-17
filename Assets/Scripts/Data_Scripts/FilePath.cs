using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilePath
{
    public enum EPathType { EXTERNAL, ASSETS, LEN }
    public enum EMapType { MAP_1, MAP_2, LEN }
    public enum ESection { SECTION_1, SECTION_2, SECTION_3, SECTION_4, SECTION_5, LEN }

    // PersistentDataPath //
    public static readonly string EXTERNAL_PATH = Application.persistentDataPath;

    public static readonly string SAVE_PATH = Application.persistentDataPath + "/Saves";

    public static readonly string SAVE_MAP_1_PATH = Application.persistentDataPath + "/Saves/Map_1";
    public static readonly string SAVE_MAP_1_SECTION_1_PATH = Application.persistentDataPath + "/Saves/Map_1/Section_1";
    public static readonly string SAVE_MAP_1_SECTION_2_PATH = Application.persistentDataPath + "/Saves/Map_1/Section_2";
    public static readonly string SAVE_MAP_1_SECTION_3_PATH = Application.persistentDataPath + "/Saves/Map_1/Section_3";
    public static readonly string SAVE_MAP_1_SECTION_4_PATH = Application.persistentDataPath + "/Saves/Map_1/Section_4";
    public static readonly string SAVE_MAP_1_SECTION_5_PATH = Application.persistentDataPath + "/Saves/Map_1/Section_5";
    
    public static readonly string SAVE_MAP_2_PATH = Application.persistentDataPath + "/Saves/Map_2";
    public static readonly string SAVE_MAP_2_SECTION_1_PATH = Application.persistentDataPath + "/Saves/Map_2/Section_1";
    public static readonly string SAVE_MAP_2_SECTION_2_PATH = Application.persistentDataPath + "/Saves/Map_2/Section_2";
    public static readonly string SAVE_MAP_2_SECTION_3_PATH = Application.persistentDataPath + "/Saves/Map_2/Section_3";
    //

    public static readonly string RESOURCES_PATH = Application.dataPath + "/Resources";

    // StreamingAssetsPath //
    public static readonly string ASSETS_PATH = Application.streamingAssetsPath;

    public static readonly string ASSETS_MAP_PATH = Application.streamingAssetsPath + "/Maps";

    public static readonly string ASSETS_MAP_1_PATH = Application.streamingAssetsPath + "/Maps/Map_1";
    public static readonly string ASSETS_MAP_1_SECTION_1_PATH = Application.streamingAssetsPath + "/Maps/Map_1/Section_1";
    public static readonly string ASSETS_MAP_1_SECTION_2_PATH = Application.streamingAssetsPath + "/Maps/Map_1/Section_2";
    public static readonly string ASSETS_MAP_1_SECTION_3_PATH = Application.streamingAssetsPath + "/Maps/Map_1/Section_3";
    public static readonly string ASSETS_MAP_1_SECTION_4_PATH = Application.streamingAssetsPath + "/Maps/Map_1/Section_4";
    public static readonly string ASSETS_MAP_1_SECTION_5_PATH = Application.streamingAssetsPath + "/Maps/Map_1/Section_5";

    public static readonly string ASSETS_MAP_2_PATH = Application.streamingAssetsPath + "/Maps/Map_2";
    public static readonly string ASSETS_MAP_2_SECTION_1_PATH = Application.streamingAssetsPath + "/Maps/Map_2/Section_1";
    public static readonly string ASSETS_MAP_2_SECTION_2_PATH = Application.streamingAssetsPath + "/Maps/Map_2/Section_2";
    public static readonly string ASSETS_MAP_2_SECTION_3_PATH = Application.streamingAssetsPath + "/Maps/Map_2/Section_3";
    //

    public static readonly string SAVE_PATH_NAME = "Saves";
    public static readonly string MAP_PATH_NAME = "Maps";

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

        CreateDirectory(SAVE_MAP_1_PATH);
        CreateDirectory(SAVE_MAP_1_SECTION_1_PATH);
        CreateDirectory(SAVE_MAP_1_SECTION_2_PATH);
        CreateDirectory(SAVE_MAP_1_SECTION_3_PATH);
        CreateDirectory(SAVE_MAP_1_SECTION_4_PATH);
        CreateDirectory(SAVE_MAP_1_SECTION_5_PATH);

        CreateDirectory(SAVE_MAP_2_PATH);
        CreateDirectory(SAVE_MAP_2_SECTION_1_PATH);
        CreateDirectory(SAVE_MAP_2_SECTION_2_PATH);
        CreateDirectory(SAVE_MAP_2_SECTION_3_PATH);
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
            case EPathType.ASSETS:
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
                    return SAVE_MAP_1_PATH;
                case EMapType.MAP_2:
                    return SAVE_MAP_2_PATH;
            }
        }
        else if (_pathType == EPathType.ASSETS)
        {
            switch (_mapType)
            {
                case EMapType.MAP_1:
                    return ASSETS_MAP_1_PATH;
                case EMapType.MAP_2:
                    return ASSETS_MAP_2_PATH;
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
                        return SAVE_MAP_1_SECTION_1_PATH;
                    case ESection.SECTION_2:
                        return SAVE_MAP_1_SECTION_2_PATH;
                    case ESection.SECTION_3:
                        return SAVE_MAP_1_SECTION_3_PATH;
                    case ESection.SECTION_4:
                        return SAVE_MAP_1_SECTION_4_PATH;
                    case ESection.SECTION_5:
                        return SAVE_MAP_1_SECTION_5_PATH;
                }
            }
            else if (_mapType == EMapType.MAP_2)
            {
                switch (_sectionType)
                {
                    case ESection.SECTION_1:
                        return SAVE_MAP_2_SECTION_1_PATH;
                    case ESection.SECTION_2:
                        return SAVE_MAP_2_SECTION_2_PATH;
                    case ESection.SECTION_3:
                        return SAVE_MAP_2_SECTION_3_PATH;
                }
            }
        }
        else if (_pathType == EPathType.ASSETS)
        {
            if (_mapType == EMapType.MAP_1)
            {
                switch (_sectionType)
                {
                    case ESection.SECTION_1:
                        return ASSETS_MAP_1_SECTION_1_PATH;
                    case ESection.SECTION_2:
                        return ASSETS_MAP_1_SECTION_2_PATH;
                    case ESection.SECTION_3:
                        return ASSETS_MAP_1_SECTION_3_PATH;
                    case ESection.SECTION_4:
                        return ASSETS_MAP_1_SECTION_4_PATH;
                    case ESection.SECTION_5:
                        return ASSETS_MAP_1_SECTION_5_PATH;
                }
            }
            else if (_mapType == EMapType.MAP_2)
            {
                switch (_sectionType)
                {
                    case ESection.SECTION_1:
                        return ASSETS_MAP_2_SECTION_1_PATH;
                    case ESection.SECTION_2:
                        return ASSETS_MAP_2_SECTION_2_PATH;
                    case ESection.SECTION_3:
                        return ASSETS_MAP_2_SECTION_3_PATH;
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
