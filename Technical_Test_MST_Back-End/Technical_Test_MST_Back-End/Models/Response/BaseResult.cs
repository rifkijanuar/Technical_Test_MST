namespace Technical_Test_MST_Back_End.Models.Response
{
    public class BaseResult
    {
        public int StatusCode { get; set; }
        public int Count { get; set; }
        public dynamic Data { get; set; }
        public string Message { get; set; }
    }
}
