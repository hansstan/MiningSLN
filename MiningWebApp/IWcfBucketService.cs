using BurnWebApp.Models;
using System.ServiceModel;

namespace MiningWebApp
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWcfBucketService" in both code and config file together.
    [ServiceContract]
    public interface IWcfBucketService
    {
        [OperationContract]
        Bucket GetDataUsingDataContract(int size);
    }
}
