using System;
using System.Collections.Generic;
using System.Text;

namespace Virtual_AGV
{
    public class PositionData
    {
        // 속성
        public int No { get; set; }
        public bool IsCross { get; set; }
        public List<PositionData> FrontList { get; set; }
        public List<PositionData> RearList { get; set; }
        public List<PositionData> LinkList { get; set; }
        public int Distance { get; set; }

        // 생성자
        public PositionData(int no, bool isCross)
        {
            this.No = no;
            this.IsCross = isCross;
            this.FrontList = new List<PositionData>();
            this.RearList = new List<PositionData>();
            this.LinkList = new List<PositionData>();
            this.Distance = 0;
        }

        // 외부함수
        public void AddFront(params PositionData[] posList)
        {
            this.FrontList.AddRange(posList);
            foreach (PositionData pos in posList)
            {
                if (this.LinkList.Contains(pos) == false)
                {
                    this.LinkList.Add(pos);
                }
                if (pos.LinkList.Contains(this) == false)
                {
                    pos.LinkList.Add(this);
                }
            }
        }
        public void AddRear(params PositionData[] posList)
        {
            this.RearList.AddRange(posList);
            foreach (PositionData pos in posList)
            {
                if (this.LinkList.Contains(pos) == false)
                {
                    this.LinkList.Add(pos);
                }
                if (pos.LinkList.Contains(this) == false)
                {
                    pos.LinkList.Add(this);
                }
            }
        }

        // Override
        public override string ToString()
        {
            // 변수 선언
            StringBuilder builder = new StringBuilder();

            // Pos
            builder.Append("Pos:" + No);

            // FrontList
            builder.Append(" Front:");
            foreach (PositionData pos in FrontList)
            {
                builder.Append(pos.No + ",");
            }
            if (FrontList.Count > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }

            // RearList
            builder.Append(" Rear:");
            foreach (PositionData pos in RearList)
            {
                builder.Append(pos.No + ",");
            }
            if (RearList.Count > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }

            // LinkList
            builder.Append(" Link:");
            foreach (PositionData pos in LinkList)
            {
                builder.Append(pos.No + ",");
            }
            if (LinkList.Count > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }

            // Distance
            builder.Append(" Dis:" + Distance);

            return builder.ToString();
        }
    }
}
