using Microsoft.AspNetCore.Mvc.Routing;
using System;

namespace MobileInvitation.Config
{
    public class BarunnConfig
    {
        public FileConfig FileConfig { get; set; }

        public NaverMapConfig Map { get; set; }

        public ImageSizeConfig MaxSize { get; set; }

        /// <summary>
        /// ASE 암호화 키 - 비밀에 저장 
        /// </summary>
        public string AesKey { get; set; }

        public StaticContent Sites { get; set; }

    }
    public class FileConfig
    {
        public long FileSizeLimit { get; set; }
        public string UploadPath { get; set; }
        public string UploadContainer { get; set; }
    }
    public class NaverMapConfig
    {
        public string NaverCloudId { get; set; }
        public string NaverCloudKey { get; set; }
        public double DefaultMapLat { get; set; }
        public double DefaultMapLot { get; set; }
    }
    public class StaticContent
    {
        public Uri Url { get; set; }
        /// <summary>
        /// 더이상 사용하지 않음.
        /// </summary>
        public Uri ServiceUrl { get; set; }
        public Uri CDNUrl { get; set; }
        public Uri PrivateApiUrl { get; set; }
        public Uri BarunFamilyUrl { get; set; }
        public Uri UserFileUrl { get; set; }

    }
    public class ImageSizeConfig
    {
        public int Thumnail { get; set; }
        public int Photo { get; set; }
        public int SNS { get; set; }
        public int Gallery { get; set; }
    }
}
