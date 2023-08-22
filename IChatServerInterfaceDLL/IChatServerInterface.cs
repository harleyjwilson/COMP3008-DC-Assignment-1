using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IChatServerInterfaceDLL
{
    [ServiceContract]
    public interface IChatServerInterface
    {
        [OperationContract]
        String GetName();
    }
}
