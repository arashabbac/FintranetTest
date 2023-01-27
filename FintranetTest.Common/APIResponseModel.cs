namespace FintranetTest.Common;

public class APIResponseModel
{
    public bool IsSuccess { get; set; }
    public string[] Messages { get; set; }
}

public class APIResponseModel<TData> : APIResponseModel
{
    public TData Data { get; set; }
}
