using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThsCrmSamples.CrmDevSession1.Logic
{
    public interface ISampleDataAccess
    {
        IEnumerable<Contact> GetContacsByAccountId(Guid accountId);
    }
}
