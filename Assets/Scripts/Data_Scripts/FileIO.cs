using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

public class FileIO
{
    /// <summary>
    /// External ��ο� Resources ��θ� Ȯ���Ͽ� �սǵ� ���� ���ٸ� true�� ��ȯ�ϰ� �սǵ� ���� �ִٸ� false�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="_missList">�սǵ� Directory�� Name�� ����ϴ�.</param>
    /// <returns>true: ��� ������ ���� | false: �սǵ� ������ ����</returns>
    public static bool CheckFileState(out List<string> _missList, string _fileType = "*")
    {
        bool checker = true;

        // System.Linq ���
        DirectoryInfo externalDir = new DirectoryInfo(FilePath.SAVE_PATH);
        var externalFiles = from file in externalDir.GetFiles(_fileType, SearchOption.AllDirectories) select file;
        DirectoryInfo resourceDir = new DirectoryInfo(FilePath.ASSETS_MAP_PATH);
        var resourceFiles = from file in resourceDir.GetFiles(_fileType, SearchOption.AllDirectories) select file;

        List<string> missDirList = new List<string>();
        int i = 0;
        foreach (FileInfo file in resourceFiles)
        {
            if (externalFiles.Any(f => f.Name == file.Name) == false)
            {
                // ��� ���� ���ڸ� '\'���� '/'���� ���Ͻ�ŵ�ϴ�.
                string missFilePath = Regex.Replace(file.FullName, @"[\\]", "/").Replace(FilePath.ASSETS_MAP_PATH, "");
#if UNITY_EDITOR
                //Debug.LogFormat("[FilePath] {0}- Missing File Name: {1}, path: {2}", i, file.Name, missFilePath);
#endif
                missDirList.Add(missFilePath);
                checker = false;
            }
            ++i;
        }

        _missList = missDirList;


        return checker;
    }

    /// <summary>
    /// <paramref name="_dir"/>�� �ִ� <paramref name="_fileName"/> ������ Binary�� �����ɴϴ�.
    /// </summary>
    /// <param name="_dir">������ Directory</param>
    /// <param name="_fileName">������ ������ Name (Ȯ���ڸ� ����)</param>
    /// <returns></returns>
    public static byte[] GetFileBinary(string _dir, string _fileName)
    {
        if (Directory.Exists(_dir) == false) return null;

        byte[] bytes = null;
        string path = Path.Combine(_dir, _fileName);

        if(File.Exists(path))
            bytes = File.ReadAllBytes(path);

        return bytes;
    }

    /// <summary>
    /// <paramref name="_sourceDir"/>�� �ִ� <paramref name="_fileName"/> ������ <paramref name="_destinationDir"/>�� �����մϴ�.<br/>
    /// <paramref name="_overwrite"/>�� ���� ������ ����� ������ ���� �� �ֽ��ϴ�.
    /// </summary>
    /// <param name="_sourceDir"></param>
    /// <param name="_destinationDir"></param>
    /// <param name="_fileName"></param>
    /// <param name="_overwrite"></param>
    public static void CopyFile(string _sourceDir, string _destinationDir, string _fileName, bool _overwrite = false)
    {
        if (Directory.Exists(_sourceDir) == false) return;

        if (Directory.Exists(_destinationDir) == false)
            Directory.CreateDirectory(_destinationDir);

        var dir = new DirectoryInfo(_sourceDir);
        
        foreach (FileInfo file in dir.GetFiles(_fileName))
        {
            if (file.Exists && file.Extension != ".meta")
            {
                string path = Path.Combine(_destinationDir, file.Name);
                file.CopyTo(path, _overwrite);
            }
        }
    }

    /// <summary>
    /// <paramref name="_sourceDir"/>�� �ִ� <paramref name="_fileType"/>������ ���ϵ��� <paramref name="_destinationDir"/>�� �����մϴ�.<br/>
    /// <paramref name="_recursive"/>�� ���� ���� Directory���� ������ ������ ���� �� �ֽ��ϴ�.<br/>
    /// <paramref name="_overwrite"/>�� ���� ������ ����� ������ ���� �� �ֽ��ϴ�.<br/>
    /// </summary>
    /// <param name="_sourceDir">������ Directory</param>
    /// <param name="_destinationDir">����� Directory</param>
    /// <param name="_fileType">�����ϰ��� �ϴ� �������� | default: "*"</param>
    /// <param name="_recursive">���� Directory���� ������ ������ ���� | default: false</param>
    /// <param name="_overwrite">����⸦ �� ������ ���� | default: false</param>
    public static void CopyDirectory(string _sourceDir, string _destinationDir, string _fileType = "*", bool _recursive = false, bool _overwrite = false)
    {
        if (Directory.Exists(_sourceDir) == false) return;

        if (Directory.Exists(_destinationDir) == false)
            Directory.CreateDirectory(_destinationDir);

        //Debug.LogFormat("sour: {0}, dest: {1}, type: {2}", _sourceDir, _destinationDir, _fileType);

        DirectoryInfo dir = new DirectoryInfo(_sourceDir);

        foreach (FileInfo file in dir.GetFiles(_fileType))
        {
            string path = Path.Combine(_destinationDir, file.Name);
            file.CopyTo(path, _overwrite);
        }

        if (_recursive)
        {
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(_destinationDir, subDir.Name);
                //Debug.Log("dir: " + newDestinationDir);
                CopyDirectory(_destinationDir, newDestinationDir, _fileType, true);
            }
        }
    }
}
