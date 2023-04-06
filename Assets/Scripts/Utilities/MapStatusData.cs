using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataForSaveLoad
{
    /// <summary>
    /// : �� �� WorkSection, Parts �� ���� ȹ�� Income, �۾� ���� �� �����ϴ� Ŭ����
    ///  - static
    ///  - �������� ���� �ϳ��� List�� �����Ѵ�. (Swap�ذ��� ���)
    /// </summary>
    public static class MapStatusData
    {
        private static List<WorkSectionStatusData> workstationList_ = new List<WorkSectionStatusData>();
    } // end of class
    public class WorkSectionStatusData
    {
        // # worksection�� Income����
        private float currentSectionIncome_;
        private float totalSectionIncome_;

        // # worksection�� ��������
        private float currentSectionStar_;
        private float totalSectionStar_;

        // # �Ҽӵ� part����
        private List<PartStatusData> partList_ = new List<PartStatusData>();
    } // end of class
    public struct PartStatusData
    {
        private string workSectionBelong_; // part�� �Ҽӵ� worksection
      //  private string partCategory_; // ex) �˺����� , �˺� -> �̰� ���� ���� & ui���� ������ �� ó��
        private string partName_; // ex) �˺�����1, �˺�����2, �˺�
    } // end of structure

} // end of namespace