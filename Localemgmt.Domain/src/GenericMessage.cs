namespace Localemgmt.Domain
{
    public class GenericMessage
    {
        public string Content { get; set; }
        public string Code { get; set; }

        public GenericMessage()
        {
            Content = "no content";
            Code = "000";
        }
    }
}