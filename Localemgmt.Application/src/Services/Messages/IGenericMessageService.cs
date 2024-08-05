using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Localemgmt.Domain;

namespace Localemgmt.Application.Services.Messages
{
    public interface IGenericMessageService
    {
        GenericMessage GetGenericMessage();
    }
}