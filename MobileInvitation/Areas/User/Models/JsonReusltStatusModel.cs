using System.Collections.Generic;

namespace MobileInvitation.Areas.User.Models
{
    public class JsonReusltStatusModel
    {
        public bool status { get; set; }
        public string message { get; set; }

        public List<string> errors { get; set; }
    }

    public class JsonOrderSaveResultModel
    {
        public string success { get; set; }
        public string result { get; set; }
        public string message { get; set; }
        public bool auth { get; set; }
    }

    public class JsonReusltRefundSaveModel
    {
        public bool status { get; set; }
        public string message1 { get; set; }
        public string message2 { get; set; }

    }
}
