using System;

namespace GabTrans.Application.DataTransfer.GlobusBank;

public class GlobusStateListResponse
{
    public string responsecode { get; set; }
    public string responseMeassge { get; set; }
    public List<GlobusStateListDatum> data { get; set; }

    public class GlobusStateListDatum
    {
        public int id { get; set; }

        public string statename { get; set; }
    }
}
