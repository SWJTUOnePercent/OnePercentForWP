using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaoDeMapAPI
{
    class Toilet
    {
        /// <summary>
        /// 厕所编号
        /// </summary>
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 厕所信息
        /// </summary>
        private string info;

        public string Info
        {
            get { return info; }
            set { info = value; }
        }

        /// <summary>
        /// 纬度
        /// </summary>
        private double latitude;

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        /// <summary>
        /// 纬度
        /// </summary>
        private double longitude;

        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

    }
}
