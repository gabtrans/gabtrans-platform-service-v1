

namespace GabTrans.Application.Abstractions.Services
{
    public interface IAmountToWordService
    {
        string NumberToNaira(string myNumber);
        string NumberToDollar(string myNumber);
    }
}
