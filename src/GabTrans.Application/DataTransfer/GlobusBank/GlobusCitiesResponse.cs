using System;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusCitiesResponse
{
    public string responsecode { get; set; }
    public string responseMessage { get; set; }
    public List<GlobusCitiesDatum> data { get; set; }

    public class GlobusCitiesDatum
    {
        public string city { get; set; }
    }
}
