using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Localemgmt.Application.Services.Messages;
using Localemgmt.Domain;

namespace Localemgmt.Application.src.Services.Messages
{
    public class GenericMessageService : IGenericMessageService
    {
        public GenericMessage GetGenericMessage()
        {
            return new GenericMessage();
        }
    }
}