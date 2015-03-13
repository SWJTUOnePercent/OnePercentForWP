using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DunKe
{
    /// <summary>
    /// 新闻类
    /// </summary>
    class News
    {
        /// <summary>
        /// 新闻的Id号
        /// </summary>
        private string newsId;

        public string NewsId
        {
            get { return newsId; }
            set { newsId = value; }
        }

        /// <summary>
        /// 新闻标题
        /// </summary>
        private string newsTitle;

        public string NewsTitle
        {
            get { return newsTitle; }
            set { newsTitle = value; }
        }


        /// <summary>
        /// 新闻缩略图
        /// </summary>
        private string newsThumbnail;

        public string NewsThumbnail
        {
            get { return newsThumbnail; }
            set { newsThumbnail = value; }
        }


        /// <summary>
        /// 新闻出处
        /// </summary>
        private string newsSource;

        public string NewsSource
        {
            get { return newsSource; }
            set { newsSource = value; }
        }


        /// <summary>
        /// 新闻发布时间
        /// </summary>
        private string newsPublishedTime;

        public string NewsPublishedTime
        {
            get { return newsPublishedTime; }
            set { newsPublishedTime = value; }
        }


        /// <summary>
        /// 阅读量
        /// </summary>
        private string newsReadCount;

        public string NewsReadCount
        {
            get { return newsReadCount; }
            set { newsReadCount = value; }
        }
    }
}
