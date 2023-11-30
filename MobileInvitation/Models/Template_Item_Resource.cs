namespace MobileInvitation.Models
{
    /// <summary>
    /// Template_Item테이블과 Item_Resource테이블을 합쳐서 활용하는 별도 클래스
    /// </summary>
    public class Template_Item_Resource
    {
        public int item_id { get; set; }
        public int resource_id { get; set; }
        public string pid { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public double? top { get; set; }
        public double? left { get; set; }
        public double? height { get; set; }
        public double? width { get; set; }
        public string chracterset { get; set; }
        public double? fontsize { get; set; }
        public string fontcolor { get; set; }
        public string bgcolor { get; set; }
        public bool bold_yn { get; set; }
        public bool italic_yn { get; set; }
        public bool underline_yn { get; set; }
        public double? between_text { get; set; }
        public double? between_line { get; set; }
        public string vertical_align { get; set; }
        public string horizontal_align { get; set; }
        public int? zindex { get; set; }
        public string font { get; set; }
        public string resource_url { get; set; }
        public double? org_height { get; set; }
        public double? org_width { get; set; }
    }
}
