using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BMap.NET.WindowsForm.BMapElements
{
    class BVideoPoint : BMapElement
    {

        /// <summary>
        /// 位置坐标
        /// </summary>
        public LatLngPoint Location
        {
            get;
            set;
        }

        public override void Draw(Graphics g, LatLngPoint center, int zoom, Size screen_size)
        {
            throw new NotImplementedException();
        }
    }
}
